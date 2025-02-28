namespace ExportBatch.Models.EFT
{
	// Batch Header Record
	public class BatchHeader
    {
        public string RecordType { get; set; } 
        public string Filler { get; set; }
        public string TransactionCode { get; set; }
        public string PaymentSundryInformation { get; set; }
        public string ValueDate { get; set; }
        public string Filler6 { get; set; }

        public BatchHeader(string transactionCode, string paymentSundryInformation, string valueDate)
        {
            RecordType = "5";//[lenght 1] Batch Header indicator
            Filler = string.Empty.PadRight(46);//[lenght 46] Space fill
            /*
             * [lenght 3]
             * A valid CPA transaction type code
                Will be assigned to each payment within this batch
                Refer to the EFT reference guide appendix for a complete listing of CPA transaction codes
                Example: ‘331’ = Life Insurance
             */
            TransactionCode = transactionCode;
            /*
             * //[lenght 10]
             * Any additional information you wish to enter to identify the payment
                • Will be assigned to each payment within this batch
                • This information will sent with the payment to the receiving FI
             */
            PaymentSundryInformation = paymentSundryInformation;
            /*
             * [lenght 6] This is the date the funds are to be settled to the receiver’s account
                • Will be assigned to each payment within this batch
                • Format: 0YYDDD(Julian Date) or YYMMDD
                • 0 - numeric zero
                • YY - last two digits of the year
                • DDD - day number within the year
                Example: 010162 or 100611 = June 11, 2010
             */
            ValueDate = valueDate;
            Filler6 = string.Empty.PadRight(14);//[lenght 14] Space fill
        }

        public override string ToString()
        {
            string batchHeader = RecordType + Filler + TransactionCode + PaymentSundryInformation + ValueDate + Filler6;
            return batchHeader;
        }
    }
}
