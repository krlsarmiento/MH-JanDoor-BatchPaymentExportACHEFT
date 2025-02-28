using PX.Data;
using PX.Data.BQL;

namespace ExportBatch.Descriptor
{
	public class CDExportFormatTypeListAttribute : PXStringListAttribute
	{
		public const string None = "NNA";
		public const string Ach = "ACH";
		public const string Eft = "EFT";

		public const string AchDN = "NACHA - 94";
		public const string EftDN = "EFT - 80";
		public const string NoneDN = "N/A";
		public CDExportFormatTypeListAttribute() : base(new string[] { None, Ach, Eft }, new string[] { NoneDN, AchDN, EftDN }) { }

		public class ach : BqlType<IBqlString, string>.Constant<ach>
		{
			public ach() : base(Ach) { }
		}
		public class eft : BqlType<IBqlString, string>.Constant<eft>
		{
			public eft() : base(Eft) { }
		}
		public class none : BqlType<IBqlString, string>.Constant<none>
		{
			public none() : base(None) { }
		}
	}
}
