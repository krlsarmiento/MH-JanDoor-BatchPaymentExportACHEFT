namespace ExportBatch.Models.ACH
{
	public class BatchControlRecord
	{
		//RECORD TYPE CODE
		public string RecordTypeCode { get; set; }
		public string ServiceClassCode { get; set; }
		public string EntryAddendaCount { get; set; }
		public string EntryHash { get; set; }
		public string TotalDebitEntryDollarAmount { get; set; }
		public string TotalCreditEntryDollarAmount { get; set; }
		public string CompanyIdentification { get; set; }
		//MESSAGE AUTHENTICATION CODE
		public string MessageAuthenticationCode { get; set; }
		//RESERVED
		public string Reserved { get; set; }
		public string OriginatingDFIIdentification { get; set; }
		public string BatchHeader { get; set; }

		public BatchControlRecord(string entryAddendaCount, string entryHash, string totalDebitEntryDollarAmount,
			string totalCreditEntryDollarAmount, string companyIdentification, string batchHeader)
		{
			RecordTypeCode = "8";// [lenght 1] Must use ‘8’ 
			ServiceClassCode = "200";// [lenght 3] Must be the same as the 2nd field in the Batch Header(position 2 - 4)
			EntryAddendaCount = entryAddendaCount.PadRight(6);// [lenght 6] Must be equal to the number of detail records in the batch Include both entry detail and addenda records
			EntryHash = entryHash.PadRight(10);// [lenght 10] The sum of positions 4 - 11 of all Entry Detail (6) Records in the batch
			TotalDebitEntryDollarAmount = totalDebitEntryDollarAmount.PadRight(12,'0');// [lenght 12] Must equal the total of all debit payment amounts in this batch
			TotalCreditEntryDollarAmount = totalCreditEntryDollarAmount.PadRight(12, '0');// [lenght 12] Must equal the total of all credit payment amounts in this batch
			CompanyIdentification = companyIdentification;// [lenght 10] Must match the Batch Header Record, Field 8 (position 41 - 50)
			MessageAuthenticationCode = string.Empty.PadRight(19);// [lenght 19] Fill field with blank spaces
			Reserved = string.Empty.PadRight(6);// [lenght 6] Fill field with blank spaces
			OriginatingDFIIdentification = "02100002";// [lenght 8] Same as the value in field 16 of Batch Header(position 80 87)
			BatchHeader = batchHeader;// [lenght 7] Same as the value in field 17 of Batch Header(position 88 - 94)
		}

		public override string ToString()
		{
			string record = RecordTypeCode + ServiceClassCode + EntryAddendaCount.PadRight(6) + EntryHash + TotalDebitEntryDollarAmount 
							+ TotalCreditEntryDollarAmount + CompanyIdentification + MessageAuthenticationCode
							+ Reserved + OriginatingDFIIdentification + BatchHeader;
			return record;
		}
	}
}
