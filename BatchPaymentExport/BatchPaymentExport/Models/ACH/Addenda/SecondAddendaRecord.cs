namespace ExportBatch.Models.ACH.Addenda
{
	public class SecondAddendaRecord : AddendaRecordBase
	{
		//ORIGINATOR NAME
		public string OriginatorName { get; set; }
		//ORIGINATOR STREET ADDRESS
		public string OriginatorStreetAddress { get; set; }

		/*
		* [lenght 1] RECORD TYPE CODE  Must use ‘7’ 
		* [lenght 2] ADDENDA TYPE CODE  Must use ‘11’ 
		* [lenght 14] RESERVED  Blank Field is space filled 
		* [lenght 7] ENTRY DETAIL SEQUENCE NUMBER Same as last seven digits of Trace Number from Detail Record, Field 13 
		*/
		public SecondAddendaRecord(string originatorName, string originatorStreetAddress, string entryDetailSequenceNumber) 
			: base("7", "11", string.Empty.PadRight(14), entryDetailSequenceNumber)
		{
			/*
			 * [lenght 35] Contains the name of the originator of the transaction 
			 * Must use the same name as the ACH Originator and settlement account.
			 * Left justified and space filled
			 */
			OriginatorName = originatorName;
			/*
			 * [lenght 35] Contains the physical street address of the originator of the transaction - same as for the ACH Originator and settlement account
			 * Cannot use P.O.Box address
			 * Left justified and space filled
			 */
			OriginatorStreetAddress = originatorStreetAddress;
		}

		public override string ToString()
		{
			string result = RecordTypeCode + AddendaTypeCode + OriginatorName.PadRight(35) + OriginatorStreetAddress.PadRight(35) + Reserved + EntryDetailSequenceNumber;
			return result;
		}
	}
}
