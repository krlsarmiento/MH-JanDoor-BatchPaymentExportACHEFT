using ExportBatch.Models.ACH;
using ExportBatch.Models.ACH.Addenda;
using System;
using System.IO;
using System.Text;

namespace ExportBatch
{

	public class ACHExport
	{
		public virtual string GenerateFileName(string clientDefined, string sysMode)
		{
			string fileName = $"{clientDefined}.I0094.{sysMode}" + ".dat";
			return fileName;
		}
		public virtual string PrepareACHFileContent(string directory, string fileName, string fileHeaderRecord, string batchHeaderRecord, string detailsWithAddenda,string batchControl, string fileTrailerControl)
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
					writer.WriteLine(fileHeaderRecord);
					writer.WriteLine(batchHeaderRecord);
					writer.WriteLine(detailsWithAddenda);
					writer.WriteLine(batchControl);
					if (!string.IsNullOrEmpty(fileTrailerControl))
					{
						writer.WriteLine(fileTrailerControl);
					}
				}
				stream.Dispose();
			}
			return filePath;
		}
		public virtual string CreateFileHeader(string immediateOrigin, DateTime transmissionDate, string fileIDModifier, string immediateOriginatorName)
		{
			FileHeaderRecord fileHeaderRecord = new FileHeaderRecord(immediateOrigin, transmissionDate, transmissionDate, fileIDModifier, immediateOriginatorName);
			return fileHeaderRecord.ToString();
		}
		public virtual string CreateBatchHeader(string originatorIdentification, string companyEntryDescription, DateTime effectiveEntryDate, string batchNumber)
		{
			BatchHeaderRecord batchHeaderRecord = new BatchHeaderRecord(originatorIdentification, companyEntryDescription, effectiveEntryDate, batchNumber);
			return batchHeaderRecord.ToString();
		}
		public virtual string CreateEntryDetail(string transactionCode, string receivingDFIIdentificationAndCheckDigit, string amount, string receiversAccountNumber, string traceNumber)
		{
			EntryDetailRecord entryDetail = new EntryDetailRecord(transactionCode, receivingDFIIdentificationAndCheckDigit, amount, receiversAccountNumber, traceNumber);
			return entryDetail.ToString();
		}
		public virtual string CreateFirstAddenda(string foreignPaymentAmount, string receiverName, string entryDetailSequenceNumber)
		{
			FirstAddendaRecord firstAddenda = new FirstAddendaRecord(foreignPaymentAmount, receiverName, entryDetailSequenceNumber);
			return firstAddenda.ToString();
		}
		public virtual string CreateSecondAddenda(string originatorName, string originatorStreetAddress, string entryDetailSequenceNumber)
		{
			SecondAddendaRecord secondAddenda = new SecondAddendaRecord(originatorName, originatorStreetAddress, entryDetailSequenceNumber);
			return secondAddenda.ToString();
		}
		public virtual string CreateThirdAddenda(string originatorCityStateProvince, string originatorCountryPostalCode, string entryDetailSequenceNumber)
		{
			ThirdAddendaRecord thirdAddenda = new ThirdAddendaRecord(originatorCityStateProvince, originatorCountryPostalCode, entryDetailSequenceNumber);
			return thirdAddenda.ToString();
		}
		public virtual string CreateFourthAddenda(string entryDetailSequenceNumber)
		{
			FourthAddendaRecord fourthAddenda = new FourthAddendaRecord(entryDetailSequenceNumber);
			return fourthAddenda.ToString();
		}
		public virtual string CreateFifthAddenda(string receivingDfiName, string receivingDfiIdentification, string entryDetailSequenceNumber)
		{
			FifthAddendaRecord fifthAddenda = new FifthAddendaRecord(receivingDfiName, receivingDfiIdentification, entryDetailSequenceNumber);
			return fifthAddenda.ToString();
		}
		public virtual string CreateSixthAddenda(string receiverIdentificationNumber, string receiverStreetAddress, string entryDetailSequenceNumber)
		{
			SixthAddendaRecord sixthAddenda = new SixthAddendaRecord(receiverIdentificationNumber, receiverStreetAddress, entryDetailSequenceNumber);
			return sixthAddenda.ToString();
		}
		public virtual string CreateSeventhAddenda(string receiverCityStateProvince, string receiverCountryPostalCode, string entryDetailSequenceNumber)
		{
			SeventhAddendaRecord seventhAddenda = new SeventhAddendaRecord(receiverCityStateProvince, receiverCountryPostalCode, entryDetailSequenceNumber);
			return seventhAddenda.ToString();
		}
		public virtual string CreateRemittanceAddenda(string entryDetailSequenceNumber)
		{
			RemittanceAddendaRecord semittanceAddenda = new RemittanceAddendaRecord(entryDetailSequenceNumber);
			return semittanceAddenda.ToString();
		}
		public virtual string CreateBatchControl(string entryAddendaCount, string entryHash, string totalDebitEntryDollarAmount, string totalCreditEntryDollarAmount, string companyIdentification, string batchHeader)
		{
			BatchControlRecord batchControl = new BatchControlRecord(entryAddendaCount, entryHash, totalDebitEntryDollarAmount, totalCreditEntryDollarAmount, companyIdentification, batchHeader);
			return batchControl.ToString();
		}
		public virtual string CreateFileTrailerControl(string batchCount, string blockCount, string entryAddendaCount, string entryHash, string totalDebitEntryDollarAmountInFile, string totalCreditEntryDollarAmountInFile)
		{
			FileTrailerControlRecord fileTrailerControl = new FileTrailerControlRecord(batchCount, blockCount, entryAddendaCount, entryHash, totalDebitEntryDollarAmountInFile, totalCreditEntryDollarAmountInFile);
			return fileTrailerControl.ToString();
		}
	}
}