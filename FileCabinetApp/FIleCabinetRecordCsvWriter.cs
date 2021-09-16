using System;
using System.Globalization;
using System.IO;

namespace FileCabinetApp
{
    /// <summary>
    /// CSV writer class.
    /// </summary>
    public class FileCabinetRecordCsvWriter
    {
        private readonly TextWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">Writer to use.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Write the record by using current writer.
        /// </summary>
        /// <param name="record">Record for writing.</param>
        public void Write(FileCabinetRecord record)
        {
            CultureInfo ci = new ("en-US");
            this.writer.WriteLine(record.Id + "," + record.FirstName + "," + record.LastName + "," + record.DateOfBirth.ToString("MM/dd/yyyy", ci) + "," + record.WorkExperience + "," + record.Weight + "," + record.LuckySymbol);
        }
    }
}
