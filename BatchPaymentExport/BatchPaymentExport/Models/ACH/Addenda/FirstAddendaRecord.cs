namespace ExportBatch.Models.ACH.Addenda
{
	public class FirstAddendaRecord : AddendaRecordBase
	{
		//TRANSACTION TYPE CODE
		public string TransactionTypeCode { get; set; }
		//FOREIGN PAYMENT AMOUNT
		public string ForeignPaymentAmount { get; set; }
		//FOREIGN TRACE NUMBER
		public string ForeignTraceNumber { get; set; }
		//RECEIVER NAME
		public string ReceiverName { get; set; }

		/*
		* [lenght 1] RECORD TYPE CODE  Must use ‘7’ 
		* [lenght 2] ADDENDA TYPE CODE  Must use ‘10’ 
		* [lenght 6] RESERVED  Blank Field is space filled 
		* [lenght 7] ENTRY DETAIL SEQUENCE NUMBER Same as last seven digits of Trace Number from Detail Record, Field 13 
		*/
		public FirstAddendaRecord(string foreignPaymentAmount,string receiverName, string entryDetailSequenceNumber)
			: base("7", "10", string.Empty.PadRight(6), entryDetailSequenceNumber)
		{
			/*
			 * [lenght 3] 
			 * Field must contain one of the following values: 
			 * ANN – Annuity 
			 * BUS – Business/Commercial  
			 * DEP – Deposit 
			 * LOA – Loan 
			 * MIS – Miscellaneous  
			 * MOR – Mortgage  
			 * PEN – Pension 
			 * RLS – Rent/Lease  
			 * SAL – Salary/Payroll  
			 * TAX – Tax
			 */
			TransactionTypeCode = "BUS";//based on customer requirements
			ForeignPaymentAmount = foreignPaymentAmount;//[lenght 18] Same value as Detail Record, Field 7 Must be right justified and zero filled
			ForeignTraceNumber = string.Empty.PadRight(22);//[lenght 22] Field is space filled
			ReceiverName = receiverName;//[lenght 35] Identifies the Name of the Receiver of the payment instruction. Must be left justified and space filled
		}

		public override string ToString()
		{
			string result = RecordTypeCode + AddendaTypeCode + TransactionTypeCode 
							+ ForeignPaymentAmount.PadRight(18,'0') + ForeignTraceNumber 
							+ ReceiverName.PadRight(35) + Reserved + EntryDetailSequenceNumber;
			return result;
		}
	}
}
