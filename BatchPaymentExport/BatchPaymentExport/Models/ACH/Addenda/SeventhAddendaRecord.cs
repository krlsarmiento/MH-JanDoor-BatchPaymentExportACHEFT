namespace ExportBatch.Models.ACH.Addenda
{
	//Seventh mandatory addenda record(7)
	public class SeventhAddendaRecord : AddendaRecordBase
	{
		//RECEIVER CITY & STATE/PROVINCE
		public string ReceiverCityStateProvince { get; set; }
		//RECEIVER COUNTRY & POSTAL CODE
		public string ReceiverCountryPostalCode { get; set; }

		/*
			* [lenght 1] RECORD TYPE CODE  Must use ‘7’ 
			* [lenght 2] ADDENDA TYPE CODE  Must use ‘16’ 
			* [lenght 14] RESERVED  Blank Field is space filled 
			* [lenght 7] ENTRY DETAIL SEQUENCE NUMBER Same as last seven digits of Trace Number from Detail Record, Field 13 
		*/
		public SeventhAddendaRecord(string receiverCityStateProvince, string receiverCountryPostalCode, string entryDetailSequenceNumber) : base("7", "16",string.Empty.PadRight(14), entryDetailSequenceNumber)
		{
			/*
			 * [lenght 35]
			 * Contains the city and the ISO state or province code of the receiver. 
			 * An asterisk (‘*’) will be the delimiter between the data elements, and the backslash (‘\’) will be the terminator following the last data element 
			 * Left justified and space filled  
			 * Example: TORONTO*ON\ TAMPA*FL\
			 */
			ReceiverCityStateProvince = receiverCityStateProvince.PadRight(35);
			/*
			 * [lenght 35]
			 * Contains the  2 character standard ISO country code and postal/zip code of the receiver. 
			 * An asterisk (‘*’) will be the delimiter between the data elements, and the backslash (‘\’) will be the terminator following the last data element 
			 * Left justified and space filled Acceptable Postal / Zip Code formats: A1A1A1 99999 999999999 
			 * Example: CA*A1A1A1\ US*33610\ US*335252212\ 
			 * See Appendix for ISO country codes 
			 */
			ReceiverCountryPostalCode = receiverCountryPostalCode.PadRight(35);
		}

		public override string ToString()
		{
			string result = RecordTypeCode + AddendaTypeCode + ReceiverCityStateProvince + ReceiverCountryPostalCode + Reserved + EntryDetailSequenceNumber;
			return result;
		}
	}
}
