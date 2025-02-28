namespace ExportBatch.Models.ACH.Addenda
{
	public class FifthAddendaRecord : AddendaRecordBase
	{
		//RECEIVING DFI NAME
		public string ReceivingDfiName { get; set; }
		//RECEIVING DFI IDENTIFICATION NUMBER QUALIFIER
		public string ReceivingDfiIdentificationNumberQualifier { get; set; }
		//RECEIVING DFI IDENTIFICATION
		public string ReceivingDfiIdentification { get; set; }
		//RECEIVING DFI BRANCH COUNTRY CODE
		public string ReceivingDfiBranchCountryCode { get; set; }

		/*
		* [lenght 1] RECORD TYPE CODE  Must use ‘7’ 
		* [lenght 2] ADDENDA TYPE CODE  Must use ‘14’ 
		* [lenght 10] RESERVED  Blank Field is space filled 
		* [lenght 7] ENTRY DETAIL SEQUENCE NUMBER Same as last seven digits of Trace Number from Detail Record, Field 13 
		*/
		public FifthAddendaRecord(string receivingDfiName, string receivingDfiIdentification, string entryDetailSequenceNumber) : base("7", "14", string.Empty.PadRight(10), entryDetailSequenceNumber)
		{
			ReceivingDfiName = receivingDfiName;//[lenght 35] Contains the name of the receiving depository financial institution Left justified and space filled
			ReceivingDfiIdentificationNumberQualifier = "01";//[lenght 2] Must use ‘01’ 
			ReceivingDfiIdentification = receivingDfiIdentification;//[lenght 34] Contains a valid 9 digit ABA bank routing number and is used to identify the DFI in which the receiver maintains an account. Left justified and space filled
			ReceivingDfiBranchCountryCode = "US ";//[lenght 3] Must use ‘US’ 
		}

		public override string ToString()
		{
			string result = RecordTypeCode + AddendaTypeCode + ReceivingDfiName.PadRight(35) + ReceivingDfiIdentificationNumberQualifier 
							+ ReceivingDfiIdentification.PadRight(34) + ReceivingDfiBranchCountryCode + Reserved + EntryDetailSequenceNumber;
			return result;
		}
	}
}
