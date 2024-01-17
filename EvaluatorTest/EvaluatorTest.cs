// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("1", null) + "right is 1");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5+3", null) + "right is 8");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5-3", null) + "right is 2");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5*3", null) + "right is 15");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5/3", null) + "right is 1");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5*3+1", null) + "right is 16");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5/3-1", null) + "right is 0");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5+3*1", null) + "right is 8");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5-3/1", null) + "right is 2");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("3*3*3", null) + "right is 27");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5-(3/1+2)", null) + "right is 0");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5-(3/1-2)", null) + "right is 4");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("(5-(3/1-2)+5-(3/1+2))+8", null) + "right is 12");

Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5-3/one", a => { return 1; }) + "  right is 2");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("3*3*w234245sdf", a => { return 3; }) + "  right is 27");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5-(3/1+ewre)", a => { return 2; }) + "  right is 0");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("5-(3/1-jdf)", a => { return 2; }) + "  right is 4");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("(5-(3/1-2)+five-(3/1+2))+8", a => { return 5; }) + "  right is 12");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("(5-(3/1-2))*(5-(3/1-2))", null) + "  right is 16");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("(4)*(4)/(4)", null) + "  right is 4");
Console.WriteLine(FormulaEvaluator.Evaluator.Evaluate("(5-(3/1-jdf))*(5-(3/1-jdf))/(5-(3/1-jdf))", a => { return 2; }) + "  right is 4");



try
{
    FormulaEvaluator.Evaluator.Evaluate("5-3/0", null);
    Console.WriteLine("Divide by 0 Exception not worked");
}
catch (Exception)
{
    Console.WriteLine("Divide by 0 Exception worked");
}

try
{
    FormulaEvaluator.Evaluator.Evaluate("5-3/woerd", null);
    Console.WriteLine("Found unknown variables not worked");
}
catch (Exception)
{
    Console.WriteLine("Found unknown variables worked");
}

try
{
    FormulaEvaluator.Evaluator.Evaluate("5-3/woerd", a =>{ return 0; });
    Console.WriteLine("Cant divide by 0 not worked");
}
catch (Exception)
{
    Console.WriteLine("Cant divide by 0 worked");
}

try
{
    FormulaEvaluator.Evaluator.Evaluate("*7", null);
    Console.WriteLine("Cant found when value stack empty");
}
catch (Exception)
{
    Console.WriteLine("Can find value stack is empty");
}

try
{
    FormulaEvaluator.Evaluator.Evaluate("unknown+", a =>{ return 3; });
    Console.WriteLine("Cant found when value stack empty");
}
catch (Exception)
{
    Console.WriteLine("Can find value stack is empty");
}

try
{
    FormulaEvaluator.Evaluator.Evaluate("++()()(+", a => { return 3; });
    Console.WriteLine("Cant found when format is not right");
}
catch (Exception)
{
    Console.WriteLine("Can find wrong format");
}
