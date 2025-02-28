namespace ExportBatch.Models.EFT
{
    // File Trailer Record
    public class FileTrailer
    {
        public string RecordType { get; set; }
        public string BatchCount { get; set; }
        public string DetailCount { get; set; }
        public string Filter { get; set; }

        public FileTrailer(string batchCount, string detailCount)
        {
            RecordType = "9";//[lenght 1] Always ‘9’ File trailer Indicator
            /*
             * [lenght 6]
             * Total number of ‘5’ records (batch records) for reconciliation purposes
                Right justified and filled with leading zeros
                Example: 000012
             */
            BatchCount = batchCount.PadLeft(6,'0');
            /*
             * [lenght 6]
             * Total number of ‘6’ records (detail records) right justified and filled with leading zeros
                Example: 000012
             */
            DetailCount = detailCount.PadLeft(6, '0');
            Filter = string.Empty.PadRight(67);//[lenght 67] Space fill
        }
		public override string ToString()
		{
            string fileTrailer = RecordType + BatchCount + DetailCount + Filter;
			return fileTrailer;
		}
	}
}
