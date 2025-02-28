using System;

namespace ExportBatch.Models.ACH
{
	//File header record (5) 
	public class BatchHeaderRecord
	{
		public string RecordTypeCode { get; set; }
		public string ServiceClassCode { get; set; }
		public string IatIndicator { get; set; }
		public string ForeignExchangeIndicator { get; set; }
		public string ForeignExchangeReferenceIndicator { get; set; }
		public string ForeignExchangeReference { get; set; }
		public string IsoDestinationCountryCode { get; set; }
		public string OriginatorIdentification { get; set; }
		public string StandardEntryClass { get; set; }
		public string CompanyEntryDescription { get; set; }
		public string IsoOriginatingCurrencyCode { get; set; }
		public string IsoDestinationCurrencyCode { get; set; }
		public string EffectiveEntryDate { get; set; }
		public string SettlementDate { get; set; }
		public string OriginatorStatusCode { get; set; }
		public string OriginatingDFIIdentification { get; set; }
		public string BatchNumber { get; set; }

		public BatchHeaderRecord(string originatorIdentification, string companyEntryDescription, DateTime effectiveEntryDate, string batchNumber)
		{
			RecordTypeCode = "5";// [lenght 1] Must use ‘5’ 
			/*
				ServiceClassCode: Must use one of the following: 
									200 - Mixed Debits & Credits 
									220 - Credits only 
									225 - Debits only 
			 */
			ServiceClassCode = "200";
			IatIndicator = string.Empty.PadLeft(16);// [lenght 16] BLANK Field is space filled 
			ForeignExchangeIndicator = "FF";// [lenght 2] Must use ‘FF’ 
			ForeignExchangeReferenceIndicator = "3";// [lenght 1] Must use ‘3’ 
			ForeignExchangeReference = string.Empty.PadLeft(15);// [lenght 15] BLANK Field is space filled 
			IsoDestinationCountryCode = "US";// [lenght 2] ISO Country Code where the IAT will be sent Must use ‘US’ 
			OriginatorIdentification = originatorIdentification;// [lenght 10] Same as the Originator Number in the File Header
			StandardEntryClass = "IAT";// [lenght 10] Must use ‘IAT’ 
			CompanyEntryDescription = companyEntryDescription.PadRight(10);// [lenght 10] Description of each payment in this batch Example: PAYROLL
			IsoOriginatingCurrencyCode = "USD";// [lenght 3] Must use ‘USD’ 
			IsoDestinationCurrencyCode = "USD";// [lenght 3] Must use ‘USD’ 
			EffectiveEntryDate = effectiveEntryDate.ToString("YYMMDD");// [lenght 6] Value date of payments within this batch
			SettlementDate = string.Empty.PadLeft(3);// [lenght 3] Field is space filled 
			OriginatorStatusCode = "1";// [lenght 1] Must use ‘1’ 
			OriginatingDFIIdentification = "02100002";// [lenght 8] Must use ‘02100002’ 
			BatchNumber = batchNumber.PadLeft(7);//TODO: get the field [lenght 7] This is the batch number on which the batch level unique check is done. The batch numbers should be unique within a file.
		}

		public override string ToString()
		{
			string batchHeaderRecord = RecordTypeCode + ServiceClassCode 
									+ IatIndicator + ForeignExchangeIndicator 
									+ ForeignExchangeReferenceIndicator + ForeignExchangeReference 
									+ IsoDestinationCountryCode + OriginatorIdentification 
									+ StandardEntryClass + CompanyEntryDescription 
									+ IsoOriginatingCurrencyCode + IsoDestinationCurrencyCode
									+ EffectiveEntryDate + SettlementDate 
									+ OriginatorStatusCode + OriginatingDFIIdentification
									+ BatchNumber;
			return batchHeaderRecord;
		}
	}
}