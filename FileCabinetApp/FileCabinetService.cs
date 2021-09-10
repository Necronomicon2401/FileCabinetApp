using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

        public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short workExperience, decimal weight, char luckySymbol)
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

            var record = new FileCabinetRecord
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

            return record.Id;
        }

        public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short workExperience, decimal weight, char luckySymbol)
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

            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].Id == id)
                {
                    this.list[i].FirstName = firstName;
                    this.list[i].LastName = lastName;
                    this.list[i].DateOfBirth = dateOfBirth;
                    this.list[i].WorkExperience = workExperience;
                    this.list[i].Weight = weight;
                    this.list[i].LuckySymbol = luckySymbol;
                    return;
                }
            }

            throw new ArgumentException("There is no record with such id");
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            List<FileCabinetRecord> listWithRecords = new List<FileCabinetRecord>();
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].FirstName.ToLower().Equals(firstName.ToLower()))
                {
                    listWithRecords.Add(this.list[i]);
                }
            }

            return listWithRecords.ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            List<FileCabinetRecord> listWithRecords = new List<FileCabinetRecord>();
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].LastName.ToLower().Equals(lastName.ToLower()))
                {
                    listWithRecords.Add(this.list[i]);
                }
            }

            return listWithRecords.ToArray();
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
