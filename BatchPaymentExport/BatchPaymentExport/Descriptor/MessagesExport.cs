using PX.Common;

namespace ExportBatch.Descriptor
{
	[PXLocalizable]
	public static class MessagesExport
	{
		//"The originator number assigned by CIBC must be the 10 digit."
		public const string IMMEDIATE_ORIGINATOR = "The originator number assigned by CIBC must be the 10 digit.";
        //ClientDefinedACHFileNameIsRequired
		public const string CLIENT_DEFINED_ACH_FILE_NAME_IS_REQUIRED = "Client Defined ACH File Name is required. Please check AP Preferences.";
        //SystemModeIsRequired
		public const string SYSTEM_MODE_IS_REQUIRED = "System Mode is required. Please check AP Preferences.";
        //IMMEDIATE_ORIGINATOR_IS_REQUIRED
		public const string IMMEDIATE_ORIGINATOR_IS_REQUIRED = "Immediate Originator is required. Please check Company settings.";
		//EFTChannelIsRequired
		public const string EFT_CHANNEL_IS_REQUIRED = "EFT Channel is required. Please check AP Preferences.";
        //EFTChannel Export Serial Number is empty
        public const string EFT_CHANNEL_SERIAL_NUMBER_REQUIRED = "Export Serial Number Not Generate. Please check AP Preferences.";

        public const string TRANSIT_REQUIRED = "Branch / Transit not define in Cash Account- Remittance setting";
        public const string ACCOUNT_REQUIRED = "Account No not define in Cash Account- Remittance setting";

        public const string BANKINSTITUTIONID_REQUIRED = "Bank / Financial Institution not define in Vendor";
        public const string VENDOR_TRANSIT_REQUIRED = "Branch / Transit not define in Vendor";
        public const string VENDOR_ACCOUNT_REQUIRED = "Account No not define in Vendor";
    }
}
