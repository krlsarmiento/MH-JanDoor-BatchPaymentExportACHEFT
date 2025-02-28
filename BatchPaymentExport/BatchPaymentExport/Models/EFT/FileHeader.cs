namespace ExportBatch.Models.EFT
{
    // File Header Record
    public class FileHeader
    {
        public string LogicalRecordType { get; set; }
        public string Filler { get; set; }
        public string DestinationDataCenter { get; set; }
        public string Filler4 { get; set; }
        public string OriginatorNumber { get; set; }
        public string FileCreationDate { get; set; }
        public string FileCreationNumber { get; set; }
        public string Filler8 { get; set; }
        public string InstitutionNumber { get; set; }
        public string BranchTransitNumber { get; set; }
        public string AccountNumber { get; set; }
        public string Filler12 { get; set; }
        public string OriginatorsShortName { get; set; }
        public string Filler14 { get; set; }
        public string CurrencyIndicator { get; set; }
        public string Filler16 { get; set; }

        public FileHeader(string destinationDataCenter, string originatorNumber,
            string fileCreationDate, string fileCreationNumber, string branchTransitNumber,
            string accountNumber, string originatorsShortName, string currencyIndicator)
        {
            LogicalRecordType = "1";//[lenght 1] Header record indicator
            Filler = string.Empty.PadRight(2);//[lenght 2] Space fill
            DestinationDataCenter = destinationDataCenter;//[lenght 5] The first 5 digits of your originator number Example: 01020
            Filler4 = string.Empty.PadRight(5);//[lenght 5] Space fill
            OriginatorNumber = originatorNumber;//[lenght 10] Your 10-digit originator number assigned to you by CIBC
            /*
             * [lenght 6]
             * Format: 0YYDDD (Julian date) or YYMMDD
                • 0 - numeric zero
                • YY - last two digits of the year
                • DDD - day number within the year
                • Cannot exceed 7 calendar days before the date of transmission
                Example: 010162 or 100611 = June 11, 2010
             */
            FileCreationDate = fileCreationDate;
            /*
             * [lenght 4]
             * Used to ensure that all files created by the originator are not processed twice
                • Must be unique
                • Do not use 0000 as a file creation number
                • After 9999, numbering should rollover to 0001
             */
            FileCreationNumber = fileCreationNumber;
            Filler8 = string.Empty.PadRight(1);//[lenght 1] Space fill
            InstitutionNumber = "0010";//[lenght 4] Financial institution number for the settlement account. Must be 0010 for CIBC
            /*
             * [lenght 5]
             * CIBC settlement transit number
                • Must be a 5-digit number
                • No spaces
                Example: 06572
             */
            BranchTransitNumber = branchTransitNumber;
            /*
             * [lenght 12]
             * Your CIBC settlement account number
                • Must be a 7-digit number
                • Left Justified
                • Space fill from position 51-55
                Example: 6015816
             */
            AccountNumber = accountNumber.PadRight(12);
            Filler12 = string.Empty.PadRight(2);//[lenght 2] Space fill
            OriginatorsShortName = originatorsShortName;//[lenght 15] Originator name shortened to 15 characters
            Filler14 = string.Empty.PadRight(1);//[lenght 1] Space fill
            CurrencyIndicator = currencyIndicator;//[lenght 3] CAD for Canadian Dollars, USD for U.S. Dollars
            Filler16 = string.Empty.PadRight(4);//[lenght 4] Space fill
        }

        public override string ToString()
        {
            string header = LogicalRecordType + Filler + DestinationDataCenter + Filler4 + OriginatorNumber + FileCreationDate + FileCreationNumber + Filler8 + InstitutionNumber + BranchTransitNumber + AccountNumber + Filler12 + OriginatorsShortName + Filler14 + CurrencyIndicator + Filler16;
            return header;
        }
    }
}
