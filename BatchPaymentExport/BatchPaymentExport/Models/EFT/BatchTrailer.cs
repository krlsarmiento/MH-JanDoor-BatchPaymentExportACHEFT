using System;

namespace ExportBatch.Models.EFT
{
    // Batch Trailer Record
    public class BatchTrailer
    {
        public string RecordType { get; set; }
        public string TransactionCode { get; set; }
        public string BatchEntryCount { get; set; }
        public string Reserved { get; set; }
        public string Filler { get; set; }
        public string EntryDollarAmount { get; set; }
        public string Filler7 { get; set; }

        public BatchTrailer( string transactionCode, string batchEntryCount, string entryDollarAmount)
        {
            RecordType = "7";// [lenght 1] Always ‘7’ Batch header indicator
            /*
             *  [lenght 3]
             * Must match CPA transaction type code used in Batch Header
                Example: ‘331’ = Life Insurance
             */
            TransactionCode = transactionCode;
            /*
             *  [lenght 6]
             * Indicates the total number of detail records ('6') in a batch.
                Right justified and filled with leading zeros
                Example: 000012
             */
            BatchEntryCount = batchEntryCount.PadLeft(6,'0');
            Reserved = string.Empty.PadRight(10, '0');// [lenght 10] Zero fill
            Filler = string.Empty.PadRight(20);// [lenght 20] Space fill
            /*
             *  [lenght 12]
             * The total dollar value of detail records (Record type ‘6’)
                Right justified and filled with zeros
                Example: 000000430956
             */
            
            EntryDollarAmount = entryDollarAmount;
            Filler7 = string.Empty.PadRight(28);// [lenght 28] Space fill
        }

        public override string ToString()
        {
            string batchTrailer = RecordType + TransactionCode + BatchEntryCount + Reserved + Filler + EntryDollarAmount + Filler7;
            return batchTrailer;
        }
    }
}
