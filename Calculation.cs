using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculator
{
    internal class Calculation
    {
        public static decimal PerformCalculation(decimal a, decimal b, string operation)
        {
            decimal result = 0;
            switch (operation)
            {
                case "add":
                    result = Add(a, b);
                    break;
                case "subtract":
                    result = Substract(a, b);
                    break;
                case "multiply":
                    result = Multiply(a, b);
                    break;
            }
            return result;
        }

        private static decimal Add(decimal a, decimal b)
        {
            return a + b;
        }

        private static decimal Substract(decimal a, decimal b)
        {
            return a - b;
        }

        private static decimal Multiply(decimal a, decimal b)
        {
            return a * b;
        }

    }
}

