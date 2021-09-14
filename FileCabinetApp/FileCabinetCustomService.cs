using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Class that creates new instance of <see cref="FileCabinetCustomService"/> class with custom validator settings.
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetCustomService"/> class with custom validator.
        /// </summary>
        public FileCabinetCustomService()
            : base(new CustomValidator())
        {
        }
    }
}
