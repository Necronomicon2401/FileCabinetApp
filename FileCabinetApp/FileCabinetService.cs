using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

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

            if (this.firstNameDictionary.ContainsKey(firstName))
            {
                this.firstNameDictionary[firstName].Add(record);
            }
            else
            {
                List<FileCabinetRecord> list = new List<FileCabinetRecord>();
                list.Add(record);
                this.firstNameDictionary.Add(firstName, list);
            }

            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary[lastName].Add(record);
            }
            else
            {
                List<FileCabinetRecord> list = new List<FileCabinetRecord>();
                list.Add(record);
                this.lastNameDictionary.Add(lastName, list);
            }

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
                    var record = new FileCabinetRecord
                    {
                        Id = id,
                        FirstName = firstName,
                        LastName = lastName,
                        DateOfBirth = dateOfBirth,
                        WorkExperience = workExperience,
                        Weight = weight,
                        LuckySymbol = luckySymbol,
                    };

                    this.firstNameDictionary[this.list[i].FirstName].Remove(this.list[i]);
                    if (this.firstNameDictionary.ContainsKey(firstName))
                    {
                        this.firstNameDictionary[firstName].Add(record);
                    }
                    else
                    {
                        List<FileCabinetRecord> list = new List<FileCabinetRecord>();
                        list.Add(record);
                        this.firstNameDictionary.Add(firstName, list);
                    }

                    this.lastNameDictionary[this.list[i].LastName].Remove(this.list[i]);
                    if (this.lastNameDictionary.ContainsKey(lastName))
                    {
                        this.lastNameDictionary[lastName].Add(record);
                    }
                    else
                    {
                        List<FileCabinetRecord> list = new List<FileCabinetRecord>();
                        list.Add(record);
                        this.lastNameDictionary.Add(lastName, list);
                    }

                    this.list[i] = record;
                    return;
                }
            }

            throw new ArgumentException("There is no record with such id");
        }

        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            return this.firstNameDictionary[firstName].ToArray();
        }

        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            return this.lastNameDictionary[lastName].ToArray();
        }

        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            List<FileCabinetRecord> listWithRecords = new List<FileCabinetRecord>();
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].DateOfBirth.Equals(dateOfBirth))
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
