using PX.Data;
using PX.Data.BQL;
using PX.Objects.CR;

namespace ExportBatch.DAC_Ext.CR
{
	public sealed class BAccountExt : PXCacheExtension<BAccount>
	{
		public static bool IsActive() => true;

		//create string field called IMMEDIATE ORIGINATOR
		//Must be the 10 digit originator number assigned by CIBC.
		#region UsrImmediateOriginator
		[PXDBString(10, IsUnicode = true, InputMask = "0000000000")]
		[PXUIField(DisplayName = "Immediate Originator")]
		public string UsrImmediateOriginator { get; set; }
		public abstract class usrImmediateOriginator : BqlType<IBqlString, string>.Field<usrImmediateOriginator> { }
		#endregion
	}
}