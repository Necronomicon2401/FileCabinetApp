using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// Class for FileCabinetService snapshots.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="list">List of records.</param>
        public FileCabinetServiceSnapshot(List<FileCabinetRecord> list)
        {
            this.records = list.ToArray();
        }

        /// <summary>
        /// Creates CSV record.
        /// </summary>
        /// <param name="streamWriter">Stream writer to use.</param>
        public void SaveToCsv(StreamWriter streamWriter)
        {
            FileCabinetRecordCsvWriter fileCabinetRecordCsvWriter = new (streamWriter);
            streamWriter.WriteLine("Id,First Name,Last Name,Date of Birth,Work Experience,Weight,Lucky symbol");
            foreach (var record in this.records)
            {
                fileCabinetRecordCsvWriter.Write(record);
            }
        }

        /// <summary>
        /// Creates XML record.
        /// </summary>
        /// <param name="streamWriter">Stream writer to use.</param>
        public void SaveToXml(StreamWriter streamWriter)
        {
            XmlWriterSettings settings = new ();
            settings.ConformanceLevel = ConformanceLevel.Auto;
            settings.Indent = true;
            settings.NewLineOnAttributes = true;
            XmlWriter writer = XmlWriter.Create(streamWriter, settings);
            FileCabinetRecordXmlWriter fileCabinetRecordXmlWriter = new (writer);
            fileCabinetRecordXmlWriter.Write(this.records);
            writer.Close();
        }
    }
}
