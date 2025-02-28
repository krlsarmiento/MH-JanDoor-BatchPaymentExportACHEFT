namespace ExportBatch.Models.ACH
{
	public class FileTrailerControlRecord
	{
		//RECORD TYPE CODE
		public string RecordTypeCode { get; set; }
		public string BatchCount { get; set; }
		public string BlockCount { get; set; }
		public string EntryAddendaCount { get; set; }
		public string EntryHash { get; set; }
		public string TotalDebitEntryDollarAmountInFile { get; set; }
		public string TotalCreditEntryDollarAmountInFile { get; set; }
		//RESERVED
		public string Reserved { get; set; }

		public FileTrailerControlRecord(string batchCount, string blockCount, string entryAddendaCount, string entryHash, string totalDebitEntryDollarAmountInFile, string totalCreditEntryDollarAmountInFile)
		{
			RecordTypeCode = "9";// [lenght 1] Must use ‘9’ 
			BatchCount = batchCount.PadRight(6);// [lenght 6] Must be equal to the number of batches in the file
			BlockCount = blockCount.PadRight(6);// [lenght 6] Must be equal to the number of blocks in the file(e.g., ten lines of data equal ‘1’ block)
			EntryAddendaCount = entryAddendaCount.PadRight(8);// [lenght 8] Must be equal to the number of detail and addenda records in the file
			EntryHash = entryHash.PadRight(10);// [lenght 10] The sum of positions 11 - 20 of all Batch Control Records
			TotalDebitEntryDollarAmountInFile = totalDebitEntryDollarAmountInFile.PadRight(12);// [lenght 12] The sum of positions 21 - 32 of all Batch Control Records
			TotalCreditEntryDollarAmountInFile = totalCreditEntryDollarAmountInFile.PadRight(12);// [lenght 12] The sum of positions 33 - 44 of all Batch Control Records
			Reserved = string.Empty.PadRight(39);// [lenght 39] Fill field with blank spaces
		}

		public override string ToString()
		{
			string result = RecordTypeCode + BatchCount + BlockCount + EntryAddendaCount + EntryHash + TotalDebitEntryDollarAmountInFile + TotalCreditEntryDollarAmountInFile + Reserved;
			return result;
		}
	}
}
