using System;
using System.Globalization;
using System.IO;

namespace FileCabinetApp
{
    /// <summary>
    /// Main class of FileCabinetApp.
    /// </summary>
    public static class Program
    {
        private const string DeveloperName = "Dmitry Bolbat";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static readonly Tuple<string, Action<string>>[] Commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("export", Export),
        };

        private static readonly string[][] HelpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "shows the number of records that the service stores", "The 'stat' command shows the number of records that the service stores." },
            new string[] { "create", "creates new record", "The 'create' command creates new record." },
            new string[] { "list", "shows list of records", "The 'list' command shows list of records." },
            new string[] { "edit", "edits created record by id", "The 'edit' command allows to edit created record by id." },
            new string[] { "find", "finds and shows created records by inputed property", "The 'find' command finds and shows created records by inputed property." },
            new string[] { "export", "exports current records to csv or xml file", "The 'export' command exports current records to csv or xml file." },
        };

        private static string typeOfValidation;

        private static FileCabinetRecord defaultValidationRecord = new FileCabinetRecord
        {
            Id = 0,
            FirstName = "Sam",
            LastName = "Dif",
            DateOfBirth = new DateTime(1950, 5, 5),
            WorkExperience = 1,
            Weight = 10,
            LuckySymbol = '7',
        };

        private static FileCabinetRecord customValidationRecord = new FileCabinetRecord
        {
            Id = 0,
            FirstName = "FirstName",
            LastName = "LastName",
            DateOfBirth = new DateTime(2000, 1, 1),
            WorkExperience = 15,
            Weight = 70,
            LuckySymbol = 'S',
        };

        private static IFileCabinetService fileCabinetServiceInterface;

        private static bool isRunning = true;

        /// <summary>
        /// Starting point of console application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            if (args is null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
            UsingValidationRules(args);
            Console.WriteLine(Program.HintMessage);
            Console.WriteLine();

            do
            {
                Console.Write("> ");
                var inputs = Console.ReadLine().Split(' ', 2);
                const int commandIndex = 0;
                var command = inputs[commandIndex];

                if (string.IsNullOrEmpty(command))
                {
                    Console.WriteLine(Program.HintMessage);
                    continue;
                }

                var index = Array.FindIndex(Commands, 0, Commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    Commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        /// <summary>
        /// Specifying the type of validation rules.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        private static void UsingValidationRules(string[] args)
        {
            if (args.Length == 0)
            {
                fileCabinetServiceInterface = new FileCabinetService(new DefaultValidator());
                Console.WriteLine("Using default validation rules.");
                typeOfValidation = "default";
                return;
            }

            if (args[0].ToLower() == "--validation-rules=default")
            {
                fileCabinetServiceInterface = new FileCabinetService(new DefaultValidator());
                Console.WriteLine("Using default validation rules.");
                typeOfValidation = "default";
                return;
            }

            if (args[0].ToLower() == "--validation-rules=custom")
            {
                fileCabinetServiceInterface = new FileCabinetService(new CustomValidator());
                Console.WriteLine("Using custom validation rules.");
                typeOfValidation = "custom";
                return;
            }

            if (args[0] == "-v")
            {
                if (args[1].ToLower() == "default")
                {
                    fileCabinetServiceInterface = new FileCabinetService(new DefaultValidator());
                    Console.WriteLine("Using default validation rules.");
                    typeOfValidation = "default";
                    return;
                }

                if (args[1].ToLower() == "custom")
                {
                    fileCabinetServiceInterface = new FileCabinetService(new CustomValidator());
                    Console.WriteLine("Using custom validation rules.");
                    typeOfValidation = "custom";
                    return;
                }
            }
        }

        /// <summary>
        /// Prints information about not existing command writed by user.
        /// </summary>
        /// <param name="command">String representation of writed command.</param>
        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        /// <summary>
        /// Prints information about all commands that application support.
        /// </summary>
        /// <param name="parameters">String representation of writed command parameters.</param>
        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(HelpMessages, 0, HelpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(HelpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in HelpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Exits the application.
        /// </summary>
        /// <param name="parameters">String representation of writed command parameters.</param>
        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        /// <summary>
        /// Prints the number of records that the service stores.
        /// </summary>
        /// <param name="parameters">String representation of writed command parameters.</param>
        private static void Stat(string parameters)
        {
            Console.WriteLine($"{Program.fileCabinetServiceInterface.GetStat()} record(s).");
        }

        /// <summary>
        /// Reads input and validate it.
        /// </summary>
        /// <param name="converter">Function for converter.</param>
        /// <param name="validator">Function for validator.</param>
        /// <returns>Input in nessecary type.</returns>
        private static T ReadInput<T>(Func<string, Tuple<bool, string, T>> converter, Func<T, Tuple<bool, string>> validator)
        {
            do
            {
                T value;

                var input = Console.ReadLine();
                var conversionResult = converter(input);

                if (!conversionResult.Item1)
                {
                    Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                    continue;
                }

                value = conversionResult.Item3;

                var validationResult = validator(value);
                if (!validationResult.Item1)
                {
                    Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                    continue;
                }

                return value;
            }
            while (true);
        }

        /// <summary>
        /// Convert string.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <returns>Tuple that represent convert status and converted result.</returns>
        private static Tuple<bool, string, string> StringConverter(string str)
        {
            return new Tuple<bool, string, string>(true, str, str);
        }

        /// <summary>
        /// Convert DateTime.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <returns>Tuple that represent convert status and converted result.</returns>
        private static Tuple<bool, string, DateTime> DateConverter(string str)
        {
            string datePattern = "MM/dd/yyyy";
            var parsed = DateTime.TryParseExact(str, datePattern, null, 0, out DateTime dateOfBirth);
            return new Tuple<bool, string, DateTime>(parsed, str, dateOfBirth);
        }

        /// <summary>
        /// Convert short.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <returns>Tuple that represent convert status and converted result.</returns>
        private static Tuple<bool, string, short> ShortConverter(string str)
        {
            var parsed = short.TryParse(str, out short sh);
            return new Tuple<bool, string, short>(parsed, str, sh);
        }

        /// <summary>
        /// Convert decimal.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <returns>Tuple that represent convert status and converted result.</returns>
        private static Tuple<bool, string, decimal> DecimalConverter(string str)
        {
            var parsed = decimal.TryParse(str, out decimal dcm);
            return new Tuple<bool, string, decimal>(parsed, str, dcm);
        }

        /// <summary>
        /// Convert char.
        /// </summary>
        /// <param name="str">String to convert.</param>
        /// <returns>Tuple that represent convert status and converted result.</returns>
        private static Tuple<bool, string, char> CharConverter(string str)
        {
            var parsed = char.TryParse(str, out char ch);
            return new Tuple<bool, string, char>(parsed, str, ch);
        }

        /// <summary>
        /// First name validation.
        /// </summary>
        /// <param name="firstName">String to validate.</param>
        /// <returns>Tuple with bool that represent validation status and string if exception catched.</returns>
        private static Tuple<bool, string> FirstNameValidator(string firstName)
        {
            FileCabinetRecord record = defaultValidationRecord;

            if (typeOfValidation.Equals("custom"))
            {
                record = customValidationRecord;
            }

            record.FirstName = firstName;
            return fileCabinetServiceInterface.StartValidation(record);
        }

        /// <summary>
        /// Last name validation.
        /// </summary>
        /// <param name="lastName">String to validate.</param>
        /// <returns>Tuple with bool that represent validation status and string if exception catched.</returns>
        private static Tuple<bool, string> LastNameValidator(string lastName)
        {
            FileCabinetRecord record = defaultValidationRecord;

            if (typeOfValidation.Equals("custom"))
            {
                record = customValidationRecord;
            }

            record.LastName = lastName;
            return fileCabinetServiceInterface.StartValidation(record);
        }

        /// <summary>
        /// Date of birth validation.
        /// </summary>
        /// <param name="dateOfBirth">DateTime object to validate.</param>
        /// <returns>Tuple with bool that represent validation status and string if exception catched.</returns>
        private static Tuple<bool, string> DateOfBirthValidator(DateTime dateOfBirth)
        {
            FileCabinetRecord record = defaultValidationRecord;

            if (typeOfValidation.Equals("custom"))
            {
                record = customValidationRecord;
            }

            record.DateOfBirth = dateOfBirth;
            return fileCabinetServiceInterface.StartValidation(record);
        }

        /// <summary>
        /// Work experience validation.
        /// </summary>
        /// <param name="workExperience">Short object to validate.</param>
        /// <returns>Tuple with bool that represent validation status and string if exception catched.</returns>
        private static Tuple<bool, string> WorkExperienceValidator(short workExperience)
        {
            FileCabinetRecord record = defaultValidationRecord;

            if (typeOfValidation.Equals("custom"))
            {
                record = customValidationRecord;
            }

            record.WorkExperience = workExperience;
            return fileCabinetServiceInterface.StartValidation(record);
        }

        /// <summary>
        /// Weightvalidation.
        /// </summary>
        /// <param name="weight">Decimal object to validate.</param>
        /// <returns>Tuple with bool that represent validation status and string if exception catched.</returns>
        private static Tuple<bool, string> WeightValidator(decimal weight)
        {
            FileCabinetRecord record = defaultValidationRecord;

            if (typeOfValidation.Equals("custom"))
            {
                record = customValidationRecord;
            }

            record.Weight = weight;
            return fileCabinetServiceInterface.StartValidation(record);
        }

        /// <summary>
        /// Lucky symbol validation.
        /// </summary>
        /// <param name="luckySymbol">Char object to validate.</param>
        /// <returns>Tuple with bool that represent validation status and string if exception catched.</returns>
        private static Tuple<bool, string> LuckySymbolValidator(char luckySymbol)
        {
            FileCabinetRecord record = defaultValidationRecord;

            if (typeOfValidation.Equals("custom"))
            {
                record = customValidationRecord;
            }

            record.LuckySymbol = luckySymbol;
            return fileCabinetServiceInterface.StartValidation(record);
        }

        /// <summary>
        /// Creates new record and save it as FileCabinetRecord object.
        /// </summary>
        /// <param name="parameters">String representation of writed command parameters.</param>
        private static void Create(string parameters)
        {
            FileCabinetRecord newRecord = new ();
            Console.Write("First name: ");
            newRecord.FirstName = ReadInput(StringConverter, FirstNameValidator);
            Console.Write("Last name: ");
            newRecord.LastName = ReadInput(StringConverter, LastNameValidator);
            Console.Write("Date of birth: ");
            newRecord.DateOfBirth = ReadInput(DateConverter, DateOfBirthValidator);
            Console.Write("Work experience: ");
            newRecord.WorkExperience = ReadInput(ShortConverter, WorkExperienceValidator);
            Console.Write("Weight: ");
            newRecord.Weight = ReadInput(DecimalConverter, WeightValidator);
            Console.Write("Lucky symbol: ");
            newRecord.LuckySymbol = ReadInput(CharConverter, LuckySymbolValidator);

            try
            {
                int id = fileCabinetServiceInterface.CreateRecord(newRecord);
                Console.WriteLine($"Record #{id} is created");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Edits existing record and save it as FileCabinetRecord object.
        /// </summary>
        /// <param name="parameters">String representation of writed command parameters.</param>
        private static void Edit(string parameters)
        {
            var isIdParsed = int.TryParse(parameters, out int id);
            if (!isIdParsed)
            {
                Console.WriteLine("Record id must be a number");
                return;
            }

            var records = fileCabinetServiceInterface.GetRecords();
            bool isRecordExist = false;
            for (int i = 0; i < records.Count; i++)
            {
                if (records[i].Id == id)
                {
                    isRecordExist = true;
                }
            }

            if (isRecordExist)
            {
                FileCabinetRecord updatedRecord = new ();
                updatedRecord.Id = id;
                Console.Write("First name: ");
                updatedRecord.FirstName = ReadInput(StringConverter, FirstNameValidator);
                Console.Write("Last name: ");
                updatedRecord.LastName = ReadInput(StringConverter, LastNameValidator);
                Console.Write("Date of birth: ");
                updatedRecord.DateOfBirth = ReadInput(DateConverter, DateOfBirthValidator);
                Console.Write("Work experience: ");
                updatedRecord.WorkExperience = ReadInput(ShortConverter, WorkExperienceValidator);
                Console.Write("Weight: ");
                updatedRecord.Weight = ReadInput(DecimalConverter, WeightValidator);
                Console.Write("Lucky symbol: ");
                updatedRecord.LuckySymbol = ReadInput(CharConverter, LuckySymbolValidator);

                try
                {
                    fileCabinetServiceInterface.EditRecord(updatedRecord);
                    Console.WriteLine($"Record #{id} is updated");
                }
                catch (ArgumentNullException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
            else
            {
                Console.WriteLine($"#{id} record is not found");
            }
        }

        /// <summary>
        /// Finds and prints records according to some property.
        /// </summary>
        /// <param name="parameters">String representation of writed command parameters.</param>
        private static void Find(string parameters)
        {
            CultureInfo ci = new ("en-US");
            var inputs = parameters.Split(' ', 2);
            if (inputs.Length != 2)
            {
                Console.WriteLine("Please input property of search and text for search like \" find firstname \"Ivan\" \" ");
                return;
            }

            string property = inputs[0];
            string text = inputs[1].Replace("\"", string.Empty);
            if (property.ToLower().Equals("firstname"))
            {
                var list = fileCabinetServiceInterface.FindByFirstName(text);
                if (list is null)
                {
                    Console.WriteLine("There aren't such records");
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        Console.WriteLine($"#{list[i].Id}, {list[i].FirstName}, {list[i].LastName}, {list[i].DateOfBirth.ToString("yyyy'-'MMM'-'dd", ci)}, Work experience: {list[i].WorkExperience}, Weight: {list[i].Weight}, Lucky symbol: {list[i].LuckySymbol}");
                    }
                }
            }
            else if (property.ToLower().Equals("lastname"))
            {
                var list = fileCabinetServiceInterface.FindByLastName(text);
                if (list is null)
                {
                    Console.WriteLine("There aren't such records");
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        Console.WriteLine($"#{list[i].Id}, {list[i].FirstName}, {list[i].LastName}, {list[i].DateOfBirth.ToString("yyyy'-'MMM'-'dd", ci)}, Work experience: {list[i].WorkExperience}, Weight: {list[i].Weight}, Lucky symbol: {list[i].LuckySymbol}");
                    }
                }
            }
            else if (property.ToLower().Equals("dateofbirth"))
            {
                string pattern = "yyyy-MMM-dd";
                var parsed = DateTime.TryParseExact(text, pattern, ci, 0, out DateTime dateOfBirth);
                if (!parsed)
                {
                    Console.WriteLine("Invalid date type, please, use yyyy-MMM-dd (2001-Dec-01) pattern");
                    return;
                }

                var list = fileCabinetServiceInterface.FindByDateOfBirth(dateOfBirth);
                if (list is null)
                {
                    Console.WriteLine("There aren't such records");
                }
                else
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        Console.WriteLine($"#{list[i].Id}, {list[i].FirstName}, {list[i].LastName}, {list[i].DateOfBirth.ToString("yyyy'-'MMM'-'dd", ci)}, Work experience: {list[i].WorkExperience}, Weight: {list[i].Weight}, Lucky symbol: {list[i].LuckySymbol}");
                    }
                }
            }
            else
            {
                Console.WriteLine("There isn't such property for search");
            }
        }

        /// <summary>
        /// Prints all existing records.
        /// </summary>
        /// <param name="parameters">String representation of writed command parameters.</param>
        private static void List(string parameters)
        {
            var list = fileCabinetServiceInterface.GetRecords();
            CultureInfo ci = new ("en-US");
            if (list.Count == 0)
            {
                Console.WriteLine("List of records is empty, please use 'create' command to add record");
            }
            else
            {
                for (int i = 0; i < list.Count; i++)
                {
                    Console.WriteLine($"#{list[i].Id}, {list[i].FirstName}, {list[i].LastName}, {list[i].DateOfBirth.ToString("yyyy'-'MMM'-'dd", ci)}, Work experience: {list[i].WorkExperience}, Weight: {list[i].Weight}, Lucky symbol: {list[i].LuckySymbol}");
                }
            }
        }

        /// <summary>
        /// Export all existing records.
        /// </summary>
        /// <param name="parameters">String representation of writed command parameters.</param>
        private static void Export(string parameters)
        {
            if (fileCabinetServiceInterface.GetRecords().Count == 0)
            {
                Console.WriteLine("List of records is empty, please use 'create' command to create record");
            }

            var inputs = parameters.Split(' ', 2);
            if (inputs.Length != 2)
            {
                Console.WriteLine("Please input property of export  like \"export csv filename.csv \" ");
                return;
            }

            string exportType = inputs[0];
            string fileName = inputs[1];

            try
            {
                FileStream fileStream = new (fileName, FileMode.Open);
                fileStream.Close();
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Export failed: can't open file {fileName}");
                return;
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine($"Export failed: can't access to the file {fileName}");
                return;
            }
            catch (FileNotFoundException)
            {
                FileStream fileStream = new (fileName, FileMode.Create);
                fileStream.Close();
                StreamWriter writer = new (fileName);
                writer.AutoFlush = true;
                FileCabinetServiceSnapshot snapshot = fileCabinetServiceInterface.MakeSnapshot();
                if (exportType == "csv")
                {
                    snapshot.SaveToCsv(writer);
                    Console.WriteLine($"All records are exported to file {fileName}");
                }

                writer.Close();
                return;
            }

            Console.Write($"File is exist - rewrite {fileName}? [Y/n] ");
            var input = Console.ReadLine();
            if (input.ToLower().Equals("y"))
            {
                StreamWriter writer = new (fileName);
                writer.AutoFlush = true;
                FileCabinetServiceSnapshot snapshot = fileCabinetServiceInterface.MakeSnapshot();
                if (exportType == "csv")
                {
                    snapshot.SaveToCsv(writer);
                    Console.WriteLine($"All records are exported to file {fileName}");
                }

                writer.Close();
            }

            if (input.ToLower().Equals("n"))
            {
                return;
            }
        }
    }
}