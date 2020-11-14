using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace Task2
{
    public class RPN
    {
        public static double Calculate(string input, string x)
        {
            try { return double.Parse(GetExpression(input, x));}
            catch (Exception) { return Counting(GetExpression(input, x));}
            
        }

        private static string GetExpression(string input, string x)
        {
            string output = string.Empty;
            string fun = string.Empty;
            Stack<char> operStack = new Stack<char>();
            char k = ' ';
            string p = "";
            for (int i = 0; i < input.Length; i++)
            {
                if (IsOperator(input[i]) || Char.IsDigit(input[i]) || (input[i] == 'x'))
                {
                    if (k == ' ')
                    {
                        k = input[i];
                    }
                    else if (input[i] == '-' && !Char.IsDigit(k))    
                    {
                        p += " 0 ";
                    }
                    
                    k = input[i];
                }

                if (input[i] == 'x')
                {
                    p += x;
                }
                else
                {
                    p += input[i];
                }
                
            }
            input = p;
            for (int i = 0; i < input.Length; i++) 
            {
                if (IsDelimeter(input[i]))
                    continue;
                if (Char.IsDigit(input[i]) || (input[i] == 'x'))
                {
                    
                    while (!IsDelimeter(input[i]) && !IsOperator(input[i]))
                    {
                        if (input[i] == 'x')
                        {
                            output += x;
                            break;
                        }
                        output += input[i];
                        i++;
                        if (i == input.Length) break;
                        
                    }
                    output += " "; 
                    i--; 
                }
                else
                    if (IsOperator(input[i])) 
                    {
                        if (input[i] == '(') 
                            operStack.Push(input[i]); 
                        else if (input[i] == ')')
                        {
                            char s = operStack.Pop();
                            while (s != '(')
                            {
                                output += s.ToString() + ' ';
                                s = operStack.Pop();
                            }
                        }
                        else
                        {
                            if (operStack.Count > 0) 
                                if (GetPriority(input[i]) <= GetPriority(operStack.Peek()))
                                    output += operStack.Pop().ToString() + " "; 

                            operStack.Push(char.Parse(input[i].ToString()));

                        }
                    }
                    else if (input[i] == '\u03C0')
                        output += " \u03C0 ";
                    else if (input[i] == 'e')
                        output += " e ";
                    else
                    {
                        fun = String.Empty;
                        while (input[i] != '(')
                        {
                            fun += input[i];
                            i++;
                            if (i == input.Length) break;
                        }
                        i++;
                        if (NotFunction(fun))
                        {
                            String param = String.Empty;
                            while (input[i] != ')')
                            {
                                param += input[i];
                                i++;
                                if (i == input.Length) break; 
                            }
                            
                            double d;
                            try { d = double.Parse(param); }
                            catch (Exception) { d = Counting(GetExpression(param, x)); }
                            output += doFunc(fun, d);
                        }

                    }
            }
            
            while (operStack.Count > 0)
                output += operStack.Pop() + " ";

                return output;
        }

        private static double Counting(string input)
        {
            double result = 0;
            double b = 0;
            Stack<double> temp = new Stack<double>(); 
            try { return double.Parse(input); }
            catch (Exception)
            {
                for (int i = 0; i < input.Length; i++) 
                {
                    if (Char.IsDigit(input[i]))
                    {
                        string a = string.Empty;

                        while (!IsDelimeter(input[i]) && !IsOperator(input[i])) 
                        {
                            a += input[i];
                            i++;
                            if (i == input.Length) break;
                        }
                        temp.Push(double.Parse(a)); 
                        i--;
                    }
                    else if (input[i] == '\u03C0')
                        temp.Push(Math.PI);
                    else if (input[i] == 'e')
                        temp.Push(Math.E);
                    else if (IsOperator(input[i])) 
                    {
                        double a = temp.Pop();
                        try
                        { b = temp.Pop(); }
                        catch (Exception) { b = 0; }

                        switch (input[i])
                        {
                            //case '!': result = Factorial((int)a); break;
                            //case 'P': result = Factorial((int)b) / Factorial((int)(b - a)); break;
                            //case 'C': result = Factorial((int)b) / (Factorial((int)a) * Factorial((int)(b - a))); break;
                            case '^': result = Math.Pow(b,a); break;
                            //case '%': result = b % a; break;
                            case '+': result = b + a; break;
                            case '-': result = b - a; break;
                            case '*': result = b * a; break;
                            case '/': if (a == 0) throw new DividedByZeroException(); else result = b / a; break;
                            case 'a': result = And((int)a, (int)b); break;
                            case 'o': result = Or((int)a, (int)b); break;
                            case 'x': result = Xor((int)a, (int)b); break;
                        }
                        temp.Push(result);
                    }
                }
                try { return temp.Peek(); }
                catch (Exception) { throw new SyntaxException(); }
                
            }
            
        }
        private static bool IsDelimeter(char c)
        {
            if ((" =".IndexOf(c) != -1))
                return true;
            return false;
        }
        /*private static bool IsOperator(char с)
        {
            if (("+-/*^()PC!%aox".IndexOf(с) != -1))
                return true;
            return false;
        }*/
        private static bool IsOperator(char с)
        {
            if (("+-/*^()aox".IndexOf(с) != -1))
                return true;
            return false;
        }
        /*private static bool IsOneOperandFunction(String s)
        {
            String[] func = { "sin", "cos", "tg", "asin", "acos", "atg", "sqrt", "ln", "lg", "not" };
            if (Array.Exists(func, e => e == s))
                return true;
            return false;
        }*/
        private static bool NotFunction(String s)
        {
            String[] func = { "not" };
            if (Array.Exists(func, e => e == s))
                return true;
            return false;
        }
        
        private static double doFunc(String fun,double param)
        {
            switch (fun)
            {
                /*case "cos": return Math.Cos(param);
                case "sin": return Math.Sin(param);
                case "tg": if (Math.Abs(param % (2 * Math.PI)) == (Math.PI / 2)) throw new TgException(param); 
                    else return Math.Tan(param);
                case "asin": if (param < -1 || param > 1) throw new ArcSinCosException(param); 
                    else return Math.Asin(param);
                case "acos": if (param < -1 || param > 1) throw new ArcSinCosException(param); 
                    else return Math.Acos(param);
                case "atg": return Math.Atan(param);
                case "sqrt": if (param < 0) throw new SqrtException(param); 
                    else return Math.Sqrt(param);
                case "ln": if (param <= 0) throw new LogException(param); 
                    else return Math.Log(param);
                case "lg": if (param <= 0) throw new LogException(param); 
                    else return Math.Log10(param);*/
                case "not": return Not(param);
                    
                default: return 0;
            }
        }
        private static byte GetPriority(char s)
        {
            switch (s)
            {
                case '(': return 0;
                case ')': return 1;
                case '+': return 2;
                case '-': return 3;
                //case '!': return 4;
                //case '%': return 4;
                case '*': return 4;
                case '/': return 4;
                case '^': return 5;
                case 'a': return 5;
                case 'o': return 5;
                case 'x': return 5;
                default: return 4;
            }
        }
        /*private static int Factorial(int x)
        {
            int i = 1;
            for (int s = 1; s <= x; s++)
                i = i * s;
            if (x < 0) throw new NegativeFactorialException(x);
            return i;
        }*/

        private static List<char> IntToBinary(int x)
        {
            List<char> outputList = new List<char>();
            foreach (var elem in Convert.ToString(x, 2))
            {
                outputList.Add(elem);
            }

            return outputList;
        }

        private static int BinaryToInt(List<char> x)
        {
            StringBuilder xStringBuilder = new StringBuilder();
            foreach (var elem in x)
            {
                xStringBuilder.Append(elem);
            }
            
            return Convert.ToInt32(xStringBuilder.ToString(), 2);
        }
        
        private static double Not(double x)
        {
            List<char> xList = IntToBinary((int)x);
            List<char> notXList = new List<char>();
            foreach (var elem in xList)
            {
                notXList.Add(BitNot(elem));
            }

            return BinaryToInt(notXList);
        }
        
        private static char BitNot(char bit)
        {
            if (bit == '1')
            {
                return '0';
            }
            else
            {
                return '1';
            }
        }

        private static double And(int firstValue, int secondValue)
        {
            List<char> firstValueList = IntToBinary(firstValue);
            
            List<char> secondValueList = IntToBinary(secondValue);
            
            while (firstValueList.Count < secondValueList.Count)
            {
                firstValueList.Insert(0, '0');
            }
            
            while (firstValueList.Count > secondValueList.Count)
            {
                secondValueList.Insert(0, '0');
            }
            
            List<char> outputList = new List<char>();
            for (int i = 0; i < firstValueList.Count; i++)
            {
                outputList.Add(BitAnd(firstValueList[i], secondValueList[i]));
            }

            return BinaryToInt(outputList);

        }
        
        private static char BitAnd(char firstBit, char secondBit)
        {
            if ((firstBit == '1') && (secondBit == '1'))
            {
                return '1';
            }
            else
            {
                return '0';
            }
        }
        
        private static double Or(int firstValue, int secondValue)
        {
            List<char> firstValueList = IntToBinary(firstValue);
            
            List<char> secondValueList = IntToBinary(secondValue);
            
            while (firstValueList.Count < secondValueList.Count)
            {
                firstValueList.Insert(0, '0');
            }
            
            while (firstValueList.Count > secondValueList.Count)
            {
                secondValueList.Insert(0, '0');
            }
            
            List<char> outputList = new List<char>();
            for (int i = 0; i < firstValueList.Count; i++)
            {
                outputList.Add(BitOr(firstValueList[i], secondValueList[i]));
            }

            return BinaryToInt(outputList);

        }
        
        private static char BitOr(char firstBit, char secondBit)
        {
            if ((firstBit == '0') && (secondBit == '0'))
            {
                return '0';
            }
            else
            {
                return '1';
            }
        }
        
        private static double Xor(int firstValue, int secondValue)
        {
            List<char> firstValueList = IntToBinary(firstValue);
            
            List<char> secondValueList = IntToBinary(secondValue);
            
            while (firstValueList.Count < secondValueList.Count)
            {
                firstValueList.Insert(0, '0');
            }
            
            while (firstValueList.Count > secondValueList.Count)
            {
                secondValueList.Insert(0, '0');
            }
            
            List<char> outputList = new List<char>();
            for (int i = 0; i < firstValueList.Count; i++)
            {
                outputList.Add(BitXor(firstValueList[i], secondValueList[i]));
            }

            return BinaryToInt(outputList);

        }
        
        private static char BitXor(char firstBit, char secondBit)
        {
            if (((firstBit == '0') && (secondBit == '0')) || ((firstBit == '1') && (secondBit == '1')))
            {
                return '0';
            }
            else
            {
                return '1';
            }
        }
    }
    
    public class MyException : Exception
    {
        public string type;
    }
    /*public class NegativeFactorialException : MyException
    {
        public NegativeFactorialException(int x)
        {
            this.type = "Math error";
            //MessageBox.Show("Factorial(" + x + ") does not exsists", type, MessageBoxButtons.OK);
            Console.WriteLine("Factorial(" + x + ") does not exsists");
        }
    }
    public class TgException : MyException
    {
        public TgException(double x)
        {
            this.type = "Math error";
            //MessageBox.Show("Tg(" + x + ") does not exsists", type, MessageBoxButtons.OK);
            Console.WriteLine("Tg(" + x + ") does not exsists");
        }
    }
    public class SqrtException : MyException
    {
        public SqrtException(double x)
        {
            this.type = "Math error";
            //MessageBox.Show("Sqrt(" + x + ") does not exsists", type, MessageBoxButtons.OK);
            Console.WriteLine("Sqrt(" + x + ") does not exsists");
        }
    }*/
    public class DividedByZeroException : MyException
    {
        public DividedByZeroException()
        {
            this.type = "Math error";
            //MessageBox.Show("Division by zero is impossible", type, MessageBoxButtons.OK);
            Console.WriteLine("Division by zero is impossible");
        }
    }
    /*public class LogException : MyException
    {
        public LogException(double x)
        {
            this.type = "Math error";
            //MessageBox.Show("Log(" + x + ") does not exsists", type, MessageBoxButtons.OK);
            Console.WriteLine("Log(" + x + ") does not exsists");
        }
    }*/
    public class SyntaxException : MyException
    {
        public SyntaxException()
        {
            this.type = "Syntax error";
            //MessageBox.Show("You made a mistake", type, MessageBoxButtons.OK);
            Console.WriteLine("You made a mistake");
        }
    }
    /*public class ArcSinCosException : MyException
    {
        public ArcSinCosException(double x)
        {
            this.type = "Math error";
            //MessageBox.Show("Acos(or Asin) (" + x + ") does not exsists", type, MessageBoxButtons.OK);
            Console.WriteLine("Acos(or Asin) (" + x + ") does not exsists");
        }
    }*/
}