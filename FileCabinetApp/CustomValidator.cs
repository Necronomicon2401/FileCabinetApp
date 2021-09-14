using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Validator class with custom parameters.
    /// </summary>
    public class CustomValidator : IRecordValidator
    {
        /// <summary>
        /// Validate record with custom parameters.
        /// </summary>
        /// <param name="record">Record to validate.</param>
        public void ValidateParameters(FileCabinetRecord record)
        {
            if (string.IsNullOrWhiteSpace(record.FirstName))
            {
                throw new ArgumentNullException(nameof(record.FirstName));
            }

            if (record.FirstName.Length < 4 || record.FirstName.Length > 30)
            {
                throw new ArgumentException("First name length must be greater then 5 and less then 31");
            }

            if (string.IsNullOrWhiteSpace(record.LastName))
            {
                throw new ArgumentNullException(nameof(record.LastName));
            }

            if (record.LastName.Length < 4 || record.LastName.Length > 30)
            {
                throw new ArgumentException("Last name length must be greater then 5 and less then 31");
            }

            if (record.DateOfBirth > new DateTime(2003, 1, 1) || record.DateOfBirth < new DateTime(1960, 2, 24))
            {
                throw new ArgumentException("Date of birth should be greater then 02/24/1960 and less then 01/01/2003");
            }

            if (record.WorkExperience < 4 || record.WorkExperience > 29)
            {
                throw new ArgumentException("Work experience cannot be less then 5 or greater then 30");
            }

            if (record.Weight < 49 || record.Weight > 99)
            {
                throw new ArgumentException("Weight cannot be less then 50 or greater then 100");
            }

            if (record.LuckySymbol == ' ' || char.IsDigit(record.LuckySymbol))
            {
                throw new ArgumentException("Lucky symbol cannot be empty or digit");
            }
        }
    }
}
