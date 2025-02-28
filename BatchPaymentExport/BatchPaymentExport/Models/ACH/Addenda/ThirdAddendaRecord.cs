namespace ExportBatch.Models.ACH.Addenda
{
	public class ThirdAddendaRecord : AddendaRecordBase
	{
		//ORIGINATOR CITY & STATE / PROVINCE
		public string OriginatorCityStateProvince { get; set; }
		//ORIGINATOR COUNTRY & POSTAL CODE
		public string OriginatorCountryPostalCode { get; set; }

		/*
		* [lenght 1] RECORD TYPE CODE  Must use ‘7’ 
		* [lenght 2] ADDENDA TYPE CODE  Must use ‘12’ 
		* [lenght 14] RESERVED  Blank Field is space filled 
		* [lenght 7] ENTRY DETAIL SEQUENCE NUMBER Same as last seven digits of Trace Number from Detail Record, Field 13 
		*/
		public ThirdAddendaRecord(string originatorCityStateProvince, string originatorCountryPostalCode, string entryDetailSequenceNumber) 
			: base("7", "12", string.Empty.PadRight(14), entryDetailSequenceNumber)
		{
			/*
			 * [lenght 35] 
			 * Contains the city and the ISO state or province code of the originator
			 * An asterisk(‘*’) will be the delimiter between the data elements, and the backslash(‘\’) will be the terminator following the last data element
			 * Left justified and space filled
			 * Example:TORONTO* ON\ TAMPA* FL\
			 */
			OriginatorCityStateProvince = originatorCityStateProvince;
			/*
			 * [lenght 35]
			 * Contains the 2 character standard ISO country code and postal/zip code of the originator 
			 * An asterisk (‘*’) will be the delimiter between the data elements, and the backslash (‘\’) will be the terminator following the last data element 
			 * Left justified and space filled
			 * Acceptable Postal / Zip Code formats: A1A1A1 55555 999999999 
			 * Example: CA*A1A1A1\  US*33610\ US*335252212\ 
			 * See Appendix for ISO country codes 
			 */
			OriginatorCountryPostalCode = originatorCountryPostalCode;
		}

		public override string ToString()
		{
			string result = RecordTypeCode + AddendaTypeCode + OriginatorCityStateProvince.PadRight(35) + OriginatorCountryPostalCode.PadRight(35) + Reserved + EntryDetailSequenceNumber;
			return result;
		}
	}
}
