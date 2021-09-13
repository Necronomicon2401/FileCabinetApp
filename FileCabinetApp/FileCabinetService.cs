using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    /// <summary>
    /// Application service class.
    /// </summary>
    public class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new ();

        /// <summary>
        /// Creates record as FileCabinetRecord object and add it to all dictionaries.
        /// </summary>
        /// <param name="firstName">Person first name.</param>
        /// <param name="lastName">Person last name.</param>
        /// <param name="dateOfBirth">Person date of birth.</param>
        /// <param name="workExperience">Person work experience.</param>
        /// <param name="weight">Person weight.</param>
        /// <param name="luckySymbol">Person lucky symbol.</param>
        /// <returns>Return created record id.</returns>
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
                List<FileCabinetRecord> list = new ();
                list.Add(record);
                this.firstNameDictionary.Add(firstName, list);
            }

            if (this.lastNameDictionary.ContainsKey(lastName))
            {
                this.lastNameDictionary[lastName].Add(record);
            }
            else
            {
                List<FileCabinetRecord> list = new ();
                list.Add(record);
                this.lastNameDictionary.Add(lastName, list);
            }

            if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                this.dateOfBirthDictionary[dateOfBirth].Add(record);
            }
            else
            {
                List<FileCabinetRecord> list = new ();
                list.Add(record);
                this.dateOfBirthDictionary.Add(dateOfBirth, list);
            }

            return record.Id;
        }

        /// <summary>
        /// Edits record if it exist and update all dictionaries.
        /// </summary>
        /// <param name="id">Record id.</param>
        /// <param name="firstName">Edited first name.</param>
        /// <param name="lastName">Edited last name.</param>
        /// <param name="dateOfBirth">Edited date of birth.</param>
        /// <param name="workExperience">Edited work experience.</param>
        /// <param name="weight">Edited weight.</param>
        /// <param name="luckySymbol">Edited lucky symbol.</param>
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
                        List<FileCabinetRecord> list = new ();
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
                        List<FileCabinetRecord> list = new ();
                        list.Add(record);
                        this.lastNameDictionary.Add(lastName, list);
                    }

                    this.dateOfBirthDictionary[this.list[i].DateOfBirth].Remove(this.list[i]);
                    if (this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
                    {
                        this.dateOfBirthDictionary[dateOfBirth].Add(record);
                    }
                    else
                    {
                        List<FileCabinetRecord> list = new ();
                        list.Add(record);
                        this.dateOfBirthDictionary.Add(dateOfBirth, list);
                    }

                    this.list[i] = record;
                    return;
                }
            }

            throw new ArgumentException("There is no record with such id");
        }

        /// <summary>
        /// Finds records according to first name.
        /// </summary>
        /// <param name="firstName">First name to search for.</param>
        /// <returns>Array of records.</returns>
        public FileCabinetRecord[] FindByFirstName(string firstName)
        {
            if (!this.firstNameDictionary.ContainsKey(firstName))
            {
                return null;
            }
            else
            {
                return this.firstNameDictionary[firstName].ToArray();
            }
        }

        /// <summary>
        /// Finds records according to last name.
        /// </summary>
        /// <param name="lastName">Last name to search for.</param>
        /// <returns>Array of records.</returns>
        public FileCabinetRecord[] FindByLastName(string lastName)
        {
            if (!this.lastNameDictionary.ContainsKey(lastName))
            {
                return null;
            }
            else
            {
                return this.lastNameDictionary[lastName].ToArray();
            }
        }

        /// <summary>
        /// Finds records according to date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth to search for.</param>
        /// <returns>Array of records.</returns>
        public FileCabinetRecord[] FindByDateOfBirth(DateTime dateOfBirth)
        {
            if (!this.dateOfBirthDictionary.ContainsKey(dateOfBirth))
            {
                return null;
            }
            else
            {
                return this.dateOfBirthDictionary[dateOfBirth].ToArray();
            }
        }

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>Array of all records.</returns>
        public FileCabinetRecord[] GetRecords()
        {
            return this.list.ToArray();
        }

        /// <summary>
        /// Gets count of all records.
        /// </summary>
        /// <returns>Count of all records.</returns>
        public int GetStat()
        {
            return this.list.Count;
        }
    }
}
