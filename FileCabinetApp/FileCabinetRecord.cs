using System;

namespace FileCabinetApp
{
    public class FileCabinetRecord
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }

        public short WorkExperience { get; set; }

        public decimal Weight { get; set; }

        public char LuckySymbol { get; set; }
    }
}