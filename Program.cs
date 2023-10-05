using Calculator;
using System;

namespace Calculator
{
    public class Program
    {
        private const string WELCOME_MESSAGE = "Welcome to my Calculator app!";
        private const string SYNTAX_MESSAGE = "<register> <operation> <value>";
        private const string PRINT_REGISTER_MESSAGE = "print <register>";
        private const string QUIT_MESSAGE = "quit";
        private const string OPERATIONS_MESSAGE = "<add> <substract> <multiply>";
        private const string BAD_INPUT_MESSAGE = "--- BAD INPUT! ---";
        private const string INSTRUCTION_MESSAGE = "You must use the syntax given in the description of the calculator. Please try again.";
        private static string? filePath;
        private static StreamReader fileReader = null;
        public static void Main(string[] args)
        {
            DisplayWelcomeMessage();      
            MainLoop();
        }
        private static void DisplayWelcomeMessage()
        {
            Console.WriteLine($"{WELCOME_MESSAGE}\n");
            Console.WriteLine($"The syntax of the app is listed below\n-----------------------------\n");
            Console.WriteLine($"{SYNTAX_MESSAGE}\n{PRINT_REGISTER_MESSAGE}\n{QUIT_MESSAGE}\n");
            Console.WriteLine($"-----------------------------\n");
            Console.WriteLine($"Choose one of the following operators for your calculations:\n{OPERATIONS_MESSAGE}\n-----------------------------\n");
        }
        private static void MainLoop()
        {
            bool quit = false;
            IsFileInput();
           
            while (!quit)
            {

                string userInput = GetInput();
                Console.WriteLine();

                string[] userInputs = userInput.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                switch (userInputs.Length)
                {
                    case 1:
                        HandleSingleWordInput(userInputs[0], ref quit);
                        break;
                    case 2:
                        HandleTwoWordInput(userInputs);
                        break;
                    case 3:
                        HandleThreeWordInput(userInputs);
                        break;
                    default:
                        Console.WriteLine($"{BAD_INPUT_MESSAGE}\n{INSTRUCTION_MESSAGE}\n");
                        break;
                }
            }
        }
        private static void IsFileInput()
        {
            Console.WriteLine("Do you want to use Input from a text file? (Y/N) \n");
            string answer = Console.ReadLine().ToLower();
            if (answer == "y")
            {
                InputFilePath();
            }
            else {
                Console.WriteLine("\nPlease typ your desired operations in the console.\n");
            }
        }
        private static void InputFilePath()
        {
            Console.WriteLine("Please enter the path to your text file \n");
            filePath = Console.ReadLine().ToLower();
            Console.WriteLine("");
        }
        private static void HandleSingleWordInput(string input, ref bool quit)
        {
            if (input == "quit")
            { quit = true; }
            else
            { Console.WriteLine($"{BAD_INPUT_MESSAGE}\n{INSTRUCTION_MESSAGE}\n"); }
        }
        private static string GetInput() // Borde kanske ha hetat GetInputType?
        {
            if (filePath != null && fileReader == null)
            {
                if (File.Exists(filePath))
                {
                    fileReader = new StreamReader(filePath);
                }
                else
                {
                    Console.WriteLine("File not found! Switching to console input.");
                    filePath = null;
                }
            }

            if (fileReader != null)
            {
                if (!fileReader.EndOfStream) // fileReader har en internal pointer som pekar på vart i dokumentet readern är atm
                {
                    var input = fileReader.ReadLine().ToLower();
                    Console.WriteLine(input);
                    return input;
                }
                else
                {
                    fileReader.Close();
                    fileReader = null;
                    Console.WriteLine("Finished processing file. Exiting program");
                    return "quit";
                }
            }

            return Console.ReadLine().ToLower();
        }
        private static void HandleTwoWordInput(string[] inputs)
        {
            if (inputs[0] == "print" && HandleRegisterExistence(inputs[1]))
            {
                decimal registerValue = RegisterIO.PerformRegisterInstructions(inputs[1]);
                Console.WriteLine($"Current sum of the register \"{inputs[1]}\" is: {registerValue}\n");
            }
            else
            {
                Console.WriteLine($"{BAD_INPUT_MESSAGE}\n{INSTRUCTION_MESSAGE}\n");
            }
        }
        private static void HandleThreeWordInput(string[] inputs)
        {
            if (HandleRegisterExistence(inputs[0]) && Validation.IsValidOperation(inputs[1]))
            {
                decimal numA = RegisterIO.ReadRegisterValue(inputs[0]);

                if (Validation.IsNumber(inputs[2]))
                {
                    decimal numB = decimal.Parse(inputs[2]);
                    decimal result = Calculation.PerformCalculation(numA, numB, inputs[1]);
                    RegisterIO.WriteToRegister(inputs[0], result);
                }
                else if (HandleRegisterExistence(inputs[2]))
                {
                    RegisterIO.AddRegisterInstruction(inputs[0], inputs[1], inputs[2]);
                }
            }
            else
            {
                Console.WriteLine($"{BAD_INPUT_MESSAGE}\n{INSTRUCTION_MESSAGE}\n");
            }
        }
        private static bool HandleRegisterExistence(string registerName)
        {
            if (RegisterIO.DoesRegisterExist(registerName))
            {
                return true;
            }
            else
            {
                Console.WriteLine($"The Register name you entered does not seem to exist. Do you want to create a new register with the name {registerName}? (Y/N) \n");
                string answer = Console.ReadLine().ToLower();

                if (answer == "y")
                {
                    RegisterIO.CreateRegister(registerName);
                    Console.WriteLine("New register named \"" + registerName + "\" created.\n");
                    return true;
                }
                else
                {
                    Console.WriteLine("\nAdd new input into the calculator.\n");
                    return false;
                }
            }
        }
    }
}