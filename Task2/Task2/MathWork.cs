using System;
using System.Collections.Generic;

namespace Task2
{
    public class MathWork
    {
        List<double> xCoordinates = new List<double>();
        List<double> yCoordinates = new List<double>();
        
        public void GetCoordinates(int p, int k, string func)
        {
            for (int i = 0; i <= (Math.Pow(p, k) - 1); i++)
            {
                double x = (i % Math.Pow(2, k)) / Math.Pow(2, k);
                double y = (RPN.Calculate(func, i.ToString()) % Math.Pow(2, k)) / Math.Pow(2, k);
                
                xCoordinates.Add(x);
                yCoordinates.Add(y);
            }
        }

        public void CreateGraph()
        {
            Dictionary<double, double> coordinates = new Dictionary<double, double>();
            coordinates.Add(xCoordinates[0], yCoordinates[0]);
            Console.WriteLine(xCoordinates[0] + " " + yCoordinates[0]);
            //построение графика
            xCoordinates.RemoveAt(0);
            yCoordinates.RemoveAt(0);
        }
        
        /*public void Output()
        {
            foreach (var elem in xCoordinates)
            {
                Console.Write(elem + " ");
            }
            Console.WriteLine();
            foreach (var elem in yCoordinates)
            {
                Console.Write(elem + " ");
            }
        }*/
    }
}