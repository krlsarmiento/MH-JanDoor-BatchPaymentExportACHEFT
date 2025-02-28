using PX.Data;
using PX.Objects.CA;
using PX.Objects.CS.DAC;
using System;
using System.Collections;
using PX.Objects.GL.DAC;
using PX.Objects.CR;
using PX.Objects.GL;
using PX.Data.BQL;
using ExportBatch.DAC_Ext.CS;
using PX.Objects.AP;
using ExportBatch.DAC_Ext.AP;
using PX.Common;
using System.IO;
using ExportBatch.DAC_Ext.CA;
using ExportBatch.Descriptor;
using ExportBatch.DAC;
using PX.Objects.CS;
using PX.Api;

namespace ExportBatch.Graph_Ext.CA
{
	public class CABatchEntryExportExt : PXGraphExtension<CABatchEntry>
	{
		public static bool IsActive() => true;

        #region Views
        public PXSelect<FileExportNumber,Where<FileExportNumber.exportType,Equal<Optional<FileExportNumber.exportType>>>> ExportNumberView;
        public PXSelectJoin<OrganizationBAccount,
									InnerJoin<Organization,
										On<Organization.bAccountID, Equal<BAccount.bAccountID>>>> OrganizationBAccountView;
		#endregion

		#region PXOverride
		public delegate IEnumerable ExportDelegate(PXAdapter adapter);
        [PXOverride]
        public IEnumerable Export(PXAdapter adapter, ExportDelegate baseMethod)
        {
            PaymentMethod paymentMethod = PXSelect<
                PaymentMethod, 
                Where<PaymentMethod.paymentMethodID, Equal<Current<CABatch.paymentMethodID>>>>
                .Select(Base);
            if (paymentMethod != null)
            {
                CDPaymentMethodExt dPaymentMethodExt = paymentMethod.GetExtension<CDPaymentMethodExt>();
                VerifySettings(dPaymentMethodExt.UsrCDExportFormatType);
                return ExportACHPayment(adapter, dPaymentMethodExt.UsrCDExportFormatType);
            }
            return baseMethod(adapter);
        }
		#endregion

		#region Export Process
		public virtual IEnumerable ExportACHPayment(PXAdapter adapter, string exportFormatType)
		{
			if (this.Base.Document.Current is CABatch batch)
			{
                PXLongOperation.StartOperation(Base.UID, delegate ()
                {
                    CABatchEntry batchEntry = PXGraph.CreateInstance<CABatchEntry>();
                    batchEntry.Document.Current = batchEntry.Document.Search<CABatch.batchNbr>(batch.BatchNbr);
                    CABatchEntryExportExt batchEntryExportExt = batchEntry.GetExtension<CABatchEntryExportExt>();

                    Branch currentBranch = Branch.PK.Find(batchEntry, batchEntry.Accessinfo.BranchID);
                    OrganizationBAccount organizationBAccount = batchEntryExportExt.GetOrganizationBAccount(batchEntry).Select(currentBranch.OrganizationID);
                    Address defAddress = PXSelect<Address, Where<Address.addressID, Equal<@P.AsInt>>>.Select(batchEntry, organizationBAccount.DefAddressID);
                    APSetup aPSetup = batchEntry.APSetup.SelectSingle();
                    APSetupExt setupExt = aPSetup.GetExtension<APSetupExt>();
                    string directory = AppDomain.CurrentDomain.BaseDirectory + "\\ExportACH";

                    var paymentmethodDetail = new PXSelect<PaymentMethodDetail,
                        Where<PaymentMethodDetail.paymentMethodID, Equal<@P.AsString>,
                            And<PaymentMethodDetail.descr, Equal<@P.AsString>>>>(batchEntry);
                    var paymentDetailView = new PXSelect<VendorPaymentMethodDetail,
                        Where<VendorPaymentMethodDetail.bAccountID, Equal<@P.AsInt>,
                            And<VendorPaymentMethodDetail.detailID, Equal<@P.AsString>>>>(batchEntry);
                    var vendorView = new PXSelect<Vendor,
                        Where<Vendor.bAccountID, Equal<@P.AsInt>>>(batchEntry);
                    var cashAccountOriginator = new PXSelect<CashAccount,
                        Where<CashAccount.cashAccountID, Equal<@P.AsInt>>>(batchEntry);
                    var addressView = new PXSelect<Address,
                        Where<Address.addressID, Equal<@P.AsInt>>>(batchEntry);

                    PaymentMethodDetail methodDetailRoutingNbrABA = paymentmethodDetail.Select(batch.PaymentMethodID, "Bank Routing Number (ABA):");
                    PaymentMethodDetail methodDetailReceiverAcctNbr = paymentmethodDetail.Select(batch.PaymentMethodID, "Beneficiary Account No:");
                    CashAccount cashAccount = cashAccountOriginator.Select(batch.CashAccountID);
                    string fielPath = string.Empty;
					string immediateOrigin = organizationBAccount.GetExtension<OrganizationBAccountExt>()?.UsrImmediateOriginator;
                    if (exportFormatType == CDExportFormatTypeListAttribute.Ach)
                    {
                        ACHExport exportProcess = new ACHExport();
                        string fileName = exportProcess.GenerateFileName(setupExt.UsrClientDefinedACHFileName, setupExt.UsrSystemMode);
                        string fileHeaderRecord = exportProcess.CreateFileHeader(immediateOrigin, batch.CreatedDateTime.Value, "A", organizationBAccount.AcctName.Trim());

                        string descr = !string.IsNullOrEmpty(batch.TranDesc) ? (batch.TranDesc.Length > 10 ? batch.TranDesc.Substring(0, 10).ToUpper() : batch.TranDesc) : string.Empty;
                        string batchNbr = batch.BatchNbr.Length > 7 ? batch.BatchNbr.Substring(batch.BatchNbr.Length - 7) : batch.BatchNbr;
                        string batchHeaderRecord = exportProcess.CreateBatchHeader(immediateOrigin, descr, PXTimeZoneInfo.Now, batchNbr);

                        string detailsWithAddenda = string.Empty;
                        bool indexFirstIdentifyFlag = true;
                        int recordCount = 0;
                        int entryHashNumber = 0;
                        decimal totalCreditEntryDollarAmount = 0;
                        decimal totalDebitEntryDollarAmount = 0;

                        foreach (PXResult<CABatchDetail, APPayment> paymentDetails in batchEntry.BatchPayments.Select())
                        {
                            recordCount += 1;
                            CABatchDetail aBatchDetail = paymentDetails;
                            APPayment aPPayment = paymentDetails;
                            string transactionCode = aPPayment.DocType == APDocType.Check ? "22" : "27";
                            if (transactionCode == "22")
                            {
                                totalCreditEntryDollarAmount += aPPayment.CuryOrigDocAmt.GetValueOrDefault();
                            }
                            else
                            {
                                totalDebitEntryDollarAmount += aPPayment.CuryOrigDocAmt.GetValueOrDefault();
                            }

                            VendorPaymentMethodDetail vendorPaymentMethodDetail = paymentDetailView.Select(aPPayment.VendorID, methodDetailRoutingNbrABA.DetailID);
                            VendorPaymentMethodDetail vPmtDetReceiverAcctNbr = paymentDetailView.Select(aPPayment.VendorID, methodDetailReceiverAcctNbr.DetailID);
                            Vendor vendor = vendorView.Select(aPPayment.VendorID);
                            Address vendorAddress = addressView.Select(vendor.DefAddressID);
                            string traceNbr = aPPayment.ExtRefNbr?.Trim();
                            string traceNbrLast7Digits = traceNbr != null ? (traceNbr.Length > 7 ? traceNbr.Substring(traceNbr.Length - 7) : traceNbr) : string.Empty;

                            string originatorName = organizationBAccount.AcctName.Trim();
                            string originatorStreetAddress = (defAddress.AddressLine1 ?? defAddress.AddressLine2)?.Trim();
                            string originatorCityStateProvince = defAddress.City?.Trim() + "*" + defAddress.State?.Trim() + "\\";
                            string originatorCountryPostalCode = defAddress.CountryID?.Trim() + "*" + defAddress.PostalCode?.Trim() + "\\";

                            string receiversAccountNumber = vPmtDetReceiverAcctNbr.DetailValue.Trim();
                            string receiverName = vendor.AcctName.Trim();
                            string receivingDfiIdentification = vendorPaymentMethodDetail.DetailValue.Trim();
                            string receiverStreetAddress = (vendorAddress.AddressLine1 ?? vendorAddress.AddressLine2)?.Trim();
                            string receiverCityStateProvince = vendorAddress.City?.Trim() + "*" + vendorAddress.State?.Trim() + "\\";
                            string receiverCountryPostalCode = vendorAddress.CountryID?.Trim() + "*" + vendorAddress.PostalCode?.Trim() + "\\";

                            if (int.TryParse(receivingDfiIdentification, out int converted))
                            {
                                entryHashNumber += converted;
                            }
                            if (!indexFirstIdentifyFlag)
                            {
                                detailsWithAddenda += "\n";
                            }
                            else
                            {
                                indexFirstIdentifyFlag = false;
                            }
                            detailsWithAddenda += exportProcess.CreateEntryDetail(transactionCode, receivingDfiIdentification, aPPayment.CuryOrigDocAmt.ToString(), receiversAccountNumber, traceNbr);
                            detailsWithAddenda += "\n";
                            detailsWithAddenda += exportProcess.CreateFirstAddenda(receiversAccountNumber, receiverName, traceNbrLast7Digits);
                            detailsWithAddenda += "\n";
                            detailsWithAddenda += exportProcess.CreateSecondAddenda(originatorName, originatorStreetAddress, traceNbrLast7Digits);
                            detailsWithAddenda += "\n";
                            detailsWithAddenda += exportProcess.CreateThirdAddenda(originatorCityStateProvince, originatorCountryPostalCode, traceNbrLast7Digits);
                            detailsWithAddenda += "\n";
                            detailsWithAddenda += exportProcess.CreateFourthAddenda(traceNbrLast7Digits);
                            detailsWithAddenda += "\n";
                            detailsWithAddenda += exportProcess.CreateFifthAddenda(receiverName, receivingDfiIdentification, traceNbrLast7Digits);
                            detailsWithAddenda += "\n";
                            detailsWithAddenda += exportProcess.CreateSixthAddenda(receiverName, receiverStreetAddress, traceNbrLast7Digits);
                            detailsWithAddenda += "\n";
                            detailsWithAddenda += exportProcess.CreateSeventhAddenda(receiverCityStateProvince, receiverCountryPostalCode, traceNbrLast7Digits);
                            detailsWithAddenda += "\n";
                            detailsWithAddenda += exportProcess.CreateRemittanceAddenda(traceNbrLast7Digits);
                        }
                        string entryAddendaCount = (recordCount * 9).ToString();
                        int totalAddendaCount = (recordCount * 8);
                        string batchControl = exportProcess.CreateBatchControl(entryAddendaCount, entryHashNumber.ToString(), totalDebitEntryDollarAmount.ToString(), totalCreditEntryDollarAmount.ToString(), immediateOrigin, batchNbr);

                        int totalRecordCount = (recordCount * 9) + 4;
                        string blockCount = ((totalRecordCount / 10) + (totalRecordCount % 10 > 0 ? 1 : 0)).ToString();

                        string fileTrailerControl = exportProcess.CreateFileTrailerControl("1", blockCount, totalAddendaCount.ToString(), entryHashNumber.ToString(), totalDebitEntryDollarAmount.ToString(), totalCreditEntryDollarAmount.ToString());

                        fielPath = exportProcess.PrepareACHFileContent(directory, fileName, fileHeaderRecord, batchHeaderRecord, detailsWithAddenda, batchControl, fileTrailerControl);
                    }
                    else if (exportFormatType == CDExportFormatTypeListAttribute.Eft)
                    {
                        EFTExport eFTExport = new EFTExport();

                        string fileName =System.String.Empty;
                        CABatchExt batchExt= batchEntry.Document.Current.GetExtension<CABatchExt>();
                        if (String.IsNullOrEmpty(batchExt.UsrExportSerialNbr))
                        {
                            fileName = AutoNumberAttribute.GetNextNumber(batchEntry.Document.Cache, batchEntry.Document.Current, setupExt.UsrExportSerialNbr, batchEntry.Document.Current.TranDate);
                            batchExt.UsrExportSerialNbr = fileName;
                            batchEntry.Document.UpdateCurrent();
                        }
                        else {
                            fileName = batchExt.UsrExportSerialNbr;  
                        }
                        //string fileName = eFTExport.GenerateFileName(setupExt.UsrClientDefinedACHFileName, setupExt.UsrSystemMode, setupExt.UsrEFtChannel);
                        if (!String.IsNullOrEmpty(fileName))
                        {
                            fileName = fileName + ".txt";
                        }
                        else {
                            throw new PXException(MessagesExport.EFT_CHANNEL_SERIAL_NUMBER_REQUIRED);
                        }
                        
                        FileExportNumber exportNumber = batchEntryExportExt.ExportNumberView.Select("EFT");
                        //string fileCreationNumber = exportNumber != null && exportNumber.ExportNumber.HasValue && exportNumber.ExportNumber!=9999 ? (exportNumber.ExportNumber+1).ToString() : "1";
                        string fileCreationNumber = batchExt.UsrExportSerialNbr.Substring(batchExt.UsrExportSerialNbr.ToString().Length - 4, 4);

                        if (immediateOrigin.Length!=10)
                        {
                            throw new PXException("Originator Number must be 10 character");
                        }
                        
                        if (exportNumber != null)
                        {
                            exportNumber.ExportNumber = exportNumber.ExportNumber.GetValueOrDefault() + 1;
                            exportNumber = batchEntryExportExt.ExportNumberView.Update(exportNumber);
                        }
                        else
                        {
                            exportNumber = new FileExportNumber()
                            {
                                ExportType = "EFT",
                                ExportNumber = 1
                            };
                            exportNumber = batchEntryExportExt.ExportNumberView.Insert(exportNumber);
                        }


                        CashAccountPaymentMethodDetail TRANSIT = CashAccountPaymentMethodDetail.PK.Find(batchEntry, batchEntry.Document.Current.CashAccountID, batchEntry.Document.Current.PaymentMethodID, "TRANSIT");
                        if (TRANSIT == null)
                        {
                            throw new PXException(MessagesExport.TRANSIT_REQUIRED);
                        }
                        else
                        {
                            if (TRANSIT.DetailValue == null)
                            {
                                throw new PXException(MessagesExport.TRANSIT_REQUIRED);
                            }
                        }
                        string branchTransitNumber = TRANSIT.DetailValue;
                        

                        CashAccountPaymentMethodDetail Account = CashAccountPaymentMethodDetail.PK.Find(batchEntry, batchEntry.Document.Current.CashAccountID, batchEntry.Document.Current.PaymentMethodID, "ACCOUNT");
                        if (Account == null)
                        {
                            throw new PXException(MessagesExport.ACCOUNT_REQUIRED);
                        }
                        else
                        {
                            if (Account.DetailValue == null)
                            {
                                throw new PXException(MessagesExport.ACCOUNT_REQUIRED);
                            }
                        }

                        if (branchTransitNumber.ToString().Trim().Length!=5)
                        {
                            throw new PXException("Branch Transit Number length must be 5 ");
                        }

                        string _accountnumber = Account.DetailValue;

                        string destinationDataCenter = immediateOrigin.Substring(0, 5);
                        string accountNumber = _accountnumber?.Length > 7 ? _accountnumber.Substring(0, 7) : _accountnumber;
                        string originatorsShortName = organizationBAccount.AcctName.Trim().Length > 15 ? organizationBAccount.AcctName.Trim().Substring(0, 15) : organizationBAccount.AcctName.Trim();
                        string currencyIndicator = batch.CuryID.Trim();

                        if (originatorsShortName.ToString().Length < 15)
                        {
                            throw new PXException("Originators Short Name length must be 15 ");
                        }

                        if (accountNumber.ToString().Length<7)
                        {
                            throw new PXException("Account number length must be 7 ");
                        }
                        if (currencyIndicator.ToString().Length != 3)
                        {
                            throw new PXException("Currency Indicator length must be 3 ");
                        }
                        string fileHeader = eFTExport.CreateFileHeader(destinationDataCenter, immediateOrigin, batch.CreatedDateTime.Value.ToString("yyMMdd"), fileCreationNumber.PadLeft(4, '0'), branchTransitNumber, accountNumber, originatorsShortName, currencyIndicator);

                        string transactionCode = "460";// "200";
                        string paymentSundryInformation = batch.BatchSeqNbr?.Length > 10 ? batch.BatchSeqNbr.Substring(batch.BatchSeqNbr.Length - 10) : batch.BatchSeqNbr;
                        paymentSundryInformation = paymentSundryInformation.PadRight(10);
                        string valueDate = batch.TranDate.Value.ToString("yyMMdd");
                         
                        string batchHeader = eFTExport.CreateBatchHeader(transactionCode, paymentSundryInformation, valueDate);

                        //string crossReferenceNumber = batch.BatchSeqNbr?.Length > 13 ? batch.BatchSeqNbr.Substring(batch.BatchSeqNbr.Length - 13) : batch.BatchSeqNbr;
                        string crossReferenceNumber = "";

                        bool indexFirstIdentifyFlag = true;
						string details = string.Empty;
                        int recordCount = 0;
                        decimal origDocAmountTotals = 0m;
                        foreach (PXResult<CABatchDetail, APPayment> paymentDetails in batchEntry.BatchPayments.Select())
                        {
                            CABatchDetail aBatchDetail = paymentDetails;
                            APPayment aPPayment = paymentDetails;
                            Vendor vendor = vendorView.Select(aPPayment.VendorID);
                            VendorPaymentMethodDetail vendorPaymentMethodDetailRoutingNbr = paymentDetailView.Select(aPPayment.VendorID, methodDetailRoutingNbrABA.DetailID);

                            VendorPaymentMethodDetail vendorPaymentMethodDetailBankInstitutionID = VendorPaymentMethodDetail.PK.Find(batchEntry, aPPayment.VendorID, aPPayment.VendorLocationID, batchEntry.Document.Current.PaymentMethodID, "BANK");
                            if (vendorPaymentMethodDetailBankInstitutionID==null)
                            {
                                throw new PXException(MessagesExport.BANKINSTITUTIONID_REQUIRED);
                            }
                            if (vendorPaymentMethodDetailBankInstitutionID.DetailValue == null)
                            {
                                throw new PXException(MessagesExport.BANKINSTITUTIONID_REQUIRED);
                            }


                            string receiverBankInstitutionID = vendorPaymentMethodDetailBankInstitutionID.DetailValue?.Length > 4 ? vendorPaymentMethodDetailBankInstitutionID.DetailValue.Substring(0, 4) : vendorPaymentMethodDetailBankInstitutionID.DetailValue; ;
                            string creditDebitIdentifier = aPPayment.DocType == APDocType.Check ? "C" : "D";



                            VendorPaymentMethodDetail receiverBankTransitNumberView = VendorPaymentMethodDetail.PK.Find(batchEntry, aPPayment.VendorID, aPPayment.VendorLocationID, batchEntry.Document.Current.PaymentMethodID, "TRANSIT");
                            if (receiverBankTransitNumberView == null)
                            {
                                throw new PXException(MessagesExport.VENDOR_TRANSIT_REQUIRED);
                            }
                            if (receiverBankTransitNumberView.DetailValue == null)
                            {
                                throw new PXException(MessagesExport.VENDOR_TRANSIT_REQUIRED);
                            }
                            string receiverBankTransitNumber = receiverBankTransitNumberView.DetailValue?.Length > 5 ? receiverBankTransitNumberView.DetailValue.Substring(0, 5) : receiverBankTransitNumberView.DetailValue;
                            if (receiverBankTransitNumber.ToString().Length < 5)
                            {
                                throw new PXException("Receiver Bank Transit Number length must be 5");
                            }

                            VendorPaymentMethodDetail receiverAccountNumberView = VendorPaymentMethodDetail.PK.Find(batchEntry, aPPayment.VendorID, aPPayment.VendorLocationID, batchEntry.Document.Current.PaymentMethodID, "ACCOUNT");
                            if (receiverAccountNumberView == null)
                            {
                                throw new PXException(MessagesExport.VENDOR_ACCOUNT_REQUIRED);
                            }
                            if (receiverAccountNumberView.DetailValue == null)
                            {
                                throw new PXException(MessagesExport.VENDOR_ACCOUNT_REQUIRED);
                            }
                           
                            string receiverAccountNumber = receiverAccountNumberView.DetailValue?.Length > 12 ? receiverAccountNumberView.DetailValue.Substring(0,12) : receiverAccountNumberView.DetailValue;



                            crossReferenceNumber = aPPayment.RefNbr?.Length > 13 ? aPPayment.RefNbr.Substring(aPPayment.RefNbr.Length - 13) : aPPayment.RefNbr;
                            string receiverName = vendor.AcctName.Trim().Length > 22 ? vendor.AcctName.Trim().Substring(0, 22) : vendor.AcctName.Trim();

                            if (!indexFirstIdentifyFlag)
                            {
                                details += "\n";
                            }
                            else
                            {
                                indexFirstIdentifyFlag = false;
                            }

                            string amt1 = aPPayment.CuryOrigDocAmt.GetValueOrDefault().ToString().Split('.')[0];
                            string cents = aPPayment.CuryOrigDocAmt.GetValueOrDefault().ToString().Split('.')[1]; 
                            string _amount = amt1.PadLeft(8, '0') + cents.PadLeft(2, '0').Substring(0,2);

                            details += eFTExport.CreateDetail(receiverBankInstitutionID,creditDebitIdentifier, receiverBankTransitNumber, receiverAccountNumber, _amount, crossReferenceNumber, receiverName);
                            recordCount += 1;
                            origDocAmountTotals += aPPayment.CuryOrigDocAmt.GetValueOrDefault();
                        }
                        string batchEntryCount = recordCount.ToString();

                        string amt2 = origDocAmountTotals.ToString().Split('.')[0];
                        string cents2 = origDocAmountTotals.ToString().Split('.')[1];
                        
                        string entryDollarAmount = amt2.PadLeft(10, '0') + cents2.PadLeft(2, '0').Substring(0,2);
              
                        string batchTrailer = eFTExport.CreateBatchTrailer(transactionCode, batchEntryCount, entryDollarAmount);
                        string fileTrailer = eFTExport.CreateFileTrailer("1", batchEntryCount);
						fielPath = eFTExport.PrepareACHFileContent(directory, fileName, fileHeader, batchHeader, details, batchTrailer, fileTrailer);
					}
					SaveFileAsAttachment(batchEntry, fielPath, ".txt");
					batchEntry.Save.PressButton();
					File.Delete(fielPath);
				});
			}
			return adapter.Get();
		}

		#endregion
		#region helper
        public virtual void VerifySettings(string exportFormatType)
        {
            APSetup aPSetup = Base.APSetup.SelectSingle();
            APSetupExt setupExt = aPSetup.GetExtension<APSetupExt>();
            if (string.IsNullOrEmpty(setupExt.UsrClientDefinedACHFileName))
            {
                throw new PXException(MessagesExport.CLIENT_DEFINED_ACH_FILE_NAME_IS_REQUIRED);
            }
            if (string.IsNullOrEmpty(setupExt.UsrSystemMode))
            {
                throw new PXException(MessagesExport.SYSTEM_MODE_IS_REQUIRED);
            }
            Branch currentBranch = Branch.PK.Find(this.Base, this.Base.Accessinfo.BranchID);
            OrganizationMaint organizationMaint = PXGraph.CreateInstance<OrganizationMaint>();
			OrganizationBAccount organizationBAccount = GetOrganizationBAccount(organizationMaint).Select(currentBranch.OrganizationID);
            if (string.IsNullOrEmpty(organizationBAccount.GetExtension<OrganizationBAccountExt>()?.UsrImmediateOriginator))
            {
                throw new PXException(MessagesExport.IMMEDIATE_ORIGINATOR_IS_REQUIRED);
            }
            if (exportFormatType == CDExportFormatTypeListAttribute.Eft)
            {
                if (string.IsNullOrEmpty(setupExt.UsrEFtChannel))
                {
                    throw new PXException(MessagesExport.EFT_CHANNEL_IS_REQUIRED);
                }
			}

		}
        private static void SaveFileAsAttachment(CABatchEntry batchEntry, string fielPath, string extension)
        {
            if (!string.IsNullOrEmpty(fielPath))
            {
                string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fielPath);
                byte[] fileBytes = File.ReadAllBytes(fielPath);
                var upl = PXGraph.CreateInstance<PX.SM.UploadFileMaintenance>();
                PX.SM.FileInfo fileinfo = new PX.SM.FileInfo(fileNameWithoutExtension + extension, null, fileBytes);
                if (upl.SaveFile(fileinfo, PX.SM.FileExistsAction.CreateVersion) && fileinfo.UID.HasValue)
                {
                    PXNoteAttribute.SetFileNotes(batchEntry.Document.Cache, batchEntry.Document.Current, fileinfo.UID.Value);
                }
            }
        }
        #endregion

        #region DB
        public virtual PXSelectBase<OrganizationBAccount> GetOrganizationBAccount(PXGraph batchEntry)
		{
			return new PXSelectJoin<OrganizationBAccount,
									InnerJoin<Organization,
										On<Organization.bAccountID, Equal<BAccount.bAccountID>>>,
									Where<Organization.organizationID, Equal<@P.AsInt>>>(batchEntry);
		}
		#endregion
	}
}