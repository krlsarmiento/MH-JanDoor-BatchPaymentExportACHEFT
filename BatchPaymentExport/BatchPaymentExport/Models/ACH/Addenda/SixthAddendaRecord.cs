namespace ExportBatch.Models.ACH.Addenda
{
	//Sixth mandatory addenda record (7) 
	public class SixthAddendaRecord : AddendaRecordBase
	{
		//RECEIVER IDENTIFICATION NUMBER
		public string ReceiverIdentificationNumber { get; set; }
		//RECEIVER STREET ADDRESS
		public string ReceiverStreetAddress { get; set; }

		/*
		* [lenght 1] RECORD TYPE CODE  Must use ‘7’ 
		* [lenght 2] ADDENDA TYPE CODE  Must use ‘15’ 
		* [lenght 34] RESERVED  Blank Field is space filled 
		* [lenght 7] ENTRY DETAIL SEQUENCE NUMBER Same as last seven digits of Trace Number from Detail Record, Field 13 
		*/
		public SixthAddendaRecord(string receiverIdentificationNumber, string receiverStreetAddress, string entryDetailSequenceNumber) : base("7", "15", string.Empty.PadRight(34), entryDetailSequenceNumber)
		{
			ReceiverIdentificationNumber = receiverIdentificationNumber; //[lenght 15] optional May be used by the originator to insert its own number for tracing purposes Ignored if present
			ReceiverStreetAddress = receiverStreetAddress;//[lenght 35] Contains the physical street address of the receiver.P.O.Box address may result in delayed or rejected payment. Left justified and space filled
		}
		public override string ToString()
		{
			string result = RecordTypeCode + AddendaTypeCode + ReceiverIdentificationNumber + ReceiverStreetAddress.PadRight(35) + Reserved + EntryDetailSequenceNumber;
			return result;
		}
	}
}
