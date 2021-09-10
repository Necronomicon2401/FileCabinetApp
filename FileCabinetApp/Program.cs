using System;
using System.Globalization;

namespace FileCabinetApp
{
    public static class Program
    {
        private const string DeveloperName = "Dmitry Bolbat";
        private const string HintMessage = "Enter your command, or enter 'help' to get help.";
        private const int CommandHelpIndex = 0;
        private const int DescriptionHelpIndex = 1;
        private const int ExplanationHelpIndex = 2;

        private static FileCabinetService fileCabinetService = new ();

        private static bool isRunning = true;

        private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
        {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("exit", Exit),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("find", Find),
        };

        private static string[][] helpMessages = new string[][]
        {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
            new string[] { "stat", "shows the number of records that the service stores", "The 'stat' command shows the number of records that the service stores." },
            new string[] { "create", "create new record", "The 'create' command create new record." },
            new string[] { "list", "shows list of records", "The 'list' command shows list of records." },
            new string[] { "edit", "edit created record by id", "The 'edit' command allow to edit created record by id." },
            new string[] { "find", "find and shows created records by inputed property", "The 'find' command find and shows created records by inputed property." },
        };

        public static void Main(string[] args)
        {
            Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
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

                var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    const int parametersIndex = 1;
                    var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                    commands[index].Item2(parameters);
                }
                else
                {
                    PrintMissedCommandInfo(command);
                }
            }
            while (isRunning);
        }

        private static void PrintMissedCommandInfo(string command)
        {
            Console.WriteLine($"There is no '{command}' command.");
            Console.WriteLine();
        }

        private static void PrintHelp(string parameters)
        {
            if (!string.IsNullOrEmpty(parameters))
            {
                var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
                if (index >= 0)
                {
                    Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
                }
                else
                {
                    Console.WriteLine($"There is no explanation for '{parameters}' command.");
                }
            }
            else
            {
                Console.WriteLine("Available commands:");

                foreach (var helpMessage in helpMessages)
                {
                    Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
                }
            }

            Console.WriteLine();
        }

        private static void Exit(string parameters)
        {
            Console.WriteLine("Exiting an application...");
            isRunning = false;
        }

        private static void Stat(string parameters)
        {
            var recordsCount = Program.fileCabinetService.GetStat();
            Console.WriteLine($"{recordsCount} record(s).");
        }

        private static void Create(string parameters)
        {
            string pattern = "MM/dd/yyyy";
            Console.Write("First name: ");
            string firstName = Console.ReadLine();
            Console.Write("Last name: ");
            string lastName = Console.ReadLine();
            Console.Write("Date of birth: ");
            var parsed = DateTime.TryParseExact(Console.ReadLine(), pattern, null, 0, out DateTime dateOfBirth);
            while (!parsed)
            {
                Console.WriteLine("Invalid date type, please, use MM/DD/YYYY pattern");
                Console.Write("Date of birth: ");
                parsed = DateTime.TryParseExact(Console.ReadLine(), pattern, null, 0, out dateOfBirth);
            }

            Console.Write("Work experience: ");
            parsed = short.TryParse(Console.ReadLine(), out short workExperience);
            while (!parsed)
            {
                Console.WriteLine("Invalid work experience input type, try short type");
                Console.Write("Work experience: ");
                parsed = short.TryParse(Console.ReadLine(), out workExperience);
            }

            Console.Write("Weight: ");
            parsed = decimal.TryParse(Console.ReadLine(), out decimal weight);
            while (!parsed)
            {
                Console.WriteLine("Invalid weight input type, try decimal type");
                Console.Write("Weight: ");
                parsed = decimal.TryParse(Console.ReadLine(), out weight);
            }

            Console.Write("Lucky symbol: ");
            parsed = char.TryParse(Console.ReadLine(), out char luckySymbol);
            while (!parsed)
            {
                Console.WriteLine("Invalid lucky symbol input, try to write one symbol");
                Console.Write("Lucky symbol: ");
                parsed = char.TryParse(Console.ReadLine(), out luckySymbol);
            }

            try
            {
                int id = fileCabinetService.CreateRecord(firstName, lastName, dateOfBirth, workExperience, weight, luckySymbol);
                Console.WriteLine($"Record #{id} is created");
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine(ex.Message);
                Create(parameters);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Create(parameters);
            }
        }

        private static void Edit(string parameters)
        {
            var parsedId = int.TryParse(parameters, out int id);
            if (!parsedId)
            {
                Console.WriteLine("Record id must be a number");
                return;
            }

            var records = fileCabinetService.GetRecords();
            for (int i = 0; i < records.Length; i++)
            {
                if (records[i].Id == id)
                {
                    string pattern = "MM/dd/yyyy";
                    Console.Write("First name: ");
                    string firstName = Console.ReadLine();
                    Console.Write("Last name: ");
                    string lastName = Console.ReadLine();
                    Console.Write("Date of birth: ");
                    var parsed = DateTime.TryParseExact(Console.ReadLine(), pattern, null, 0, out DateTime dateOfBirth);
                    while (!parsed)
                    {
                        Console.WriteLine("Invalid date type, please, use MM/DD/YYYY pattern");
                        Console.Write("Date of birth: ");
                        parsed = DateTime.TryParseExact(Console.ReadLine(), pattern, null, 0, out dateOfBirth);
                    }

                    Console.Write("Work experience: ");
                    parsed = short.TryParse(Console.ReadLine(), out short workExperience);
                    while (!parsed)
                    {
                        Console.WriteLine("Invalid work experience input type, try short type");
                        Console.Write("Work experience: ");
                        parsed = short.TryParse(Console.ReadLine(), out workExperience);
                    }

                    Console.Write("Weight: ");
                    parsed = decimal.TryParse(Console.ReadLine(), out decimal weight);
                    while (!parsed)
                    {
                        Console.WriteLine("Invalid weight input type, try decimal type");
                        Console.Write("Weight: ");
                        parsed = decimal.TryParse(Console.ReadLine(), out weight);
                    }

                    Console.Write("Lucky symbol: ");
                    parsed = char.TryParse(Console.ReadLine(), out char luckySymbol);
                    while (!parsed)
                    {
                        Console.WriteLine("Invalid lucky symbol input, try to write one symbol");
                        Console.Write("Lucky symbol: ");
                        parsed = char.TryParse(Console.ReadLine(), out luckySymbol);
                    }

                    try
                    {
                        fileCabinetService.EditRecord(id, firstName, lastName, dateOfBirth, workExperience, weight, luckySymbol);
                        Console.WriteLine($"Record #{id} is updated");
                        return;
                    }
                    catch (ArgumentNullException ex)
                    {
                        Console.WriteLine(ex.Message);
                        Create(parameters);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        Create(parameters);
                    }
                }
            }

            Console.WriteLine($"#{id} record is not found");
        }

        private static void Find(string parameters)
        {
            CultureInfo ci = new CultureInfo("en-US");
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
                for (int i = 0; i < list.Length; i++)
                {
                    Console.WriteLine($"#{i + 1}, {list[i].FirstName}, {list[i].LastName}, {list[i].DateOfBirth.ToString("yyyy'-'MMM'-'dd", ci)}, Work experience: {list[i].WorkExperience}, Weight: {list[i].Weight}, Lucky symbol: {list[i].LuckySymbol}");
                }
            }

            if (property.ToLower().Equals("lastname"))
            {
                var list = fileCabinetService.FindByLastName(text);
                for (int i = 0; i < list.Length; i++)
                {
                    Console.WriteLine($"#{i + 1}, {list[i].FirstName}, {list[i].LastName}, {list[i].DateOfBirth.ToString("yyyy'-'MMM'-'dd", ci)}, Work experience: {list[i].WorkExperience}, Weight: {list[i].Weight}, Lucky symbol: {list[i].LuckySymbol}");
                }
            }
        }

        private static void List(string parameters)
        {
            var list = fileCabinetService.GetRecords();
            CultureInfo ci = new CultureInfo("en-US");
            if (list.Length == 0)
            {
                Console.WriteLine("List of records is empty, please use 'create' command to add record");
            }
            else
            {
                for (int i = 0; i < list.Length; i++)
                {
                    Console.WriteLine($"#{i + 1}, {list[i].FirstName}, {list[i].LastName}, {list[i].DateOfBirth.ToString("yyyy'-'MMM'-'dd", ci)}, Work experience: {list[i].WorkExperience}, Weight: {list[i].Weight}, Lucky symbol: {list[i].LuckySymbol}");
                }
            }
        }
    }
}