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
/// This is a formula evaoluator test file, It is a console application.
/// It will use console printing method to test if all methods are rights,
/// And also it will test the exception throwing. I learn the test template from
/// the assignment instruction.
/// </summary>
/// 

// See https://aka.ms/new-console-template for more information
using System.Runtime.CompilerServices;
using FormulaEvaluator;

//This is to test all the possible without variable situation to check if all of them are right.
if (Evaluator.Evaluate("5+3", null) == 8 &&
    Evaluator.Evaluate("5*3", null) == 15 &&
    Evaluator.Evaluate("1", null) == 1 &&
    Evaluator.Evaluate("5/3", null) == 1 &&
    Evaluator.Evaluate("5*3+1", null) == 16 &&
    Evaluator.Evaluate("5/3-1", null) == 0 &&
    Evaluator.Evaluate("5+3*1", null) == 8 &&
    Evaluator.Evaluate("5-3/1", null) == 2 &&
    Evaluator.Evaluate("3*3*3", null) == 27 &&
    Evaluator.Evaluate("5-(3/1+2)", null) == 0 &&
    Evaluator.Evaluate("(5-(3/1-2)+5-(3/1+2))+8", null) == 12 &&
    Evaluator.Evaluate("(5-(3/1-2))*(5-(3/1-2))", null) == 16 &&
    Evaluator.Evaluate("(7891234)", null) == 7891234 &&
    Evaluator.Evaluate("6*6*6*6-5*5*7*5-08273", null) == -7852 &&
    Evaluator.Evaluate("((((((((1)+2)*3)-9)+2)-23)/3)*43)", null) == -301 &&
    Evaluator.Evaluate("2+(134+(2+(2*(23-(0-(3*(2+(1))))+2)-23)/3)*43)", null) == 867
    )
{
    Console.WriteLine();
    Console.WriteLine("Tests without Variable are Sucessful!!");
    Console.WriteLine();
}
else
{
    Console.WriteLine("Failed");
}

//This is to test all the possible with variable situation to check if all of them are right.
if (Evaluator.Evaluate("5-3/one1", a => { return 1; }) == 2 &&
    Evaluator.Evaluate("3*3*w234245", a => { return 3; }) == 27 &&
    Evaluator.Evaluate("5-(3/1+ewre8)", a => { return 2; }) == 0 &&
    Evaluator.Evaluate("5-(3/1-jdf8)", a => { return 2; }) == 4 &&
    Evaluator.Evaluate("(5-(3/1-2)+five8-(3/1+2))+8", a => { return 5; }) == 12 &&
    Evaluator.Evaluate("(5-(3/1-jdf8))*(5-(3/1-jdf8))/(5-(3/1-jdf8))", a => { return 2; }) == 4 &&
    Evaluator.Evaluate("x1*x1*x2", a => { return 2; }) == 8 &&
    Evaluator.Evaluate("x1*x1*x2", a => { if(a == "x1") { return 1; } else { return 2; }; }) == 2 &&
    Evaluator.Evaluate("(x1)*(x2*x1*34*2*2/(x1*x2*x2))", a => { if (a == "x1") { return 1; } else { return 2; }; }) == 68 &&
    Evaluator.Evaluate("5-3/1", a => { return 1; }) == 2
    )
{
    Console.WriteLine();
    Console.WriteLine("Tests with the variables are Sucessful!!");
    Console.WriteLine();
}
else
{
    Console.WriteLine();
    Console.WriteLine("Failed");
    Console.WriteLine();
}

//This will test all the wrong edge situation to check if it can throw argument exception properly
try
{
    FormulaEvaluator.Evaluator.Evaluate("5-3/0", null);
    Console.WriteLine("Divide by 0 Exception not worked");
}
catch (ArgumentException)
{
    Console.WriteLine("Divide by 0 Exception worked");
}

try
{
    FormulaEvaluator.Evaluator.Evaluate("5-3/woerd", null);
    Console.WriteLine("Found unknown variables not worked");
}
catch (ArgumentException)
{
    Console.WriteLine("Found unknown variables worked");
}

try
{
    FormulaEvaluator.Evaluator.Evaluate("5-3/woerd", a => { return 0; });
    Console.WriteLine("Cant divide by 0 not worked");
}
catch (ArgumentException)
{
    Console.WriteLine("Cant divide by 0 worked");
}

try
{
    FormulaEvaluator.Evaluator.Evaluate("*7", null);
    Console.WriteLine("Cant found when value stack empty");
}
catch (ArgumentException)
{
    Console.WriteLine("Can find value stack is empty");
}

try
{
    FormulaEvaluator.Evaluator.Evaluate("unknown+", a => { return 3; });
    Console.WriteLine("Cant found when value stack empty");
}
catch (ArgumentException)
{
    Console.WriteLine("Can find value stack is empty");
}

try
{
    FormulaEvaluator.Evaluator.Evaluate("++()()(+", a => { return 3; });
    Console.WriteLine("Cant found when format is not right");
}
catch (ArgumentException)
{
    Console.WriteLine("Can find wrong format");
}

try
{
    FormulaEvaluator.Evaluator.Evaluate("3x+3", a => { return 3; });
    Console.WriteLine("Cant found when format is not right");
}
catch (ArgumentException)
{
    Console.WriteLine("Find wrong variable format");
}

try
{
    FormulaEvaluator.Evaluator.Evaluate(" + ", a => { return 3; });
    Console.WriteLine("Cant found space");
}
catch (ArgumentException)
{
    Console.WriteLine("Find space which is correct");
}

try
{
    FormulaEvaluator.Evaluator.Evaluate("-2+3", null);
    Console.WriteLine("Cant find negative sign");
}
catch (ArgumentException)
{
    Console.WriteLine("Can dined the negative sign");
}

try
{
    FormulaEvaluator.Evaluator.Evaluate("-(2+3)", null);
    Console.WriteLine("Cant find negative sign");
}
catch (ArgumentException)
{
    Console.WriteLine("Can dined the negative sign");
}
