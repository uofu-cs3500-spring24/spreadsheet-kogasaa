using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        // the dictionary is a container of cells, string is name of object, and a cell object
        private Dictionary<string, Cell> SpreadsheetCells;
        // dependency graph of a spreadsheet
        private DependencyGraph DependencyGraph;

        private static string correctNamePattern = @"^[a-zA-Z_]([0-9a-zA-Z_]+)?$";

        /// <summary>
        /// Construct a empty spreadsheet
        /// </summary>
        public Spreadsheet()
        {
            SpreadsheetCells = new Dictionary<string, Cell>();
            DependencyGraph = new DependencyGraph();
        }

        /// <summary>
        /// This will return the object that the same name cell contained
        /// Throw invalid name when the name is wrong or null
        /// </summary>
        /// <param name="name">The name of the cell</param>
        /// <returns>Same name cell's value</returns>
        /// <exception cref="InvalidNameException">when name is invalid
        /// or it is null</exception>
        public override object GetCellContents(string name)
        {
            NameChecking(name);
            if (SpreadsheetCells.ContainsKey(name))
            {
                return SpreadsheetCells[name].Value;
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Check Cell Name valid status, if it is wrong it will Throw
        /// InvalidNameException
        /// </summary>
        /// <param name="name">name of Cell</param>
        /// <exception cref="InvalidNameException"> If name is null or invalid
        /// Throw this exception</exception>
        private static void NameChecking(string name)
        {
            if (name == null || Regex.IsMatch(name, correctNamePattern))
            {
                throw new InvalidNameException();
            }
        }

        /// <summary>
        /// It will return all the cell with non-empty contents inside it
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return SpreadsheetCells.Keys;
        }

        /// <summary>
        /// Set a double in a cell it 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="number"></param>
        /// <returns></returns>
        public override ISet<string> SetCellContents(string name, double number)
        {
            NameChecking(name);
            Cell cell = new Cell(name, number);
            if (SpreadsheetCells.ContainsKey(name))
            {
                SpreadsheetCells[name].Value = cell;
            }
            else
            {
                SpreadsheetCells.Add(name, cell);
            }
            return GetDirectDependents(name).ToHashSet();
        }

        public override ISet<string> SetCellContents(string name, string text)
        {
            NameChecking(name);
            if(text == null)
            {
                throw new ArgumentNullException("The set text is null!");
            }
            Cell cell = new Cell(name, text);
            if (SpreadsheetCells.ContainsKey(name))
            {
                SpreadsheetCells[name].Value = cell;
            }
            else
            {
                SpreadsheetCells.Add(name, cell);
            }
            if(text == "")
            {
                SpreadsheetCells.Remove(name);
            }
            return GetDirectDependents(name).ToHashSet();
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            NameChecking(name);
            if(formula == null)
            {
                throw new ArgumentNullException("You are setting a null formula in the cell - " + name +" !");
            }
            Cell cell = new Cell(name, formula);
            if (SpreadsheetCells.ContainsKey(name))
            {
                SpreadsheetCells[name].Value = cell;
            }
            else
            {
                SpreadsheetCells.Add(name, cell);
            }
            return GetDirectDependents(name).ToHashSet();
        }

        private void DeletePreviousDependees(string name)
        {
            if (SpreadsheetCells.ContainsKey(name))
            {
                if(SpreadsheetCells[name].Value.GetType() == typeof(Formula))
                {
                    DependencyGraph.ReplaceDependees(name, new List<string>());
                }
            }
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            HashSet<string> dependents = DependencyGraph.GetDependents(name).ToHashSet();
            return dependents;
        }
    }

    internal class Cell
    {
        public string Name { get; set; }
        public object Value { get; set; }

        public Cell(string name, object value)
        {
            Value = value;
            Name = name;
        }

    }
}
