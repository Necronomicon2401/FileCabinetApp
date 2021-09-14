using System;
using System.Globalization;

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
        };

        private static FileCabinetService fileCabinetService;

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
                fileCabinetService = new FileCabinetDefaultService();
                Console.WriteLine("Using default validation rules.");
                return;
            }

            if (args[0].ToLower() == "--validation-rules=default")
            {
                fileCabinetService = new FileCabinetDefaultService();
                Console.WriteLine("Using default validation rules.");
                return;
            }

            if (args[0].ToLower() == "--validation-rules=custom")
            {
                fileCabinetService = new FileCabinetCustomService();
                Console.WriteLine("Using custom validation rules.");
                return;
            }

            if (args[0] == "-v")
            {
                if (args[1].ToLower() == "default")
                {
                    fileCabinetService = new FileCabinetDefaultService();
                    Console.WriteLine("Using default validation rules.");
                    return;
                }

                if (args[1].ToLower() == "custom")
                {
                    fileCabinetService = new FileCabinetCustomService();
                    Console.WriteLine("Using custom validation rules.");
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
            Console.WriteLine($"{Program.fileCabinetService.GetStat()} record(s).");
        }

        /// <summary>
        /// Creates new record and save it as FileCabinetRecord object.
        /// </summary>
        /// <param name="parameters">String representation of writed command parameters.</param>
        private static void Create(string parameters)
        {
            FileCabinetRecord newRecord = new ();
            string datePattern = "MM/dd/yyyy";
            Console.Write("First name: ");
            newRecord.FirstName = Console.ReadLine();
            Console.Write("Last name: ");
            newRecord.LastName = Console.ReadLine();
            Console.Write("Date of birth: ");
            var parsed = DateTime.TryParseExact(Console.ReadLine(), datePattern, null, 0, out DateTime dateOfBirth);
            while (!parsed)
            {
                Console.WriteLine("Invalid date type, please, use MM/DD/YYYY pattern");
                Console.Write("Date of birth: ");
                parsed = DateTime.TryParseExact(Console.ReadLine(), datePattern, null, 0, out dateOfBirth);
            }

            newRecord.DateOfBirth = dateOfBirth;

            Console.Write("Work experience: ");
            parsed = short.TryParse(Console.ReadLine(), out short workExperience);
            while (!parsed)
            {
                Console.WriteLine("Invalid work experience input type, try short type");
                Console.Write("Work experience: ");
                parsed = short.TryParse(Console.ReadLine(), out workExperience);
            }

            newRecord.WorkExperience = workExperience;

            Console.Write("Weight: ");
            parsed = decimal.TryParse(Console.ReadLine(), out decimal weight);
            while (!parsed)
            {
                Console.WriteLine("Invalid weight input type, try decimal type");
                Console.Write("Weight: ");
                parsed = decimal.TryParse(Console.ReadLine(), out weight);
            }

            newRecord.Weight = weight;

            Console.Write("Lucky symbol: ");
            parsed = char.TryParse(Console.ReadLine(), out char luckySymbol);
            while (!parsed)
            {
                Console.WriteLine("Invalid lucky symbol input, try to write one symbol");
                Console.Write("Lucky symbol: ");
                parsed = char.TryParse(Console.ReadLine(), out luckySymbol);
            }

            newRecord.LuckySymbol = luckySymbol;

            try
            {
                int id = fileCabinetService.CreateRecord(newRecord);
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

            var records = fileCabinetService.GetRecords();
            bool isRecordExist = false;
            for (int i = 0; i < records.Length; i++)
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
                string pattern = "MM/dd/yyyy";
                Console.Write("First name: ");
                updatedRecord.FirstName = Console.ReadLine();
                Console.Write("Last name: ");
                updatedRecord.LastName = Console.ReadLine();
                Console.Write("Date of birth: ");
                var parsed = DateTime.TryParseExact(Console.ReadLine(), pattern, null, 0, out DateTime dateOfBirth);
                while (!parsed)
                {
                    Console.WriteLine("Invalid date type, please, use MM/DD/YYYY pattern");
                    Console.Write("Date of birth: ");
                    parsed = DateTime.TryParseExact(Console.ReadLine(), pattern, null, 0, out dateOfBirth);
                }

                updatedRecord.DateOfBirth = dateOfBirth;

                Console.Write("Work experience: ");
                parsed = short.TryParse(Console.ReadLine(), out short workExperience);
                while (!parsed)
                {
                    Console.WriteLine("Invalid work experience input type, try short type");
                    Console.Write("Work experience: ");
                    parsed = short.TryParse(Console.ReadLine(), out workExperience);
                }

                updatedRecord.WorkExperience = workExperience;

                Console.Write("Weight: ");
                parsed = decimal.TryParse(Console.ReadLine(), out decimal weight);
                while (!parsed)
                {
                    Console.WriteLine("Invalid weight input type, try decimal type");
                    Console.Write("Weight: ");
                    parsed = decimal.TryParse(Console.ReadLine(), out weight);
                }

                updatedRecord.Weight = weight;

                Console.Write("Lucky symbol: ");
                parsed = char.TryParse(Console.ReadLine(), out char luckySymbol);
                while (!parsed)
                {
                    Console.WriteLine("Invalid lucky symbol input, try to write one symbol");
                    Console.Write("Lucky symbol: ");
                    parsed = char.TryParse(Console.ReadLine(), out luckySymbol);
                }

                updatedRecord.LuckySymbol = luckySymbol;

                try
                {
                    fileCabinetService.EditRecord(updatedRecord);
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
                var list = fileCabinetService.FindByFirstName(text);
                if (list is null)
                {
                    Console.WriteLine("There aren't such records");
                }
                else
                {
                    for (int i = 0; i < list.Length; i++)
                    {
                        Console.WriteLine($"#{list[i].Id}, {list[i].FirstName}, {list[i].LastName}, {list[i].DateOfBirth.ToString("yyyy'-'MMM'-'dd", ci)}, Work experience: {list[i].WorkExperience}, Weight: {list[i].Weight}, Lucky symbol: {list[i].LuckySymbol}");
                    }
                }
            }
            else if (property.ToLower().Equals("lastname"))
            {
                var list = fileCabinetService.FindByLastName(text);
                if (list is null)
                {
                    Console.WriteLine("There aren't such records");
                }
                else
                {
                    for (int i = 0; i < list.Length; i++)
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

                var list = fileCabinetService.FindByDateOfBirth(dateOfBirth);
                if (list is null)
                {
                    Console.WriteLine("There aren't such records");
                }
                else
                {
                    for (int i = 0; i < list.Length; i++)
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
            var list = fileCabinetService.GetRecords();
            CultureInfo ci = new ("en-US");
            if (list.Length == 0)
            {
                Console.WriteLine("List of records is empty, please use 'create' command to add record");
            }
            else
            {
                for (int i = 0; i < list.Length; i++)
                {
                    Console.WriteLine($"#{list[i].Id}, {list[i].FirstName}, {list[i].LastName}, {list[i].DateOfBirth.ToString("yyyy'-'MMM'-'dd", ci)}, Work experience: {list[i].WorkExperience}, Weight: {list[i].Weight}, Lucky symbol: {list[i].LuckySymbol}");
                }
            }
        }
    }
}