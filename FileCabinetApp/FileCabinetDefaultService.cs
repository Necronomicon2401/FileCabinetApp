using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that creates new instance of <see cref="FileCabinetDefaultService"/> class with default validator settings.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetDefaultService"/> class with default validator.
        /// </summary>
        public FileCabinetDefaultService()
            : base(new DefaultValidator())
        {
        }
    }
}
