using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that creates validator with custom settings.
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Creates validator with custom settings.
        /// </summary>
        /// <returns>Validator with custom settings.</returns>
        protected override IRecordValidator CreateValidator()
        {
            return new CustomValidator();
        }
    }
}
