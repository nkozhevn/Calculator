using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// Пример выражения: 1 + a2x * (20 - b)

namespace Calculator
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Введите выражение (например, (12 + x) * 23 + y): ");
            string expression = Console.ReadLine();
            if (Check(expression) == false)
            {
                Console.WriteLine("Выражение введено неверно!");
            }
            else
            {
                AST(expression);
                Console.WriteLine("{0} = {1}", expression, Solve(expression));
            }
            Console.ReadLine();
        }

        static bool Check(string result) // Проверка правильности введенного выражения
        {
            result = result.Replace(" ", "");
            int count1 = 0;
            int count2 = 0;
            for (int i = 0; i < result.Length; i++)
            {
                if (result[i] == '(')
                {
                    count1++;
                }
                else if (result[i] == ')')
                {
                    count2++;
                }
                if (count2 > count1)
                {
                    return false;
                }
            }
            if (count2 != count1)
            {
                return false;
            }
            result = result.Replace("(", "");
            result = result.Replace(")", "");
            if ((!char.IsDigit(result[0]) && !char.IsLetter(result[0]) && result[0] != '-') || (!char.IsDigit(result[result.Length - 1]) && !char.IsLetter(result[result.Length - 1])))
            {
                return false;
            }
            for (int i = 0; i < result.Length; i++)
            {
                if (!char.IsDigit(result[i]) && !char.IsLetter(result[i]) && result[i] != '*' && result[i] != '+' && result[i] != '-' && result[i] != '/')
                {
                    return false;
                }
            }
            return true;
        }

        static void AST(string result) // Построение AST-дерева
        {
            var parts = result.Split(' '); // Разбиение выражения на элементы, разделенные пробелом
            var elements = new List<string>();
            for (int i = 0; i < parts.Length; i++)
            {
                elements.Add(parts[i]);
            }
            Console.Write("\n");
            Console.WriteLine("AST-дерево:");
            Console.Write("\n");
            Console.Write(elements[0]);
            for (int i = 2; i < elements.Count; i += 2)
            {
                Console.Write("   ");
                Console.Write(elements[i]);
            }
            Console.Write("\n");
            for (int i = 1; i < elements.Count; i += 2)
            {
                Console.Write("  ");
                if (elements[i + 1][0] != '(' && elements[i - 1][elements[i - 1].Length - 1] != ')')
                {
                    Console.Write(" | ");
                }
                else
                {
                    Console.Write("   ");
                }
            }
            Console.Write("\n");
            for (int i = 1; i < elements.Count; i += 2)
            {
                Console.Write("  ");
                if (elements[i + 1][0] != '(' && elements[i - 1][elements[i - 1].Length - 1] != ')')
                {
                    Console.Write(elements[i]);
                }
                else
                {
                    Console.Write("   ");
                }
            }
            Console.Write("\n");
            for (int i = 1; i < elements.Count; i += 2)
            {
                Console.Write("  ");
                if (elements[i + 1][0] != '(' && elements[i - 1][elements[i - 1].Length - 1] != ')')
                {
                    Console.Write("   ");
                }
                else
                {
                    Console.Write(" | ");
                }
            }
            Console.Write("\n");
            for (int i = 1; i < elements.Count; i += 2)
            {
                Console.Write("  ");
                if (elements[i + 1][0] != '(' && elements[i - 1][elements[i - 1].Length - 1] != ')')
                {
                    Console.Write("   ");
                }
                else
                {
                    Console.Write(elements[i]);
                }
            }
            Console.Write("\n");
            Console.Write("\n");
        }

        static string Solve(string result)
        {
            for (int i = 0; i < result.Length; i++) // Поиск идентификаторов переменных
            {
                if (char.IsLetter(result[i]))
                {
                    string oper = string.Empty;
                    oper += result[i];

                    for (int j = i + 1; j < result.Length; j++)
                    {
                        if (!char.IsLetter(result[j]) && !char.IsDigit(result[j]))
                        {
                            break;
                        }
                        oper += result[j];
                    }

                    bool param = false;
                    string x = string.Empty;
                    while (param == false)
                    {
                        Console.Write("{0} = ", oper);
                        x = Console.ReadLine();
                        for (int j = 0; j < x.Length; j++)
                        {
                            if (!char.IsDigit(x[j]))
                            {
                                Console.WriteLine("Введите число!");
                                break;
                            }
                            else
                            {
                                param = true;
                            }
                        }
                    }

                    result = result.Replace(oper, x);
                }
            }

            result = Brackets(result, 0);
            result = Calculate(result);

            return result;
        }

        static string Brackets(string result, int index) // Поиск скобок и вызов метода для решения выражений
        {
            for (int i = index; i < result.Length; i++)
            {
                if (result[i] == '(')
                {
                    index = i + 1;
                    result = Brackets(result, index);
                    string temp = string.Empty;
                    string tempresult = string.Empty;
                    for (int j = index; j < result.Length; j++)
                    {
                        if (result[j] == ')')
                        {
                            tempresult = temp;
                            break;
                        }
                        temp += result[j];
                    }
                    tempresult = Calculate(tempresult);
                    temp = '(' + temp + ')';
                    result = result.Replace(temp, tempresult);
                }
            }

            return result;
        }

        static string Calculate(string result) // Вычисление выражения
        {
            var parts = result.Split(' '); // Разбиение выражения на элементы, разделенные пробелом
            var elements = new List<string>();
            for (int i = 0; i < parts.Length; i++)
            {
                elements.Add(parts[i]);
            }   

            for (int i = 0; i < elements.Count; i++) 
            {
                if (elements[i] == "*") // Операции умножения
                {
                    string string1 = elements[i - 1];
                    string string2 = elements[i + 1];
                    int currentresult = int.Parse(string1) * int.Parse(string2);
                    elements[i] = currentresult.ToString();
                    elements.RemoveAt(i + 1);
                    elements.RemoveAt(i - 1);
                    result = result.Replace(string1 + " * " + string2, currentresult.ToString());
                }
                else if (elements[i] == "/") // Операции деления
                {
                    string string1 = elements[i - 1];
                    string string2 = elements[i + 1];
                    int currentresult = int.Parse(string1) / int.Parse(string2);
                    elements[i] = currentresult.ToString();
                    elements.RemoveAt(i + 1);
                    elements.RemoveAt(i - 1);
                    result = result.Replace(string1 + " / " + string2, currentresult.ToString());
                }
            }
            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i] == "+") // Операции сложения
                {
                    string string1 = elements[i - 1];
                    string string2 = elements[i + 1];
                    int currentresult = int.Parse(string1) + int.Parse(string2);
                    elements[i] = currentresult.ToString();
                    elements.RemoveAt(i + 1);
                    elements.RemoveAt(i - 1);
                    result = result.Replace(string1 + " + " + string2, currentresult.ToString());
                }
                else  if (elements[i] == "-") // Операции вычитания
                {
                    string string1 = elements[i - 1];
                    string string2 = elements[i + 1];
                    int currentresult = int.Parse(string1) - int.Parse(string2);
                    elements[i] = currentresult.ToString();
                    elements.RemoveAt(i + 1);
                    elements.RemoveAt(i - 1);
                    result = result.Replace(string1 + " - " + string2, currentresult.ToString());
                }
            }

            return result;
        }
    }
}