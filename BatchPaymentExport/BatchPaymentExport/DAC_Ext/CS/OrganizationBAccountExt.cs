using ExportBatch.DAC_Ext.CR;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.CS.DAC;

namespace ExportBatch.DAC_Ext.CS
{
	public sealed class OrganizationBAccountExt : PXCacheExtension<OrganizationBAccount>
	{
		public static bool IsActive() => true;
		#region UsrImmediateOriginator
		[PXDBString(10, IsUnicode = true, InputMask = "0000000000",BqlField =typeof(BAccountExt.usrImmediateOriginator))]
		[PXUIField(DisplayName = "Immediate Originator")]
		public string UsrImmediateOriginator { get; set; }
		public abstract class usrImmediateOriginator : BqlType<IBqlString, string>.Field<usrImmediateOriginator> { }
		#endregion
	}
}
