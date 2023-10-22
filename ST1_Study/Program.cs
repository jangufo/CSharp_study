namespace ST1_Study;

using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
    }

    #region static void Fun1() 控制台输入反转
    static void Fun1()
    {
        string line = Console.ReadLine();
        List<int> numbers = new List<int>();

        if (!string.IsNullOrEmpty(line))
        {
            string[] numberStrings = line.Split(' ');
            foreach (string numString in numberStrings)
            {
                if (int.TryParse(numString, out int number))
                {
                    numbers.Add(number);
                }
            }
            numbers.Reverse(); // 反转整数列表
        }

        foreach (int number in numbers)
        {
            Console.Write(number + " ");
        }
    }
    #endregion
}
