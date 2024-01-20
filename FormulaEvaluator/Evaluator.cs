/// <summary>
/// Author: Bingkun Han
/// Partner: None
/// Date: 17th-Jan-2024
/// Course: CS3500 Software Practice, 2024 Spring
/// Copyright: CS 3500 and Bingkun Han - This work may not
///            be copied for use in Academic Coursework.
///
/// I, Bingkun Han, certify that I wrote this code from scratch and
/// did not copy it in part or whole from another source.  All
/// references used in the completion of the assignments are cited
/// in my README file.
///
/// File Contents
/// This is a formula evaoluator library. It has a core method - Evaluate. It will
/// pass a string formula and a variable lookup method to calculate and retun the 
/// result of this formula. It has a delegate decleration and two helper methods. 
/// Help methods simplify the formula calculation processes.
/// </summary>
using System.Text.RegularExpressions;
namespace FormulaEvaluator
{
    public static class Evaluator
    {
        /// <summary>
        /// this is the matchPattern used to check if the variables fit the format rule
        /// </summary>
        private static string matchPattern = @"^[a-zA-Z]+[0-9]+$";


        /// <summary>
        /// This is the delegate of the variable lookup method. This method will
        /// take a string in and retur a int, which convert a variable to a value
        /// like convert from "a1" to 7
        /// </summary>
        /// <param name="variable_name">The string if the variable like "a1"</param>
        /// <returns> The value of that variable</returns>
        public delegate int Lookup(String variable_name);

        /// <summary>
        /// This method will pass a string of formula like "9*9" or "(3-0)*6/2" and a function to
        /// change a string(variable) to a sepecific integer values. It will used when there is 
        /// string variable exist in the formula. This method will calculate the string formula
        /// and return a integer type calculate result
        /// </summary>
        /// <param name="expression">the string of the formula, used to calculate like "8+9"</param>
        /// <param name="variableEvaluator">the function to convert string varible into a integer</param>
        /// <returns> int result, the result of the expression</returns>
        /// <exception cref="ArgumentException">
        /// 1. when find a ")" but cannot find a "("
        /// 2. when the evaluator cannot find a integer to replace variable by using variableEvaluator
        /// 3. when the end format is wrong (the stack situation when after going over all tokens in expression)
        ///     which also showed the expression has wrong format such as "1++", "1()3".
        /// </exception>
        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");

            // I learnt how to use generic stack class from microsoft learning webpage
            Stack<String> values = new Stack<String>();
            Stack<String> operators = new Stack<String>();
            foreach (String token in substrings)
            {
                if (int.TryParse(token, out int integer))
                {
                    if (operators.Count > 0)
                    {
                        DivideMultipleHelper(values, operators, integer);
                    }
                    else
                    {
                        values.Push(token);
                    }
                }
                else if (token == "+" || token == "-")
                {
                    if (operators.Count > 0)
                    {
                        AddMinusHelper(values, operators);
                    }
                    operators.Push(token);

                }
                else if (token == "*" || token == "/" || token == "(")
                {
                    operators.Push(token);
                }
                else if (token == ")")
                {
                    AddMinusHelper(values, operators);
                    if (operators.Peek() == "(")
                    {
                        operators.Pop();
                        if (values.Count > 1)
                        {
                            DivideMultipleHelper(values, operators, int.Parse(values.Pop()));
                        }
                    }
                    else
                    {
                        throw new ArgumentException("missing a (");
                    }
                }
                else
                {
                    if (token != "")
                    {
                        try
                        {
                            //I learn this from microsoft learning
                            if(!Regex.IsMatch(token, matchPattern))
                            {
                                throw new ArgumentException($"{token} does not match pattern");
                            }
                            int lookedValue = variableEvaluator(token);
                            if (operators.Count > 0)
                            {
                                DivideMultipleHelper(values, operators, lookedValue);
                            }
                            else
                            {
                                values.Push(lookedValue.ToString());
                            }
                        }
                        catch
                        {
                            throw new ArgumentException("Unknown Variable exist: " + token);
                        }
                    }
                }
            }
            if (values.Count == 1 && operators.Count == 0)
            {
                int finalResult = int.Parse(values.Pop());
                return finalResult;
            }
            else if (values.Count == 2 && operators.Count == 1)
            {
                if (operators.Peek() == "+")
                {
                    int value1 = int.Parse(values.Pop());
                    int value2 = int.Parse(values.Pop());
                    values.Push((value1 + value2).ToString());
                    operators.Pop();
                }
                else if (operators.Peek() == "-")
                {
                    int value1 = int.Parse(values.Pop());
                    int value2 = int.Parse(values.Pop());
                    values.Push((value2 - value1).ToString());
                    operators.Pop();
                }
                int finalResult = int.Parse(values.Pop());
                return finalResult;
            }
            else
            {
                throw new ArgumentException("the this formula has wrong format");
            }
        }

        /// <summary>
        /// This is method will do divide or multiply operation by using target value, values stack poped value and 
        /// operators poped operators. If operators stack's poped operator is "/", it will do valuesPopedValue/pass-
        /// edValue, or if it is * it will multiply two of them. IF peek operator is neither / or * it will do nothing.
        /// then it will push the new result value in the values stack
        /// </summary>
        /// <param name="values">The values stack used to poped value to calculate</param>
        /// <param name="operators">The operator stack used to choose the operation </param>
        /// <param name="passedValue">The value will be use in opration</param>
        /// <exception cref="ArgumentException"> when value stack has no enough values in it or when the formula
        /// divide by 0</exception>
        private static void DivideMultipleHelper(Stack<string> values, Stack<string> operators, int passedValue)
        {
            if (operators.Peek() == "*")
            {
                try
                {
                    int result = passedValue * int.Parse(values.Pop());
                    operators.Pop();
                    values.Push(result.ToString());
                }
                catch
                {
                    throw new ArgumentException("The value stack is empty");
                }
            }
            else if (operators.Peek() == "/")
            {
                try
                {
                    int result = int.Parse(values.Pop()) / passedValue;
                    operators.Pop();
                    values.Push(result.ToString());
                }
                catch
                {
                    throw new ArgumentException("The value stack is empty or divided by 0 happened");
                }
            }
            else
            {
                values.Push(passedValue.ToString());
            }
        }

        /// <summary>
        /// This method will do add and minus operation if the top of the oprator stack is + or -. If the top is either + or -
        /// It will pop two values from values stack and sum them or substract them depending on the top's command + or -. Then 
        /// It will push the result value in values stack. If the top of operators is not + or 
        /// </summary>
        /// <param name="values">the values stack to pop value to calculate</param>
        /// <param name="operators">the operators stack to pop operator and do operations</param>
        /// <exception cref="ArgumentException">When value stack has less than 2 values which do not support the operation</exception>
        private static void AddMinusHelper(Stack<string> values, Stack<string> operators)
        {
            if (operators.Peek() == "+")
            {
                try
                {
                    int value1 = int.Parse(values.Pop());
                    int value2 = int.Parse(values.Pop());
                    values.Push((value1 + value2).ToString());
                    operators.Pop();
                }
                catch
                {
                    throw new ArgumentException("the value stack contains fewer than 2 values");
                }
            }
            else if (operators.Peek() == "-")
            {
                try
                {
                    int value1 = int.Parse(values.Pop());
                    int value2 = int.Parse(values.Pop());
                    values.Push((value2 - value1).ToString());
                    operators.Pop();
                }
                catch
                {
                    throw new ArgumentException("the value stack contains fewer than 2 values");
                }
            }
        }

    }
}
