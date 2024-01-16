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
