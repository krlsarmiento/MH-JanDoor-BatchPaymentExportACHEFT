using PX.Data;
using PX.Data.BQL;
using System;

namespace ExportBatch.DAC
{
	[PXHidden]
	[Serializable]
	public class FileExportNumber : IBqlTable
	{
		#region ExportType
		[PXDBString(3, IsFixed = true, IsKey = true)]
		[PXUIField(DisplayName = "Export Type")]
		[PXDefault("EFT")]
		[PXStringList(new string[] { "ACH", "EFT" }, new string[] { "ACH", "EFT" })]
		public string ExportType { get; set; }
		public abstract class exportType : BqlType<IBqlString, string>.Field<exportType> { }
		#endregion

		#region ExportNumber
		[PXDBInt]
		[PXUIField(DisplayName = "Export Number")]
		[PXDefault(1)]
		public int? ExportNumber { get; set; }
		public abstract class exportNumber : BqlType<IBqlInt, int>.Field<exportNumber> { }
		#endregion

		#region NoteID
		[PXNote]
		public virtual Guid? NoteID { get; set; }
		public abstract class noteID : BqlType<IBqlGuid, Guid>.Field<noteID> { }
		#endregion

		#region Tstamp
		[PXDBTimestamp]
		public virtual byte[] Tstamp { get; set; }
		public abstract class tstamp : BqlType<IBqlByteArray, byte[]>.Field<tstamp> { }
		#endregion

		#region CreatedByID
		[PXDBCreatedByID]
		public virtual Guid? CreatedByID { get; set; }
		public abstract class createdByID : BqlType<IBqlGuid, Guid>.Field<createdByID> { }
		#endregion

		#region CreatedByScreenID
		[PXDBCreatedByScreenID]
		public virtual string CreatedByScreenID { get; set; }
		public abstract class createdByScreenID : BqlType<IBqlString, string>.Field<createdByScreenID> { }
		#endregion

		#region CreatedDateTime
		[PXDBCreatedDateTime]
		public virtual DateTime? CreatedDateTime { get; set; }
		public abstract class createdDateTime : BqlType<IBqlDateTime, DateTime>.Field<createdDateTime> { }
		#endregion

		#region LastModifiedByID
		[PXDBLastModifiedByID]
		public virtual Guid? LastModifiedByID { get; set; }
		public abstract class lastModifiedByID : BqlType<IBqlGuid, Guid>.Field<lastModifiedByID> { }
		#endregion

		#region LastModifiedByScreenID
		[PXDBLastModifiedByScreenID]
		public virtual string LastModifiedByScreenID { get; set; }
		public abstract class lastModifiedByScreenID : BqlType<IBqlString, string>.Field<lastModifiedByScreenID> { }
		#endregion

		#region LastModifiedDateTime
		[PXDBLastModifiedDateTime]
		public virtual DateTime? LastModifiedDateTime { get; set; }
		public abstract class lastModifiedDateTime : BqlType<IBqlDateTime, DateTime>.Field<lastModifiedDateTime> { }
		#endregion
	}
}