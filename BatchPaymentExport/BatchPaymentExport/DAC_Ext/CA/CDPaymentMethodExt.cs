using ExportBatch.Descriptor;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.CA;

namespace ExportBatch.DAC_Ext.CA
{
	public sealed class CDPaymentMethodExt : PXCacheExtension<PaymentMethod>
	{
		public static bool IsActive() => true;

        #region UsrCDExportFormatType
        [PXDBString(3,IsFixed =true)]
        [PXUIField(DisplayName = "Export Format Type")]
        [CDExportFormatTypeListAttribute]
        public string UsrCDExportFormatType { get; set; }
        public abstract class usrCDExportFormatType : BqlType<IBqlString,string>.Field<usrCDExportFormatType> { }
		#endregion
	}
}
