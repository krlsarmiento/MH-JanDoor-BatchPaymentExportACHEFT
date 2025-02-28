namespace ExportBatch.Models.ACH
{
	public class EntryDetailRecord
	{
		public string RecordTypeCode { get; set; }
		public string TransactionCode { get; set; }
		public string ReceivingDFIIdentificationAndCheckDigit { get; set; }
		//NUMBER OF ADDENDA RECORDS
		public string NumberOfAddendaRecords { get; set; }
		//RESERVED
		public string Reserved { get; set; }
		public string Amount { get; set; }
		//RECEIVER’S ACCOUNT NUMBER
		public string ReceiversAccountNumber { get; set; }
		//RESERVED
		public string Reserved1 { get; set; }
		//GATEWAY OPERATOR OFAC SCREENING INDICATOR
		public string GatewayOperatorOfacScreeningIndicator { get; set; }
		//SECONDARY OFAC SCREENING INDICATOR
		public string SecondaryOfacScreeningIndicator { get; set; }
		//ADDENDA RECORD INDICATOR
		public string AddendaRecordIndicator { get; set; }
		public string TraceNumber { get; set; }

		public EntryDetailRecord(string transactionCode, string receivingDFIIdentificationAndCheckDigit,  string amount, string receiversAccountNumber, string traceNumber)
		{
			RecordTypeCode = "6";// [lenght 1] Must use ‘6’  
			/*
			 *  [lenght 2]
			 * Must use one of the following: 
				22 For Chequing Credit  
				27 For Chequing Debit  
				32 For Savings Credit 
				37 For Savings Debit
			 */
			TransactionCode = transactionCode;
			ReceivingDFIIdentificationAndCheckDigit = receivingDFIIdentificationAndCheckDigit.PadRight(9);//[lenght 9] Contains a valid  9 digit ABA bank routing number and is used to identify the RDFI in which the receiver maintains an account
			NumberOfAddendaRecords = "8".PadLeft(4,'0');// [lenght 4] Total number of addenda records associated to transaction Right justified and zero filled
			Reserved = string.Empty.PadLeft(13); // [lenght 13] Field is space filled
			/*
			 * [lenght 10]
			 * Amount in USD Maximum dollar amount per payment is $99,999,999.99 
				Must be greater than zero  
				Decimal place is implied 
				Right justified with leading zeros 
				Example: ‘0542458175’ = $5,424,581.75
			 */
			Amount = amount.PadLeft(10,'0');
			/*
			 *  [lenght 35]
			 * This field contains the Receiver’s account number as assigned by the Receiving Depository Financial Institution 
				Must be left justified and space filled 
			 */
			ReceiversAccountNumber = receiversAccountNumber.PadRight(35);
			Reserved1 = string.Empty.PadLeft(2); // [lenght 2] Field is space filled
			GatewayOperatorOfacScreeningIndicator = string.Empty.PadLeft(1); // [lenght 1] Field is space filled;
			SecondaryOfacScreeningIndicator = string.Empty.PadLeft(1); // [lenght 1] Field is space filled;
			AddendaRecordIndicator = "1";// [lenght 1] Must use ‘1’  
			TraceNumber = traceNumber;//[lenght 15] Unique payment number, must be unique within each file
		}
		public override string ToString()
		{
			string entryDetailRecord = RecordTypeCode + TransactionCode + ReceivingDFIIdentificationAndCheckDigit + NumberOfAddendaRecords + Reserved + Amount + ReceiversAccountNumber + Reserved1 + GatewayOperatorOfacScreeningIndicator + SecondaryOfacScreeningIndicator + AddendaRecordIndicator + TraceNumber;
			return entryDetailRecord;
		}
	}
}