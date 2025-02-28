using System;

namespace ExportBatch.Models.EFT
{
    // Detail Record
    public class Detail
    {
        public string RecordType { get; set; }
        public string CreditDebitIdentifier { get; set; }
        public string Filler { get; set; }
        public string ReceiverBankInstitutionID { get; set; }
        public string ReceiverBankTransitNumber { get; set; }
        public string ReceiverAccountNumber { get; set; }
        public string Filler7 { get; set; }
        public string Amount { get; set; }
        public string CrossReferenceNumber { get; set; }
        public string ReceiverName { get; set; }
        public string Filler11 { get; set; }

        public Detail(string receiverBankInstitutionID,string creditDebitIdentifier, string receiverBankTransitNumber, string receiverAccountNumber, string amount, string crossReferenceNumber, string receiverName)
        {
            RecordType = "6";//[lenght 1] Always ‘6’ Batch header indicator
            /*
             * [lenght 1]
             * ‘C’ for direct deposit (credit)
                ‘D’ for pre-authorized payment (debit)
                • Must be upper case
             */
            CreditDebitIdentifier = creditDebitIdentifier;
            Filler = string.Empty.PadRight(1); // [lenght 1] Space fill
            /*
             *  [lenght 4]
             * Receiver Financial Institution ID, format 0iii, where:
                • 0 = constant zero
                • iii = institution id (for example, CIBC = “010”)
             */
            ReceiverBankInstitutionID = receiverBankInstitutionID.PadLeft(4, '0'); //"0010";
            ReceiverBankTransitNumber = receiverBankTransitNumber; // [lenght 5] The bank transit number of the receiver Example: 05772
            ReceiverAccountNumber = receiverAccountNumber.PadRight(12); // [lenght 12] Left justified, space fill to the right Example: 6015816
            Filler7 = string.Empty.PadRight(5); // [lenght 5] Space fill
            /*
             * [lenght 10]
             * Dollar amount of the payment transaction
                • Must be greater than zero
                • The decimal is implied
                • Format = $$$$$$$$¢¢
                Example: 0000011793 = $117.93
                Maximum dollar amount per payment is 9999999999 ($99,999,999.99)
             */



            Amount = amount; // 
            CrossReferenceNumber = crossReferenceNumber.PadLeft(13); // [lenght 13] This is a unique payment identifier for each payment • Must be unique within each file
            ReceiverName = receiverName.PadRight(22); // [lenght 22] Name of the payment receiver. Cannot be blank
            Filler11 = string.Empty.PadRight(6); // [lenght 6] Space fill
        }

        public override string ToString()
        {
            string detail = RecordType + CreditDebitIdentifier + Filler + ReceiverBankInstitutionID + ReceiverBankTransitNumber + ReceiverAccountNumber + Filler7 + Amount + CrossReferenceNumber + ReceiverName + Filler11;
            return detail;
        }
    }
}
