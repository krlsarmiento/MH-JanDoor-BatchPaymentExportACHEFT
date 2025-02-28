namespace ExportBatch.Models.ACH.Addenda
{
	public class AddendaRecordBase
	{
		//RECORD TYPE CODE
		public string RecordTypeCode { get; set; }
		//ADDENDA TYPE CODE
		public string AddendaTypeCode { get; set; }
		//RESERVED
		public string Reserved { get; set; }
		//ENTRY DETAIL SEQUENCE NUMBER
		public string EntryDetailSequenceNumber { get; set; }
		public AddendaRecordBase(string recordTypeCode, string addendaTypeCode, string reserved, string entryDetailSequenceNumber)
		{
			RecordTypeCode = recordTypeCode;
			AddendaTypeCode = addendaTypeCode;
			Reserved = reserved;
			EntryDetailSequenceNumber = entryDetailSequenceNumber;
		}
	}
}
