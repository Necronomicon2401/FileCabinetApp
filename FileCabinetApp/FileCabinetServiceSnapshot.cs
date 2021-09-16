using System;
using System.Collections.Generic;
using System.IO;

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
    }
}
