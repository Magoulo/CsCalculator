using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal class RegisterIO
    {
        private static Dictionary<string, List<Instruction>> registerInstructions = new Dictionary<string, List<Instruction>>();

        public static void AddRegisterInstruction(string registerName, string operation, string value)
        {
            if (!registerInstructions.ContainsKey(registerName))  
            {
                registerInstructions[registerName] = new List<Instruction>();
            }
            registerInstructions[registerName].Add(new Instruction { Operation = operation, Value = value });
        }

        public static decimal PerformRegisterInstructions(string registerName)
        {
            if (!registerInstructions.ContainsKey(registerName)) // If no instructions return the current value of the register
            {
                return ReadRegisterValue(registerName);
            }
            decimal value = ReadRegisterValue(registerName);
            foreach (var instruction in registerInstructions[registerName])
            {
                decimal operandValue;

                if (Validation.IsNumber(instruction.Value))
                {
                    operandValue = decimal.Parse(instruction.Value);
                }
                else
                {
                    operandValue = PerformRegisterInstructions(instruction.Value); // Recursivley returnar värdet från det register angetts som värde
                }

                value = Calculation.PerformCalculation(value, operandValue, instruction.Operation); // sparar det nya värdet av value i value variabeln
            }
            registerInstructions[registerName].Clear();
            WriteToRegister(registerName, value); 

            return value;
        }
   
        public static bool DoesRegisterExist(string registerName)
        {
            return File.Exists(RegisterPath(registerName));
        }

        public static decimal ReadRegisterValue(string registerName)
        {
            if (DoesRegisterExist(registerName))
            {
                string registerContent = File.ReadAllText(RegisterPath(registerName));
                decimal registerValue = Convert.ToDecimal(registerContent);

                return registerValue;
            }
            else { return 0; }
        }

        public static void WriteToRegister(string registerName, decimal sum)
        {
            File.WriteAllText(RegisterPath(registerName), sum.ToString());
        }

        public static void CreateRegister(string registerName)
        {
            File.WriteAllText(RegisterPath(registerName), "0");
        }

        private static string RegisterPath(string registerName)
        {
            string RegistersFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Registers");
            string registerPath = Path.Combine(RegistersFolderPath, registerName);
            return registerPath + ".txt";
        }
    }
}