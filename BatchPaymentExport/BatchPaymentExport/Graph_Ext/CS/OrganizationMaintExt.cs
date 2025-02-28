using ExportBatch.DAC_Ext.CS;
using ExportBatch.Descriptor;
using PX.Data;
using PX.Objects.CS;
using PX.Objects.CS.DAC;

namespace ExportBatch.Graph_Ext.CS
{
	public class OrganizationMaintExt : PXGraphExtension<OrganizationMaint>
	{
		public static bool IsActive() => true;

		#region Events
		public virtual void _(Events.RowPersisting<OrganizationBAccount> e)
		{
			if (e.Row is OrganizationBAccount)
			{
				OrganizationBAccountExt accountExt = e.Row.GetExtension<OrganizationBAccountExt>();
				if (!string.IsNullOrEmpty(accountExt?.UsrImmediateOriginator))
				{
					if (accountExt.UsrImmediateOriginator.Trim().Length != 10)
					{
						string name = PXUIFieldAttribute.GetDisplayName<OrganizationBAccountExt.usrImmediateOriginator>(e.Cache);
						e.Cache.RaiseExceptionHandling<OrganizationBAccountExt.usrImmediateOriginator>(e.Row, accountExt.UsrImmediateOriginator, new PXSetPropertyException(MessagesExport.IMMEDIATE_ORIGINATOR, PXErrorLevel.Error));
						throw new PXRowPersistingException(name, accountExt.UsrImmediateOriginator, MessagesExport.IMMEDIATE_ORIGINATOR);
					}
				}
			}
		}
		#endregion
	}
}
