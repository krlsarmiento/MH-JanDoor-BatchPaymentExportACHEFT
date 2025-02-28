using PX.Data;
using PX.Data.BQL;

namespace ExportBatch.Descriptor
{
	public class SystemModeTypeAttribute : PXStringListAttribute
	{
		public const string Production = "P";
		public const string Test = "T";

		public const string ProductionDN = "Prodcution";
		public const string TestDN = "Test";

		public SystemModeTypeAttribute()
			: base(new string[] { Production, Test }, new string[] { ProductionDN, TestDN })
		{
		}

		public class production : BqlType<IBqlString,string>.Constant<production>
		{
			public production() : base(Production) { }
		}

		public class test : BqlType<IBqlString,string>.Constant<test>
		{
			public test() : base(Test) { }
		}
	}
}
