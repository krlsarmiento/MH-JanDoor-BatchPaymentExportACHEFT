using PX.Data;
using PX.Data.BQL;

namespace ExportBatch.Descriptor
{
	public class EFTChannelTypeAttribute : PXStringListAttribute
	{
		public const string FTSChannel = "FTS";//C0080
		public const string SCAChannel = "SCA";//0080

		public const string FTSChannelDN = "FTS Channel";
		public const string SCAChannelDN = "SCA Channel";

		public EFTChannelTypeAttribute() : base(new string[] { FTSChannel,SCAChannel}, new string[] { FTSChannelDN,SCAChannelDN })
		{ }

		public class ftsChannel : BqlType<IBqlString,string>.Constant<ftsChannel>
		{
			public ftsChannel() : base(FTSChannel) { }
		}
		public class scaChannel : BqlType<IBqlString,string>.Constant<scaChannel>
		{
			public scaChannel() : base(SCAChannel) { }
		}
	}
}
