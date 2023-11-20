using System;
using System.Collections.Generic;

class Calculator
{
    static void Main()
    {
        Console.WriteLine("Введите выражение (без пробелов):");
        string input = Console.ReadLine();

        double result = EvaluateExpression(input);
        Console.WriteLine("Результат: " + result);
    }

    static double EvaluateExpression(string input)
    {
        Stack<double> operandStack = new Stack<double>();
        Stack<char> operatorStack = new Stack<char>();

        Dictionary<char, Func<double, double, double>> operations = new Dictionary<char, Func<double, double, double>>
        {
            { '+', (a, b) => a + b },
            { '-', (a, b) => a - b },
            { '*', (a, b) => a * b },
            { '/', (a, b) => a / b }
        };

        foreach (char token in input)
        {
            if (char.IsDigit(token))
            {
                operandStack.Push(double.Parse(token.ToString()));
            }
            else if (IsOperator(token))
            {
                while (operatorStack.Count > 0 && Precedence(operatorStack.Peek()) >= Precedence(token))
                {
                    double operand2 = operandStack.Pop();
                    double operand1 = operandStack.Pop();
                    char op = operatorStack.Pop();
                    operandStack.Push(operations[op](operand1, operand2));
                }

                operatorStack.Push(token);
            }
        }

        while (operatorStack.Count > 0)
        {
            double operand2 = operandStack.Pop();
            double operand1 = operandStack.Pop();
            char op = operatorStack.Pop();
            operandStack.Push(operations[op](operand1, operand2));
        }

        return operandStack.Pop();
    }

    static bool IsOperator(char c)
    {
        return c == '+' || c == '-' || c == '*' || c == '/';
    }


    //Приоритет ну типо умножение же первее делается ну и так же с делением. Вот такие пироги
    static int Precedence(char op)
    {
        return new Dictionary<char, int> { { '+', 1 }, { '-', 1 }, { '*', 2 }, { '/', 2 } }[op];
    }
}