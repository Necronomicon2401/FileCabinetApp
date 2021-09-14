using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Interface for validators.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Validate record.
        /// </summary>
        /// <param name="record">Record to validate.</param>
        public void ValidateParameters(FileCabinetRecord record);
    }
}
