using System;
using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    /// <summary>
    /// Interface for FileCabinetService.
    /// </summary>
    public interface IFileCabinetService
    {
        /// <summary>
        /// Creates record as FileCabinetRecord object and add it to all dictionaries.
        /// </summary>
        /// <param name="record">Person record.</param>
        /// <returns>Return created record id.</returns>
        public int CreateRecord(FileCabinetRecord record);

        /// <summary>
        /// Change record if it exist and update all dictionaries.
        /// </summary>
        /// <param name="record">Edited record.</param>
        public void EditRecord(FileCabinetRecord record);

        /// <summary>
        /// Finds records according to first name.
        /// </summary>
        /// <param name="firstName">First name to search for.</param>
        /// <returns>ReadOnlyColletion of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);

        /// <summary>
        /// Finds records according to last name.
        /// </summary>
        /// <param name="lastName">Last name to search for.</param>
        /// <returns>ReadOnlyColletion of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);

        /// <summary>
        /// Finds records according to date of birth.
        /// </summary>
        /// <param name="dateOfBirth">Date of birth to search for.</param>
        /// <returns>ReadOnlyColletion of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(DateTime dateOfBirth);

        /// <summary>
        /// Gets all records.
        /// </summary>
        /// <returns>ReadOnlyColletion of records.</returns>
        public ReadOnlyCollection<FileCabinetRecord> GetRecords();

        /// <summary>
        /// Gets count of all records.
        /// </summary>
        /// <returns>Count of all records.</returns>
        public int GetStat();
    }
}
