using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Validator class with default parameters.
    /// </summary>
    public class DefaultValidator : IRecordValidator
    {
        /// <summary>
        /// Validate record with default parameters.
        /// </summary>
        /// <param name="record">Record to validate.</param>
        public void ValidateParameters(FileCabinetRecord record)
        {
            if (string.IsNullOrWhiteSpace(record.FirstName))
            {
                throw new ArgumentNullException(nameof(record.FirstName));
            }

            if (record.FirstName.Length < 2 || record.FirstName.Length > 60)
            {
                throw new ArgumentException("First name length must be greater then 1 and less then 61");
            }

            if (string.IsNullOrWhiteSpace(record.LastName))
            {
                throw new ArgumentNullException(nameof(record.LastName));
            }

            if (record.LastName.Length < 2 || record.LastName.Length > 60)
            {
                throw new ArgumentException("Last name length must be greater then 1 and less then 61");
            }

            if (record.DateOfBirth > DateTime.Now || record.DateOfBirth < new DateTime(1950, 1, 1))
            {
                throw new ArgumentException("Date of birth should be greater then 01/01/1950 and less then current date");
            }

            if (record.WorkExperience < 0 || record.WorkExperience > 70)
            {
                throw new ArgumentException("Work experience cannot be less then 0 or greater then 70");
            }

            if (record.Weight < 0 || record.Weight > 200)
            {
                throw new ArgumentException("Weight cannot be less then 0 or greater then 200");
            }

            if (record.LuckySymbol == ' ')
            {
                throw new ArgumentException("Lucky symbol cannot be empty");
            }
        }
    }
}
