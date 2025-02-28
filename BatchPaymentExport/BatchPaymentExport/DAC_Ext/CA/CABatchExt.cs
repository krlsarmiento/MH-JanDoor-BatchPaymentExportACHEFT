using PX.Data.ReferentialIntegrity.Attributes;
using PX.Data;
using PX.Objects.CA;
using PX.Objects.CM;
using PX.Objects.CR;
using PX.Objects.CS;
using PX.Objects.GL;
using PX.Objects;
using PX.TM;
using System.Collections.Generic;
using System;
using PX.Objects.AP;
using ExportBatch.DAC_Ext.AP;

namespace PX.Objects.CA
{
    public class CABatchExt : PXCacheExtension<PX.Objects.CA.CABatch>
    {
        #region UsrExportSerialNbr
        [PXDBString(25)]
        [PXUIField(DisplayName = "Export Serial Nbr")]
        [AutoNumber(typeof(APSetupExt.usrExportSerialNbr), typeof(AccessInfo.businessDate))]
        public virtual string UsrExportSerialNbr { get; set; }
        public abstract class usrExportSerialNbr : PX.Data.BQL.BqlString.Field<usrExportSerialNbr> { }
        #endregion
    }
}