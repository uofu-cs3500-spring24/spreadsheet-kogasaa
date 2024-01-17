using System.Text.RegularExpressions;

namespace FormulaEvaluator
{
    public static class Evaluator
    {
        public delegate int Lookup(String variable_name);

        public static int Evaluate(String expression, Lookup variableEvaluator)
        {
            string[] substrings = Regex.Split(expression, "(\\()|(\\))|(-)|(\\+)|(\\*)|(/)");
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
        /// <exception cref="ArgumentException"></exception>
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
        /// <param name="operators"></param>
        /// <exception cref="ArgumentException"></exception>
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
