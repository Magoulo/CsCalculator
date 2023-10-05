using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal class Validation
    {
        private static HashSet<string> validOperations = new HashSet<string> { "add", "subtract", "multiply" };

        public static bool IsValidOperation(string operation)
        {
            if (validOperations.Contains(operation))
            {
                return true;
            }
            else {
                Console.WriteLine("The operation you entered is invalid. Please try again.\n");
                return false; 
            }
        }
        public static bool IsNumber(string num)
        {
            if (decimal.TryParse(num, out _))
            {
                return true;
            }
            else {
                return false; 
            }
        }

    }
}
