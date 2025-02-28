namespace ExportBatch.Models.ACH.Addenda
{
	public class FourthAddendaRecord : AddendaRecordBase
	{
		//ORIGINATING DFI NAME
		public string OriginatingDfiName { get; set; }
		//ORIGINATING DFI IDENTIFICATION NUMBER QUALIFIER
		public string OriginatingDfiIdentificationNumberQualifier { get; set; }
		//ORIGINATING DFI IDENTIFICATION
		public string OriginatingDfiIdentification { get; set; }
		//ORIGINATING DFI BRANCH COUNTRY CODE
		public string OriginatingDfiBranchCountryCode { get; set; }

		/*
		* [lenght 1] RECORD TYPE CODE  Must use ‘7’ 
		* [lenght 2] ADDENDA TYPE CODE  Must use ‘13’ 
		* [lenght 10] RESERVED  Blank Field is space filled 
		* [lenght 7] ENTRY DETAIL SEQUENCE NUMBER Same as last seven digits of Trace Number from Detail Record, Field 13 
		*/
		public FourthAddendaRecord(string entryDetailSequenceNumber) : base("7", "13", string.Empty.PadRight(10), entryDetailSequenceNumber)
		{
			OriginatingDfiName = "CIBC".PadRight(35);//[lenght 35] Must use ‘CIBC’
			OriginatingDfiIdentificationNumberQualifier = "01";//[lenght 2] Must use ‘01’
			OriginatingDfiIdentification = "0010".PadRight(34);//[lenght 34] Must use the CIBC Identifier number ‘0010’ Left justified and space filled
			OriginatingDfiBranchCountryCode = "CA ";//[lenght 3] Must use ‘CA’ 
		}
		public override string ToString()
		{
			string record = $"{RecordTypeCode}{AddendaTypeCode}{OriginatingDfiName}{OriginatingDfiIdentificationNumberQualifier}{OriginatingDfiIdentification}{OriginatingDfiBranchCountryCode}{Reserved}{EntryDetailSequenceNumber}";
			return record;
		}
	}
}
