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
            foreach (String substring in substrings)
            {
                if (int.TryParse(substring, out int integer))
                {
                    if (operators.Count > 0)
                    {
                        DivideMultipleHelper(values, operators, integer);
                    }
                    else
                    {
                        values.Push(substring);
                    }
                }
                else if (substring == "+" || substring == "-")
                {
                    if (operators.Count > 0)
                    {
                        AddMinusHelper(values, operators);
                    }
                    operators.Push(substring);

                }
                else if (substring == "*" || substring == "/" || substring == "(")
                {
                    operators.Push(substring);
                }
                else if (substring == ")")
                {
                    AddMinusHelper(values, operators);
                    try
                    {
                        DivideMultipleHelper(values, operators, int.Parse(values.Pop()));
                    }
                    catch
                    {
                        throw new Exception("values are not enough");
                    }
                    if (operators.Peek() == "(")
                    {
                        operators.Pop();

                    }
                    else
                    {
                        throw new Exception("missing a (");
                    }
                }
                else
                {
                    if (substring != "")
                    {
                        try
                        {
                            int lookedValue = variableEvaluator(substring);
                            if (operators.Count > 0)
                            {
                                DivideMultipleHelper(values, operators, lookedValue);
                            }
                            else
                            {
                                values.Push(lookedValue.ToString());
                            }
                        }
                        catch(NullReferenceException)
                        {
                            throw new Exception("Unknown Variable exist: " + substring);
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
                throw new Exception("the this formula has wrong format");
            }

        }

        private static void DivideMultipleHelper(Stack<string> values, Stack<string> operators, int lookedValue)
        {
            if (operators.Peek() == "*")
            {
                try
                {
                    int result = lookedValue * int.Parse(values.Pop());
                    operators.Pop();
                    values.Push(result.ToString());
                }
                catch
                {
                    throw new Exception("The value stack is empty");
                }
            }
            else if (operators.Peek() == "/")
            {
                try
                {
                    int result = int.Parse(values.Pop()) / lookedValue;
                    operators.Pop();
                    values.Push(result.ToString());
                }
                catch
                {
                    throw new Exception("The value stack is empty or divided by 0 happened");
                }
            }
            else
            {
                values.Push(lookedValue.ToString());
            }
        }

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
                    throw new Exception("the value stack contains fewer than 2 values");
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
                    throw new Exception("the value stack contains fewer than 2 values");
                }
            }
        }

    }
}
