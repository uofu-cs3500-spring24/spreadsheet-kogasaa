/// <summary>
/// Author: Bingkun Han
/// Partner: None
/// Date: 1st-Feb-2024
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
/// This is a implementaion of a formula API, I create a private string for the whole class 
/// formula expression. I also create the helper method to check format in the constructor
/// They will help constructor check the syntax of the formula. This implemention can make
/// sure formula object will as it was expected.
/// </summary>



// Skeleton written by Joe Zachary for CS 3500, September 2013
// Read the entire skeleton carefully and completely before you
// do anything else!

// Version 1.1 (9/22/13 11:45 a.m.)

// Change log:
//  (Version 1.1) Repaired mistake in GetTokens
//  (Version 1.1) Changed specification of second constructor to
//                clarify description of how validation works

// (Daniel Kopta) 
// Version 1.2 (9/10/17) 

// Change log:
//  (Version 1.2) Changed the definition of equality with regards
//                to numeric tokens


using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace SpreadsheetUtilities
{
  /// <summary>
  /// Represents formulas written in standard infix notation using standard precedence
  /// rules.  The allowed symbols are non-negative numbers written using double-precision 
  /// floating-point syntax (without unary preceeding '-' or '+'); 
  /// variables that consist of a letter or underscore followed by 
  /// zero or more letters, underscores, or digits; parentheses; and the four operator 
  /// symbols +, -, *, and /.  
  /// 
  /// Spaces are significant only insofar that they delimit tokens.  For example, "xy" is
  /// a single variable, "x y" consists of two variables "x" and y; "x23" is a single variable; 
  /// and "x 23" consists of a variable "x" and a number "23".
  /// 
  /// Associated with every formula are two delegates:  a normalizer and a validator.  The
  /// normalizer is used to convert variables into a canonical form, and the validator is used
  /// to add extra restrictions on the validity of a variable (beyond the standard requirement 
  /// that it consist of a letter or underscore followed by zero or more letters, underscores,
  /// or digits.)  Their use is described in detail in the constructor and method comments.
  /// </summary>
  public class Formula
  {

        private string normalizedFormula;


        
        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically invalid,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer is the identity function, and the associated validator
        /// maps every string to true.  
        /// </summary>
        public Formula(String formula) :
        this(formula, s => s, s => true)
        {
        }

        /// <summary>
        /// Creates a Formula from a string that consists of an infix expression written as
        /// described in the class comment.  If the expression is syntactically incorrect,
        /// throws a FormulaFormatException with an explanatory Message.
        /// 
        /// The associated normalizer and validator are the second and third parameters,
        /// respectively.  
        /// 
        /// If the formula contains a variable v such that normalize(v) is not a legal variable, 
        /// throws a FormulaFormatException with an explanatory message. 
        /// 
        /// If the formula contains a variable v such that isValid(normalize(v)) is false,
        /// throws a FormulaFormatException with an explanatory message.
        /// 
        /// Suppose that N is a method that converts all the letters in a string to upper case, and
        /// that V is a method that returns true only if a string consists of one letter followed
        /// by one digit.  Then:
        /// 
        /// new Formula("x2+y3", N, V) should succeed
        /// new Formula("x+y3", N, V) should throw an exception, since V(N("x")) is false
        /// new Formula("2x+y3", N, V) should throw an exception, since "2x+y3" is syntactically incorrect.
        /// </summary>
        public Formula(String formula, Func<string, string> normalize, Func<string, bool> isValid)
        {
            //firstly tokenlization of the formula expression to check syntax errors
            List<string> formulaTokens = GetTokens(formula).ToList();

            //normalize all the variable and number (like normalize from 2e2 to 200)
            NormalizeFormulaVariabels(formulaTokens, normalize, isValid);

            //check the formula syntax correction, throw formula format exception when it syntax wrong
            if (!CheckFormat(formulaTokens))
            {
                throw new FormulaFormatException("the variable are all correct, but the format of formula - " + formula + " - is wrong!");
            }

            //create a immutable private formula expression in the class
            this.normalizedFormula = "";
            foreach (var token in formulaTokens)
            {
                this.normalizedFormula += token;
            }
        }


        /// <summary>
        /// This method is a helper method to change all the variable and numbers
        ///      1. the variabel will become normalized version and use validor to check it
        ///      2. the number like 2.00000, 2e3 will be transfer to normal double expression
        ///         to like 2.0 and 2000.0
        /// </summary>
        /// <param name="formulaTokens">the formula tokens to normlaized</param>
        /// <exception cref="FormulaFormatException"> 
        /// it will throw FormulaFormat Exception in two Situations:
        ///     1. when normlier goes wrong
        ///     2. when validor regard normalized variable is illegal
        /// 
        /// </exception>
        private void NormalizeFormulaVariabels(List<string> formulaTokens, Func<string, string> nomalizor, Func<string, bool> validor)
        {
            for(int i = 0; i < formulaTokens.Count; i++)
            {
                if (!(Double.TryParse(formulaTokens[i], out double ignore) ||
                      Regex.IsMatch(formulaTokens[i], @"^[\+\-*/]$") ||
                      formulaTokens[i] == "(" ||
                      formulaTokens[i] == ")"))
                {
                    string newFormatVariable = formulaTokens[i];
                    try
                    {
                        newFormatVariable = nomalizor(newFormatVariable);

                    }
                    catch
                    {
                        throw new FormulaFormatException("the variable: " + formulaTokens[i] + " - cannot pass the nomalizor");
                    }
                    
                    if (validor(newFormatVariable))
                    {
                        formulaTokens[i] = newFormatVariable;
                    }
                    else
                    {
                        throw new FormulaFormatException("the orignal variable " + formulaTokens[i] + " is normalized to " + newFormatVariable +" but cannot pass the validor");
                    }
                }
                else if (Double.TryParse(formulaTokens[i], out double correctFormatDouble))
                {
                    formulaTokens[i] = correctFormatDouble.ToString();
                }
            }
        }

        /// <summary>
        /// This is a method to check formula rules:
        ///     1. one token rule
        ///     2. end token rule
        ///     3. start token rule
        ///     4. balance rule
        ///     5. specialize token rule
        ///     6. follow rule
        ///     7. extra token rule
        /// if one of the rule does not pass it will return wrong
        /// the constructor will throw a FormulaFormatExcpeiton
        /// </summary>
        /// <param name="formulaTokens">a list of formula tokens</param>
        /// <returns>
        ///     1. True: when pass all 7 rules
        ///     2. False: one of the rules does not pass
        /// </returns>
        private bool CheckFormat(List<string> formulaTokens)
        {
            // Patterns for individual tokens, Copied from get token method
            String opPattern = @"^[\+\-*/]$";
            String varPattern = @"^[a-zA-Z_]([0-9a-zA-Z_]+)?$";

            bool oneTokenRule = formulaTokens.Count > 0;
            if (!oneTokenRule) 
            {
                 return false;
            }
            bool endTokenRule = Double.TryParse(formulaTokens.Last(), out double ignore1) || 
                                Regex.IsMatch(formulaTokens.Last(), varPattern)|| 
                                formulaTokens.Last() == ")";
            bool startTokenRule = Double.TryParse(formulaTokens.First(), out double ignore2) ||
                                  Regex.IsMatch(formulaTokens.First(), varPattern) || 
                                  formulaTokens.First() == "(";
            int numOfRightParentheses = 0;
            int numOfLeftParentheses = 0;
            bool specificTokenRule = true;
            foreach(string token in formulaTokens)
            {
                if(token == "(")
                    numOfLeftParentheses++;
                if(token == ")")
                    numOfRightParentheses++;
                if(!(Regex.IsMatch(token, varPattern)||
                    Double.TryParse(token, out double ignore3) || 
                    Regex.IsMatch(token, opPattern)||
                    token=="("||
                    token==")"))
                    specificTokenRule = false;
            }
            bool balanceParenRule = numOfLeftParentheses == numOfRightParentheses;
            if (!(endTokenRule && startTokenRule && balanceParenRule && specificTokenRule))
            {
                return false;
            }

            bool followRule = true;
            for(int i = 1; i < formulaTokens.Count; i++)
            {
                if (Regex.IsMatch(formulaTokens[i-1], opPattern) || formulaTokens[i-1] == "(")
                {
                    if (Regex.IsMatch(formulaTokens[i], opPattern))
                    {
                        followRule = false; break;
                    }
                }
            }
            bool extraFollowRule = true;
            for (int i = 1; i < formulaTokens.Count; i++)
            {
                if (Double.TryParse(formulaTokens[i - 1], out double ignore4) ||
                    Regex.IsMatch(formulaTokens[i - 1], varPattern)||
                    formulaTokens[i - 1] == ")")
                {
                    if (!(Regex.IsMatch(formulaTokens[i], opPattern) || formulaTokens[i] == ")"))
                    {
                        extraFollowRule = false; break;
                    }
                }
            }

            return followRule && extraFollowRule;

        }

        /// <summary>
        /// This method will pass a list of tokens of a formula like {"9","*","9"} and a function to
        /// change a string(variable) to a sepecific integer values. It will used when there is 
        /// string variable exist in the formula. This method will calculate the formula as tokens
        /// and return a double type calculate result
        /// </summary>
        /// <param name="formulaTokens">the list of tokens of the formula</param>
        /// <param name="variableEvaluator">the function to convert string varible into a integer</param>
        /// <returns> double result, the result of the expression</returns>
        /// <exception cref="ArgumentException">
        ///     1. when divid by 0 happend
        ///     2. when lookup method goes wrong - unknown variabales, lookup throw a exception
        ///     those will all make evaluator return a formula error
        /// </exception>
        private static double EvaluateHelper(string formulaStringExpression, Func<string, double> variableEvaluator)
        {
            // I learnt how to use generic stack class from microsoft learning webpage
            Stack<String> values = new Stack<String>();
            Stack<String> operators = new Stack<String>();
            string[] formulaTokens = GetTokens(formulaStringExpression).ToArray();
            foreach (String token in formulaTokens)
            {
                if (double.TryParse(token, out double tokenDoubleValue))
                {
                    if (operators.Count > 0)
                    {
                        DivideMultipleHelper(values, operators, tokenDoubleValue);
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
                    operators.Pop();
                    if (values.Count > 1)
                    {
                        DivideMultipleHelper(values, operators, double.Parse(values.Pop()));
                    }
                }
                else
                {
                    try
                    {
                        double lookedValue = variableEvaluator(token);
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
                        throw new ArgumentException("Unknown Variable exist: " + token + " or divide by 0 happened");
                    }
                }
            }
            if (values.Count == 1 && operators.Count == 0)
            {
                double finalResult = double.Parse(values.Pop());
                return finalResult;
            }
            else
            {
                AddMinusHelper(values, operators);
                double finalResult = double.Parse(values.Pop());
                return finalResult;
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
        private static void DivideMultipleHelper(Stack<string> values, Stack<string> operators, double passedValue)
        {
            if (operators.Peek() == "*")
            {
                double result = passedValue * double.Parse(values.Pop());
                operators.Pop();
                values.Push(result.ToString());
            }
            else if (operators.Peek() == "/")
            {
                try
                {
                    double result = double.Parse(values.Pop()) / passedValue;
                    operators.Pop();
                    values.Push(result.ToString());
                    if(result == double.PositiveInfinity || result == double.NegativeInfinity)
                    {
                        throw new ArgumentException();
                    }
                }
                catch
                {
                    throw new ArgumentException("Divide by 0 happend!");
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
                double value1 = double.Parse(values.Pop());
                double value2 = double.Parse(values.Pop());
                values.Push((value1 + value2).ToString());
                operators.Pop();
            }
            else if (operators.Peek() == "-")
            {
                
                double value1 = double.Parse(values.Pop());
                double value2 = double.Parse(values.Pop());
                values.Push((value2 - value1).ToString());
                operators.Pop();
            }
        }


        /// <summary>
        /// Evaluates this Formula, using the lookup delegate to determine the values of
        /// variables.  When a variable symbol v needs to be determined, it should be looked up
        /// via lookup(normalize(v)). (Here, normalize is the normalizer that was passed to 
        /// the constructor.)
        /// 
        /// For example, if L("x") is 2, L("X") is 4, and N is a method that converts all the letters 
        /// in a string to upper case:
        /// 
        /// new Formula("x+7", N, s => true).Evaluate(L) is 11
        /// new Formula("x+7").Evaluate(L) is 9
        /// 
        /// Given a variable symbol as its parameter, lookup returns the variable's value 
        /// (if it has one) or throws an ArgumentException (otherwise).
        /// 
        /// If no undefined variables or divisions by zero are encountered when evaluating 
        /// this Formula, the value is returned.  Otherwise, a FormulaError is returned.  
        /// The Reason property of the FormulaError should have a meaningful explanation.
        ///
        /// This method should never throw an exception.
        /// </summary>
        public object Evaluate(Func<string, double> lookup)
        {
            try
            {
                return EvaluateHelper(normalizedFormula, lookup);
            }catch (ArgumentException e) 
            {
                return new FormulaError(e.Message);
            }
            
        }

        /// <summary>
        /// Enumerates the normalized versions of all of the variables that occur in this 
        /// formula.  No normalization may appear more than once in the enumeration, even 
        /// if it appears more than once in this Formula.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x+y*z", N, s => true).GetVariables() should enumerate "X", "Y", and "Z"
        /// new Formula("x+X*z", N, s => true).GetVariables() should enumerate "X" and "Z".
        /// new Formula("x+X*z").GetVariables() should enumerate "x", "X", and "z".
        /// </summary>
        public IEnumerable<String> GetVariables()
        {
            HashSet<String> variables = new HashSet<String>();
            List<string> formulaTokens = GetTokens(normalizedFormula).ToList();
            foreach (string token in formulaTokens)
            {
                if(Regex.IsMatch(token, @"^[a-zA-Z_]([0-9a-zA-Z_]+)?$"))
                {
                    variables.Add(token);
                }
            }
            return variables;
        }

        /// <summary>
        /// Returns a string containing no spaces which, if passed to the Formula
        /// constructor, will produce a Formula f such that this.Equals(f).  All of the
        /// variables in the string should be normalized.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        /// 
        /// new Formula("x + y", N, s => true).ToString() should return "X+Y"
        /// new Formula("x + Y").ToString() should return "x+Y"
        /// </summary>
        public override string ToString()
        {
            return normalizedFormula;
        }

        /// <summary>
        ///  <change> make object nullable </change>
        ///
        /// If obj is null or obj is not a Formula, returns false.  Otherwise, reports
        /// whether or not this Formula and obj are equal.
        /// 
        /// Two Formulae are considered equal if they consist of the same tokens in the
        /// same order.  To determine token equality, all tokens are compared as strings 
        /// except for numeric tokens and variable tokens.
        /// Numeric tokens are considered equal if they are equal after being "normalized" 
        /// by C#'s standard conversion from string to double, then back to string. This 
        /// eliminates any inconsistencies due to limited floating point precision.
        /// Variable tokens are considered equal if their normalized forms are equal, as 
        /// defined by the provided normalizer.
        /// 
        /// For example, if N is a method that converts all the letters in a string to upper case:
        ///  
        /// new Formula("x1+y2", N, s => true).Equals(new Formula("X1  +  Y2")) is true
        /// new Formula("x1+y2").Equals(new Formula("X1+Y2")) is false
        /// new Formula("x1+y2").Equals(new Formula("y2+x1")) is false
        /// new Formula("2.0 + x7").Equals(new Formula("2.000 + x7")) is true
        /// </summary>
        public override bool Equals(object? obj)
        {
            if(obj is Formula)
            {
                return this.ToString() == ((Formula)obj).ToString();
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        /// Reports whether f1 == f2, using the notion of equality from the Equals method.
        /// 
        /// </summary>
        public static bool operator ==(Formula f1, Formula f2)
        {
            return f1.Equals(f2);
        }

        /// <summary>
        ///   <change> We are now using Non-Nullable objects.  Thus neither f1 nor f2 can be null!</change>
        ///   <change> Note: != should almost always be not ==, if you get my meaning </change>
        ///   Reports whether f1 != f2, using the notion of equality from the Equals method.
        /// </summary>
        public static bool operator !=(Formula f1, Formula f2)
        {
            return !f1.Equals(f2);
        }

        /// <summary>
        /// Returns a hash code for this Formula.  If f1.Equals(f2), then it must be the
        /// case that f1.GetHashCode() == f2.GetHashCode().  Ideally, the probability that two 
        /// randomly-generated unequal Formulae have the same hash code should be extremely small.
        /// </summary>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }

        /// <summary>
        /// Given an expression, enumerates the tokens that compose it.  Tokens are left paren;
        /// right paren; one of the four operator symbols; a string consisting of a letter or underscore
        /// followed by zero or more letters, digits, or underscores; a double literal; and anything that doesn't
        /// match one of those patterns.  There are no empty tokens, and no token contains white space.
        /// </summary>
        private static IEnumerable<string> GetTokens(String formula)
        {
          // Patterns for individual tokens
          String lpPattern = @"\(";
          String rpPattern = @"\)";
          String opPattern = @"[\+\-*/]";
          String varPattern = @"[a-zA-Z_](?: [a-zA-Z_]|\d)*";
          String doublePattern = @"(?: \d+\.\d* | \d*\.\d+ | \d+ ) (?: [eE][\+-]?\d+)?";
          String spacePattern = @"\s+";

          // Overall pattern
          String pattern = String.Format("({0}) | ({1}) | ({2}) | ({3}) | ({4}) | ({5})",
                                          lpPattern, rpPattern, opPattern, varPattern, doublePattern, spacePattern);

          // Enumerate matching tokens that don't consist solely of white space.
          foreach (String s in Regex.Split(formula, pattern, RegexOptions.IgnorePatternWhitespace))
          {
            if (!Regex.IsMatch(s, @"^\s*$", RegexOptions.Singleline))
            {
              yield return s;
            }
          }

        }
  }

  /// <summary>
  /// Used to report syntactic errors in the argument to the Formula constructor.
  /// </summary>
  public class FormulaFormatException : Exception
  {
    /// <summary>
    /// Constructs a FormulaFormatException containing the explanatory message.
    /// </summary>
    public FormulaFormatException(String message)
        : base(message)
    {
    }
  }

  /// <summary>
  /// Used as a possible return value of the Formula.Evaluate method.
  /// </summary>
  public struct FormulaError
  {
    /// <summary>
    /// Constructs a FormulaError containing the explanatory reason.
    /// </summary>
    /// <param name="reason"></param>
    public FormulaError(String reason)
        : this()
    {
      Reason = reason;
    }

    /// <summary>
    ///  The reason why this FormulaError was created.
    /// </summary>
    public string Reason { get; private set; }
  }
}


// <change>
//   If you are using Extension methods to deal with common stack operations (e.g., checking for
//   an empty stack before peeking) you will find that the Non-Nullable checking is "biting" you.
//
//   To fix this, you have to use a little special syntax like the following:
//
//       public static bool OnTop<T>(this Stack<T> stack, T element1, T element2) where T : notnull
//
//   Notice that the "where T : notnull" tells the compiler that the Stack can contain any object
//   as long as it doesn't allow nulls!
// </change>
