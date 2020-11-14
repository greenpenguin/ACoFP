using System;
using System.Collections;

namespace Task2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            int p = 2;
            int k = 2;
            string func = "x+not(x)";
            MathWork mathWork = new MathWork();
            mathWork.GetCoordinates(p, k, func);
            for (int i = 0; i < Math.Pow(2, k); i++)
            {
                mathWork.CreateGraph();
            }
            
        }
    }
}