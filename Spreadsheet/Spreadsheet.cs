/// <summary>
/// Author: Bingkun Han
/// Partner: None
/// Date: 8th-Feb-2024
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
/// It is child class of abstact class of spreadsheet. I implemented all teh 
/// methods from the abstract spreadsheet class. I also create the internal 
/// cell class for future convinent coding. Also helper method for checking
/// name valid or null. Then I also have helper method to modify the spread-
/// sheet.
/// </summary>

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
                Cell targetCell = SpreadsheetCells[name];
                return targetCell.Value;
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
            if (name == null || !Regex.IsMatch(name, correctNamePattern))
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
            return SpreadsheetCells.Keys.ToHashSet();
        }

        /// <summary>
        /// Set a double in a cell and put it in the dictioanry. Then it will
        /// return the cells depended on it which need recalculating including 
        /// itself.
        /// </summary>
        /// <param name="name">
        /// The name of the cell
        /// </param>
        /// <param name="number">
        /// The double this cell will has
        /// </param>
        /// <exception cref="InvalidNameException"
        /// it will throw a invalidNameException when the name is invalid or null
        /// </exception>
        /// <returns>
        /// The Set of cells needed to recalculated included it self
        /// </returns>
        public override ISet<string> SetCellContents(string name, double number)
        {
            NameChecking(name);
            Cell cell = new Cell(name, number);
            object oldCellValue = new object();
            if (SpreadsheetCells.ContainsKey(name))
            {
                oldCellValue = SpreadsheetCells[name].Value;
                SpreadsheetCells[name] = cell;
            }
            else
            {
                SpreadsheetCells.Add(name, cell);
            }
            HashSet<string> changedCells = GetCellsToRecalculate(name).ToHashSet();
            DeletePreviousDependees(name, oldCellValue);
            return changedCells;
        }

        /// <summary>
        /// Set a string in a cell and put it in the dictioanry. Then it will
        /// return the cells depended on it which need recalculating including itself.
        /// </summary>
        /// <param name="name">
        /// The name of the cell
        /// </param>
        /// <param name="text">
        /// The text the cell will contain
        /// </param>
        /// <exception cref="InvalidNameException"
        /// it will throw a invalidNameException when the name is invalid or null
        /// </exception>
        /// <exception cref="ArgumentNullException"
        /// it will throw a ArugmentNullException when the text is null
        /// </exception>
        /// <returns>
        /// The Set of cells needed to recalculated included it self
        /// </returns>
        public override ISet<string> SetCellContents(string name, string text)
        {
            NameChecking(name);
            if(text == null)
            {
                throw new ArgumentNullException("The set text is null!");
            }
            Cell cell = new Cell(name, text);
            object oldCellValue = new object();
            if (SpreadsheetCells.ContainsKey(name))
            {
                oldCellValue = SpreadsheetCells[name].Value;
                SpreadsheetCells[name] = cell;
            }
            else
            {
                SpreadsheetCells.Add(name, cell);
            }
            if(text == "")
            {
                SpreadsheetCells.Remove(name);
            }
            HashSet<string> changedCells = GetCellsToRecalculate(name).ToHashSet();
            DeletePreviousDependees(name, oldCellValue);
            return changedCells;
        }

        /// <summary>
        /// Set a Formula in a cell and put it in the dictioanry. Then it will
        /// return the cells depended on it which need recalculating including itself.
        /// </summary>
        /// <param name="name">
        /// The name of the cell
        /// </param>
        /// <param name="text">
        /// The formula the cell will contain
        /// </param>
        /// <exception cref="InvalidNameException"
        /// it will throw a invalidNameException when the name is invalid or null
        /// </exception>
        /// <exception cref="ArgumentNullException"
        /// it will throw a ArugmentNullException when the formula is null
        /// </exception>
        /// <exception cref="CircularException"
        /// When there is a circular dependecy exist in the spreadsheet dependency graph
        /// </exception>
        /// <returns>
        /// The Set of cells needed to recalculated included it self
        /// </returns>
        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            NameChecking(name);
            try
            {
                formula.Equals(null);
            }
            catch
            {
                throw new ArgumentNullException("You are setting a null formula in the cell - " + name +" !");
            }
            Cell cell = new Cell(name, formula);
            if (SpreadsheetCells.ContainsKey(name))
            {
                SpreadsheetCells[name] = cell;
            }
            else
            {
                SpreadsheetCells.Add(name, cell);
            }
            HashSet<string> newDependees = formula.GetVariables().ToHashSet();
            DependencyGraph.ReplaceDependees(name, newDependees);
            HashSet<string> changedCells = GetCellsToRecalculate(name).ToHashSet();
            return changedCells;
        }

        /// <summary>
        /// THis is a helper method used in the setcellcontent (double version and string 
        /// version)
        /// This method will delete all the dependees when a cell used be a formula ,and it
        /// will becaome a text or double. It will delete the dependency with all variables
        ///  in the old formula.
        /// </summary>
        /// <param name="name">the cell name to modify</param>
        /// <param name="oldCellValue"> the cell's previous value</param>
        private void DeletePreviousDependees(string name, object oldCellValue)
        {
            if (SpreadsheetCells.ContainsKey(name))
            {
                if(oldCellValue.GetType() == typeof(Formula))
                {
                    DependencyGraph.ReplaceDependees(name, new List<string>());
                }
            }
        }

        /// <summary>
        /// It will get the direct dependents of a cell.
        /// </summary>
        /// <param name="name">the target cell used to get its direct dependent</param>
        /// <returns></returns>
        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            HashSet<string> dependents = DependencyGraph.GetDependents(name).ToHashSet();
            return dependents;
        }
    }

    /// <summary>
    /// This cell class is to create a cell in a spreadsheet and Stored in it. It has basic name 
    /// and value inside it
    /// </summary>
    internal class Cell
    {
        // the name of this cell
        public string Name { get; set; }

        // the value of this cell
        public object Value { get; set; }

        /// <summary>
        /// The constructor to create a cell object with given name and variable
        /// </summary>
        /// <param name="name">given name of the cell</param>
        /// <param name="value">given variabel for the cell, it should be double , string, or formula</param>
        public Cell(string name, object value)
        {
            Value = value;
            Name = name;
        }
    }
}
