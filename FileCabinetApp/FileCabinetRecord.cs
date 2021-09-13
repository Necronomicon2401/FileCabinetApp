using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that represent record.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets id of record.
        /// </summary>
        /// <value>Property <c>Id</c> represents the id of record.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets person first name in record.
        /// </summary>
        /// <value>Property <c>FirstName</c> represents the first name of person in record.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets person last name in record.
        /// </summary>
        /// <value>Property <c>LastName</c> represents the last name of person in record.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets person birth date in record.
        /// </summary>
        /// <value>Property <c>DateOfBirth</c> represents the birth date of person in record.</value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets person work experience in record.
        /// </summary>
        /// <value>Property <c>WorkExperience</c> represents the work experience of person in record.</value>
        public short WorkExperience { get; set; }

        /// <summary>
        /// Gets or sets person weight in record.
        /// </summary>
        /// <value>Property <c>Weight</c> represents the weight of person in record.</value>
        public decimal Weight { get; set; }

        /// <summary>
        /// Gets or sets person lucky symbol in record.
        /// </summary>
        /// <value>Property <c>LuckySymbol</c> represents the lucky symbol of person in record.</value>
        public char LuckySymbol { get; set; }
    }
}