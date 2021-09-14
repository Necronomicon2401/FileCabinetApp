using System;
using System.Collections.Generic;

namespace FileCabinetApp
{
    /// <summary>
    /// Application service class.
    /// </summary>
    public abstract class FileCabinetService
    {
        private readonly List<FileCabinetRecord> list = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new ();
        private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new ();
        private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new ();
        private readonly IRecordValidator validator;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetService"/> class.
        /// </summary>
        /// <param name="validator">Type of validator.</param>
        protected FileCabinetService(IRecordValidator validator)
        {
            this.validator = validator;
        }

        /// <summary>
        /// Creates record as FileCabinetRecord object and add it to all dictionaries.
        /// </summary>
        /// <param name="record">Person record.</param>
        /// <returns>Return created record id.</returns>
        public int CreateRecord(FileCabinetRecord record)
        {
            this.validator.ValidateParameters(record);
            record.Id = this.list.Count + 1;
            this.list.Add(record);

            if (this.firstNameDictionary.ContainsKey(record.FirstName))
            {
                this.firstNameDictionary[record.FirstName].Add(record);
            }
            else
            {
                List<FileCabinetRecord> list = new ();
                list.Add(record);
                this.firstNameDictionary.Add(record.FirstName, list);
            }

            if (this.lastNameDictionary.ContainsKey(record.LastName))
            {
                this.lastNameDictionary[record.LastName].Add(record);
            }
            else
            {
                List<FileCabinetRecord> list = new ();
                list.Add(record);
                this.lastNameDictionary.Add(record.LastName, list);
            }

            if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
            {
                this.dateOfBirthDictionary[record.DateOfBirth].Add(record);
            }
            else
            {
                List<FileCabinetRecord> list = new ();
                list.Add(record);
                this.dateOfBirthDictionary.Add(record.DateOfBirth, list);
            }

            return record.Id;
        }

        /// <summary>
        /// Change record if it exist and update all dictionaries.
        /// </summary>
        /// <param name="record">Edited record.</param>
        public void EditRecord(FileCabinetRecord record)
        {
            this.validator.ValidateParameters(record);
            for (int i = 0; i < this.list.Count; i++)
            {
                if (this.list[i].Id == record.Id)
                {
                    this.firstNameDictionary[this.list[i].FirstName].Remove(this.list[i]);
                    if (this.firstNameDictionary.ContainsKey(record.FirstName))
                    {
                        this.firstNameDictionary[record.FirstName].Add(record);
                    }
                    else
                    {
                        List<FileCabinetRecord> list = new ();
                        list.Add(record);
                        this.firstNameDictionary.Add(record.FirstName, list);
                    }

                    this.lastNameDictionary[this.list[i].LastName].Remove(this.list[i]);
                    if (this.lastNameDictionary.ContainsKey(record.LastName))
                    {
                        this.lastNameDictionary[record.LastName].Add(record);
                    }
                    else
                    {
                        List<FileCabinetRecord> list = new ();
                        list.Add(record);
                        this.lastNameDictionary.Add(record.LastName, list);
                    }

                    this.dateOfBirthDictionary[this.list[i].DateOfBirth].Remove(this.list[i]);
                    if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
                    {
                        this.dateOfBirthDictionary[record.DateOfBirth].Add(record);
                    }
                    else
                    {
                        List<FileCabinetRecord> list = new ();
                        list.Add(record);
                        this.dateOfBirthDictionary.Add(record.DateOfBirth, list);
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
