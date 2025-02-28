namespace ExportBatch.Models.ACH.Addenda
{
	//Remittance addenda record (7) (maximum of two optional addenda records per payment) 
	public class RemittanceAddendaRecord : AddendaRecordBase
	{
		//PAYMENT RELATED INFORMATION
		public string PaymentRelatedInformation { get; set; }
		//ADDENDA SEQUENCE NUMBER
		public string AddendaSequenceNumber { get; set; }

		/*
		 * [lenght 1] RECORD TYPE CODE  Must use ‘7’ 
		 * [lenght 2] ADDENDA TYPE CODE  Must use ‘17’ 
		 * [lenght 7] ENTRY DETAIL SEQUENCE NUMBER Same as last seven digits of Trace Number from Detail Record, Field 13 
		 */
		public RemittanceAddendaRecord(string entryDetailSequenceNumber) : base("7", "17", "", entryDetailSequenceNumber)
		{
			PaymentRelatedInformation = string.Empty.PadRight(80);//(80 characters) optional Contains remittance information for the transaction
			AddendaSequenceNumber = "1".PadLeft(4,'0');//(4 characters) Number assigned in order to each Addenda Record following an Entry Detail Record. The first addenda sequence number must always be 1 and the second will be 2.
		}
		public override string ToString()
		{
			string result = RecordTypeCode + AddendaTypeCode + PaymentRelatedInformation + AddendaSequenceNumber + EntryDetailSequenceNumber;
			return result;
		}
	}
}