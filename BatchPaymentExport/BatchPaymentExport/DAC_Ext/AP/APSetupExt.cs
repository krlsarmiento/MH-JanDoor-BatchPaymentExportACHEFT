using ExportBatch.Descriptor;
using PX.Data;
using PX.Data.BQL;
using PX.Objects.AP;
using PX.Objects.CS;

namespace ExportBatch.DAC_Ext.AP
{
	public sealed class APSetupExt : PXCacheExtension<APSetup>
	{
		public static bool IsActive() => true;

		//create string field called ClientDefinedACHFileName
		#region UsrClientDefinedACHFileName
		[PXDBString(30,IsUnicode =true)]
		[PXUIField(DisplayName = "Client Defined File Name")]
		public string UsrClientDefinedACHFileName { get; set; }
		public abstract class usrClientDefinedACHFileName : BqlType<IBqlString,string>.Field<usrClientDefinedACHFileName> { }
		#endregion

		//create string field called SystemMode
		#region UsrSystemMode
		[PXDBString(1,IsFixed =true)]
		[PXUIField(DisplayName = "System Mode")]
		[SystemModeTypeAttribute]
		public string UsrSystemMode { get; set; }
		public abstract class usrSystemMode : BqlType<IBqlString,string>.Field<usrSystemMode> { }
		#endregion

		//create string field called EFtChannel
		#region UsrEFtChannel
		[PXDBString(3, IsFixed = true)]
		[PXUIField(DisplayName = "EFT Channel")]
		[EFTChannelTypeAttribute]
		public string UsrEFtChannel { get; set; }
		public abstract class usrEFtChannel : BqlType<IBqlString, string>.Field<usrEFtChannel> { }
        #endregion

        #region UsrExportSerialNbr
        [PXDBString(25)]
        [PXUIField(DisplayName = "Export Serial Nbr. Sequence")]
		[PXSelector(typeof(Numbering.numberingID),DescriptionField = typeof(Numbering.descr))]
        public string UsrExportSerialNbr { get; set; }
        public abstract class usrExportSerialNbr : PX.Data.BQL.BqlString.Field<usrExportSerialNbr> { }
        #endregion
    }
}
