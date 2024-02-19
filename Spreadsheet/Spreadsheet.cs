/// <summary>
/// Author: Bingkun Han
/// Partner: None
/// Date: 18th-Feb-2024
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
/// 
/// Based on the AS4 I also implemnetated the extra features in the AS5:
/// 1. set cell contents only using by string and string.
/// 2. save spreadsheet file as xml file
/// 3. read a xml of a spreadsheet from a file
/// 4. get value of the cell
/// </summary>

using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        private Dictionary<string, Cell> SpreadsheetCells;
        private DependencyGraph DependencyGraph;
        private static string correctNamePattern = @"^[a-zA-Z]+[0-9]+$";
        
       
        /// <summary>
        /// True if this spreadsheet has been modified since it was created or saved                  
        /// (whichever happened most recently); false otherwise.
        /// </summary>
        public override bool Changed { get ; protected set; }


        /// <summary>
        /// Construct a empty spreadsheet
        /// </summary>
        public Spreadsheet():
        this(n => true, n => n, "default")
        {
        }

        /// <summary>
        /// Constructs an abstract spreadsheet by recording its variable validity test,
        /// its normalization method, and its version information.  
        /// </summary>
        /// 
        /// <remarks>
        ///   The variable validity test is used throughout to determine whether a string that consists of 
        ///   one or more letters followed by one or more digits is a valid cell name.  The variable
        ///   equality test should be used throughout to determine whether two variables are equal.
        /// </remarks>
        /// 
        /// <param name="isValid">   defines what valid variables look like for the application</param>
        /// <param name="normalize"> defines a normalization procedure to be applied to all valid variable strings</param>
        /// <param name="version">   defines the version of the spreadsheet (should it be saved)</param>
        public Spreadsheet(Func<string, bool> givenIsValid, Func<string, string> givenNormalizor, string VersionString):
        base(givenIsValid, givenNormalizor, VersionString)
        {
            SpreadsheetCells = new Dictionary<string, Cell>();
            DependencyGraph = new DependencyGraph();
            Changed = false;
        }

        /// <summary>
        /// To read a file of xml spreadsheet and find the all the cell informatin. All cells' name and contents, then we put them in the spreadsheet
        /// dictionary. We also pass the given Validor and normalizor. We also compare the version of spreadsheet in file and the spreadsheet version 
        /// we want to created
        /// </summary>
        /// <param name="pathToFile">The location to read the file of xml of spreadsheet</param>
        /// <param name="givenIsValid">defines what valid variables look like for the application </param>
        /// <param name="givenNormalizor">defines a normalization procedure to be applied to all valid variable strings</param>
        /// <param name="VersionString">defines the version of the spreadsheet (should it be saved)</param>
        /// <exception cref="SpreadsheetReadWriteException"></exception>
        public Spreadsheet(string pathToFile, Func<string, bool> givenIsValid, Func<string, string> givenNormalizor, string VersionString):
        base(givenIsValid, givenNormalizor, VersionString)
        {
            SpreadsheetCells = new Dictionary<string, Cell>();
            DependencyGraph = new DependencyGraph();

            if(VersionString != GetSavedVersion(pathToFile))
            {
                throw new SpreadsheetReadWriteException("The Version Does not Match");
            }

            try
            {
                using (XmlReader reader = XmlReader.Create(pathToFile))
                {
                    while (reader.Read())
                    {
                        string eachNewCellName = "";
                        string eachNewCellContent = "";
                        bool canCreateANewCell = false;
                        if (reader.IsStartElement())
                        {

                            switch (reader.Name)
                            {
                                case "name":
                                    reader.Read();
                                    eachNewCellName = reader.Value;
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    reader.Read();
                                    eachNewCellContent = reader.Value;
                                    canCreateANewCell = true;
                                    break;
                            }
                        }
                        if (canCreateANewCell)
                        {
                            SetContentsOfCell(eachNewCellName, eachNewCellContent);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(e.Message);
            }
            Changed = false;
        }

        /// <summary>
        /// It will return all the cell with non-empty contents inside it
        /// </summary>
        /// <returns>hte IEnumrable of all names of all cells' contents are not ""</returns>
        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            return SpreadsheetCells.Keys.ToHashSet();
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
            name = NormalizeAndCheckName(name);
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
        private string NormalizeAndCheckName(string name)
        {
            name = Normalize(name);
            if (IsValid(name))
            {
                if (name == null || !Regex.IsMatch(name, correctNamePattern))
                {
                    throw new InvalidNameException();
                }
            }
            else
            {
                throw new InvalidNameException();
            }

            return name;
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
        protected override IList<string> SetCellContents(string name, double number)
        {
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
            return changedCells.ToList();
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
        protected override IList<string> SetCellContents(string name, string text)
        {
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
            return changedCells.ToList();
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
        /// 
        /// <exception cref="ArgumentNullException"
        /// it will throw a ArugmentNullException when the formula is null
        /// </exception>
        /// <exception cref="CircularException"
        /// When there is a circular dependecy exist in the spreadsheet dependency graph
        /// </exception>
        /// <returns>
        /// The Set of cells needed to recalculated included it self
        /// </returns>
        protected override IList<string> SetCellContents(string name, Formula formula)
        {
            //Check formula variable correction
            foreach (string variable in formula.GetVariables())
            {
                NormalizeAndCheckName(variable);
            }
            Cell newCell = new Cell(name, formula);

            if (GetCellContents(name).GetType() != typeof(Formula))
            {
                IEnumerable<string> newDependees = formula.GetVariables();
                DependencyGraph.ReplaceDependees(name, newDependees);
                IList<string> cellsToRecalculated;
                try
                {
                    cellsToRecalculated = GetCellsToRecalculate(name).ToList();
                }
                catch(CircularException)
                {
                    DependencyGraph.ReplaceDependees(name, new List<string>());
                    throw new CircularException();
                }
                if (SpreadsheetCells.ContainsKey(name))
                {
                    SpreadsheetCells[name] = newCell;
                }
                else
                {
                    SpreadsheetCells.Add(name, newCell);
                }
                return cellsToRecalculated;
            }
            else
            {
                IEnumerable<string> newDependees = formula.GetVariables();
                IEnumerable<string> oldDependees = ((Formula)SpreadsheetCells[name].Value).GetVariables();
                DependencyGraph.ReplaceDependees(name, newDependees);
                IList<string> cellsToRecalculated;
                try
                {
                    cellsToRecalculated = GetCellsToRecalculate(name).ToList();
                }
                catch (CircularException)
                {
                    DependencyGraph.ReplaceDependees(name, oldDependees);
                    throw new CircularException();
                }
                SpreadsheetCells[name] = newCell;
                return cellsToRecalculated;
            } 
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
                if (oldCellValue.GetType() == typeof(Formula))
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


        /// <summary>
        ///   <para>Sets the contents of the named cell to the appropriate value. </para>
        ///   <para>
        ///       First, if the content parses as a double, the contents of the named
        ///       cell becomes that double.
        ///   </para>
        ///
        ///   <para>
        ///       Otherwise, if content begins with the character '=', an attempt is made
        ///       to parse the remainder of content into a Formula.  
        ///       There are then three possible outcomes:
        ///   </para>
        ///
        ///   <list type="number">
        ///       <item>
        ///           If the remainder of content cannot be parsed into a Formula, a 
        ///           SpreadsheetUtilities.FormulaFormatException is thrown.
        ///       </item>
        /// 
        ///       <item>
        ///           If changing the contents of the named cell to be f
        ///           would cause a circular dependency, a CircularException is thrown,
        ///           and no change is made to the spreadsheet.
        ///       </item>
        ///
        ///       <item>
        ///           Otherwise, the contents of the named cell becomes f.
        ///       </item>
        ///   </list>
        ///
        ///   <para>
        ///       Finally, if the content is a string that is not a double and does not
        ///       begin with an "=" (equal sign), save the content as a string.
        ///   </para>
        /// </summary>
        ///
        /// <exception cref="InvalidNameException"> 
        ///   If the name parameter is invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <exception cref="SpreadsheetUtilities.FormulaFormatException"> 
        ///   If the content is "=XYZ" where XYZ is an invalid formula, throw a FormulaFormatException.
        /// </exception>
        /// 
        /// <exception cref="CircularException"> 
        ///   If changing the contents of the named cell to be the formula would 
        ///   cause a circular dependency, throw a CircularException.  
        ///   (NOTE: No change is made to the spreadsheet.)
        /// </exception>
        /// 
        /// <param name="name"> The cell name that is being changed</param>
        /// <param name="content"> The new content of the cell</param>
        /// 
        /// <returns>
        ///       <para>
        ///           This method returns a list consisting of the passed in cell name,
        ///           followed by the names of all other cells whose value depends, directly
        ///           or indirectly, on the named cell. The order of the list MUST BE any
        ///           order such that if cells are re-evaluated in that order, their dependencies 
        ///           are satisfied by the time they are evaluated.
        ///       </para>
        ///
        ///       <para>
        ///           For example, if name is A1, B1 contains A1*2, and C1 contains B1+A1, the
        ///           list {A1, B1, C1} is returned.  If the cells are then evaluate din the order:
        ///           A1, then B1, then C1, the integrity of the Spreadsheet is maintained.
        ///       </para>
        /// </returns>
        public override IList<string> SetContentsOfCell(string name, string content)
        {
            name = NormalizeAndCheckName(name);
            Changed = true;
            if(double.TryParse(content, out double actualDouble))
            {
                return SetCellContents(name, actualDouble);
            }
            else if (content.Length > 0 && content[0] == '=')
            {
                content = content.Substring(1);
                Formula actualFormula = new Formula(content, Normalize, IsValid);
                return SetCellContents(name, actualFormula);
            }
            else
            {
                return SetCellContents(name, content);
            }
        }





        /// <summary>
        /// It will read the xml file with filename in the debug path
        /// it will use reader each line to find the informatino about the spreadsheet version
        /// </summary>
        /// <param name="filename">The path of file to be read</param>
        /// <returns>string of hte version, if the reader can not find a version it will return ""</returns>
        /// <exception cref="SpreadsheetReadWriteException">
        /// the error happened in the reader or find the file, it will throw a read write expectation
        /// </exception>
        public override string GetSavedVersion(string filename)
        {
            string ?version = null;
            try
            {
                
                using (XmlReader reader = XmlReader.Create(filename))
                {
                    while (reader.Read()) 
                    {
                        if (reader.IsStartElement())
                        {
                            
                            switch (reader.Name)
                            {
                                case "spreadsheet":
                                    version = reader["version"];
                                    return version;                            }   
                        }
                    }
                }
            } catch (Exception e)
            {
                throw new SpreadsheetReadWriteException(filename + " has this exception: " + e.Message);
            }

            return version;
            
        }





        /// <summary>
        /// THis is the method to save xml format of a spreadsheet to a file in the desk with file name
        /// </summary>
        /// <param name="filename"> the path location to save file</param>
        /// <exception cref="SpreadsheetReadWriteException"> throw when there is a problem happened in the writing file
        /// 1. when the path is not exist
        /// 2. some other writer errors when writing a xml</exception>
        public override void Save(string filename)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            Changed = false;

            try
            {
                using (XmlWriter writer = XmlWriter.Create(filename, settings))
                {


                    writer.WriteStartDocument();
                    writer.WriteStartElement("spreadsheet");

                    writer.WriteAttributeString("version", Version);


                    foreach (string cellName in GetNamesOfAllNonemptyCells())
                    {
                        writer.WriteStartElement("cell");
                        writer.WriteElementString("name", cellName);
                        writer.WriteElementString("contents", SpreadsheetCells[cellName].GetContentString());
                        writer.WriteEndElement();
                        writer.Flush();
                    }


                    writer.WriteEndElement();
                    writer.WriteEndDocument();

                }
            }catch(Exception e)
            {
                throw new SpreadsheetReadWriteException(e.Message);
            }
            
        }

        /// <summary>
        /// This is the method to return a XML format of a spreadsheet which contains all the cell information and contents
        /// </summary>
        /// <returns>
        /// XML string form of a spreadsheet
        /// </returns>
        public override string GetXML()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            StringWriter stringWriter = new StringWriter();


            using (XmlWriter writer = XmlWriter.Create(stringWriter, settings))
            {
                

                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");

                writer.WriteAttributeString("version", Version);


                foreach (string cellName in GetNamesOfAllNonemptyCells())
                {
                    writer.WriteStartElement("cell");
                    writer.WriteElementString("name", cellName);
                    writer.WriteElementString("contents", SpreadsheetCells[cellName].GetContentString());
                    writer.WriteEndElement();
                    writer.Flush();
                }

                
                writer.WriteEndElement(); 
                writer.WriteEndDocument();

            }
            return stringWriter.ToString();
        }




        /// <summary>
        /// If name is invalid, throws an InvalidNameException.
        /// </summary>
        ///
        /// <exception cref="InvalidNameException"> 
        ///   If the name is invalid, throw an InvalidNameException
        /// </exception>
        /// 
        /// <param name="name"> The name of the cell that we want the value of (will be normalized)</param>
        /// 
        /// <returns>
        ///   Returns the value (as opposed to the contents) of the named cell.  The return
        ///   value should be either a string, a double, or a SpreadsheetUtilities.FormulaError.
        /// </returns>
        public override object GetCellValue(string name)
        {
            if (SpreadsheetCells.ContainsKey(name) && SpreadsheetCells[name].Value.GetType() == typeof(Formula))
            {
                Formula cellFormula = (Formula)SpreadsheetCells[name].Value;
                return cellFormula.Evaluate(LookUp);
            }
            else
            {
                try
                {
                    return SpreadsheetCells[name].Value;
                }
                catch(KeyNotFoundException)
                {
                    return "";
                }
                
            }
        }



        /// <summary>
        /// This is the method used for lookup the variable's value and do the calculation of it
        /// It will keep search the direct dependecies and indirect dependeices values and do the 
        /// formula calculation
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private double LookUp(string name)
        {
            string rightFormatName = NormalizeAndCheckName(name);
            if (!SpreadsheetCells.ContainsKey(rightFormatName) || SpreadsheetCells[rightFormatName].Value.GetType() == typeof(string))
            {
                throw new ArgumentException("You are take a string in the calculator");
            }
            else if (SpreadsheetCells[rightFormatName].Value.GetType() == typeof(double))
            {
                return (Double)SpreadsheetCells[rightFormatName].Value;
            }
            else
            {
                Formula cellFormula = (Formula)SpreadsheetCells[name].Value;
                return (double)cellFormula.Evaluate(LookUp);
            }
        }
    }

    /// <summary>
    /// This cell class is to create a cell in a spreadsheet and Stored in it. It has basic name 
    /// and value inside it
    /// </summary>
    internal class Cell
    {
        
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

        /// <summary>
        /// This is used for create a xml file. If it is a formula it will return a string of a string of the formula and add
        /// a plus sign before the formula string form.
        /// </summary>
        /// <returns>If it is a formula it will return a add sign and a formula string, if it is string or double, it will
        /// return the normal string type of string or double.</returns>
        public string GetContentString()
        {
            if(Value.GetType() == typeof(Formula))
            {
                Formula formula = (Formula)Value;
                return "="+ formula.ToString();
            }
            else
            {
                return Value.ToString();
            }
        }
    }
}
