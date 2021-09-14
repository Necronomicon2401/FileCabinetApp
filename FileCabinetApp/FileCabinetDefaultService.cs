using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that creates validator with default settings.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Creates validator with default settings.
        /// </summary>
        /// <returns>Validator with default settings.</returns>
        protected override IRecordValidator CreateValidator()
        {
            return new DefaultValidator();
        }
    }
}
