using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short workExperience, decimal weight, char luckySymbol)
        {
            var record = new FileCabinetRecord();
            try
            {
                if (string.IsNullOrWhiteSpace(firstName))
                {
                    throw new ArgumentNullException(nameof(firstName));
                }

                if (firstName.Length < 2 || firstName.Length > 60)
                {
                    throw new ArgumentException("First name length must be greater then 1 and less then 61");
                }

                if (string.IsNullOrWhiteSpace(lastName))
                {
                    throw new ArgumentNullException(nameof(lastName));
                }

                if (lastName.Length < 2 || lastName.Length > 60)
                {
                    throw new ArgumentException("Last name length must be greater then 1 and less then 61");
                }

                if (dateOfBirth > DateTime.Now || dateOfBirth < new DateTime(1950, 1, 1))
                {
                    throw new ArgumentException("Date of birth should be greater then 01/01/1950 and less then current date");
                }

                if (workExperience < 0 || workExperience > 70)
                {
                    throw new ArgumentException("Work experience cannot be less then 0 or greater then 70");
                }

                if (weight < 0 || weight > 200)
                {
                    throw new ArgumentException("Weight cannot be less then 0 or greater then 200");
                }

                if (luckySymbol == ' ')
                {
                    throw new ArgumentException("Lucky symbol cannot be empty");
                }

                record = new FileCabinetRecord
                {
                    Id = this.list.Count + 1,
                    FirstName = firstName,
                    LastName = lastName,
                    DateOfBirth = dateOfBirth,
                    WorkExperience = workExperience,
                    Weight = weight,
                    LuckySymbol = luckySymbol,
                };

                this.list.Add(record);
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }

            return record.Id;
        }

        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        public int GetStat()
        {
            return this.list.Count;
        }
    }
}
