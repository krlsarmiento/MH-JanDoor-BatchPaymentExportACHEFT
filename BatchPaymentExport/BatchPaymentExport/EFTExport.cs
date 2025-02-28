using ExportBatch.Descriptor;
using ExportBatch.Models.EFT;
using System.IO;
using System.Text;

namespace ExportBatch
{
	public class EFTExport
	{
		public virtual string GenerateFileName(string clientDefined, string sysMode, string channel)
		{
			string fileType = channel == EFTChannelTypeAttribute.FTSChannel ? "C0080" : "0080";
			string fileName = $"{clientDefined}.{fileType}.{sysMode}" + ".dat";
			return fileName;
		}
		public virtual string PrepareACHFileContent(string directory, string fileName, string fileHeader, string batchHeader, string details, string batchTrailer, string fileTrailer)
		{
			if (!Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
			// Specify the full path for the .dat file
			string filePath = Path.Combine(directory, fileName);
			using (FileStream stream = new FileStream(filePath, FileMode.OpenOrCreate, FileAccess.ReadWrite))
			{
				// Move the stream position to the end of the file to continue writing
				stream.Seek(0, SeekOrigin.End);
				using (StreamWriter writer = new StreamWriter(stream, Encoding.ASCII))
				{
					writer.WriteLine(fileHeader);
					writer.WriteLine(batchHeader);
					writer.WriteLine(details);
					writer.WriteLine(batchTrailer);
					if (!string.IsNullOrEmpty(fileTrailer))
					{
						writer.WriteLine(fileTrailer);
					}
				}
				stream.Dispose();
			}
			return filePath;
		}
		public virtual string CreateFileHeader(string destinationDataCenter, string originatorNumber, string fileCreationDate, string fileCreationNumber, string branchTransitNumber, string accountNumber, string originatorsShortName, string currencyIndicator)
		{
			FileHeader fileHeader = new FileHeader(destinationDataCenter, originatorNumber, fileCreationDate, fileCreationNumber, branchTransitNumber, accountNumber, originatorsShortName, currencyIndicator);
			return fileHeader.ToString();
		}
		public virtual string CreateBatchHeader(string transactionCode, string paymentSundryInformation, string valueDate)
		{
			BatchHeader batchHeader = new BatchHeader(transactionCode, paymentSundryInformation, valueDate);
			return batchHeader.ToString();
		}
		public virtual string CreateDetail(string receiverBankInstitutionID,string creditDebitIdentifier, string receiverBankTransitNumber, string receiverAccountNumber, string amount, string crossReferenceNumber, string receiverName)
		{
			Detail detail = new Detail(receiverBankInstitutionID,creditDebitIdentifier, receiverBankTransitNumber, receiverAccountNumber, amount, crossReferenceNumber, receiverName);
			return detail.ToString();
		}
		public virtual string CreateBatchTrailer(string transactionCode, string batchEntryCount, string entryDollarAmount)
		{
			BatchTrailer batchTrailer = new BatchTrailer(transactionCode, batchEntryCount, entryDollarAmount);
			return batchTrailer.ToString();
		}
		public virtual string CreateFileTrailer(string batchCount, string detailCount)
		{
			FileTrailer fileTrailer = new FileTrailer(batchCount, detailCount);
			return fileTrailer.ToString();
		}
	}
}
