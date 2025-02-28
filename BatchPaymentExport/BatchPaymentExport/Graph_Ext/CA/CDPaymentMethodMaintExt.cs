using ExportBatch.DAC_Ext.CA;
using PX.Data;
using PX.Objects.CA;

namespace ExportBatch.Graph_Ext.CA
{
	public class CDPaymentMethodMaintExt : PXGraphExtension<PaymentMethodMaint>
	{
		public static bool IsActive() => true;

		#region Events
		public virtual void _(Events.RowSelected<PaymentMethod> e)
		{
			if (e.Row is PaymentMethod)
			{
				PXUIFieldAttribute.SetVisible<CDPaymentMethodExt.usrCDExportFormatType>(e.Cache, e.Row, e.Row.APCreateBatchPayment == true);
			}
		}
		#endregion
	}
}
