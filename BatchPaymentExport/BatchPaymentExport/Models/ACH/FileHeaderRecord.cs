using System;

namespace ExportBatch.Models.ACH
{
	public class FileHeaderRecord
	{
		public string RecordTypeCode { get; set; }
		public string PriorityCode { get; set; }
		public string ImmediateDestination { get; set; }
		public string ImmediateOriginator { get; set; }
		public string TransmissionDate { get; set; }
		public string TransmissionTime { get; set; }
		public string FileIDModifier { get; set; }
		public string RecordSize { get; set; }
		public string BlockingFactor { get; set; }
		public string FormatCode { get; set; }
		public string ImmediateDestinationName { get; set; }
		public string ImmediateOriginatorName { get; set; }
		public string ReferenceCode { get; set; }

		public FileHeaderRecord(string immediateOrigin, DateTime transmissionDate, DateTime transmissionTime, string fileIDModifier, string immediateOriginatorName)
		{
			RecordTypeCode = "1";// [lenght 1] Must be ‘1’ 
			PriorityCode = "01";// [lenght 2] Must be ‘01’ 
			ImmediateDestination = "021000021".PadLeft(10);// [lenght 10] Must a blank space + ‘021000021’ 
			ImmediateOriginator = immediateOrigin;//TODO : [lenght 10] added into Companies screen
			TransmissionDate = transmissionDate.ToString("YYMMDD");// [lenght 6] [CreatedDateTime] Indicates the creation date of the file and cannot be more than 7 days in the past
			TransmissionTime = transmissionTime.ToString("HHMM");// [lenght 4] [CreatedDateTime] Indicates the creation time of the file
			FileIDModifier = fileIDModifier;// [lenght 1] [RefNbr parsed] Must be UPPERCASE A - Z or 0 - 9
			RecordSize = "094";// [lenght 3] Must be ‘094’ 
			BlockingFactor = "10";// [lenght 2] Must be ‘10’ 
			FormatCode = "1";// [lenght 1] Must be ‘1’ 
			ImmediateDestinationName = "CIBC".PadRight(23);// [lenght 23] Must be ‘CIBC’ 
			ImmediateOriginatorName = "CIBC" + immediateOriginatorName; //[lenght 23] Must be ‘CIBC’ plus COMPANY NAME Must use same name as Originator and Settlement Account
			ReferenceCode = string.Empty.PadLeft(8); //[lenght 8] BLANK Field is space filled 
		}
		public override string ToString()
		{
			string fileHeaderRecord = $"{RecordTypeCode}{PriorityCode}{ImmediateDestination}{ImmediateOriginator}{TransmissionDate}{TransmissionTime}{FileIDModifier}{RecordSize}{BlockingFactor}{FormatCode}{ImmediateDestinationName}{ImmediateOriginatorName.PadRight(23)}{ReferenceCode}";
			return fileHeaderRecord;
		}
	}
}