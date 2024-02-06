using SpreadsheetUtilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SS
{
    public class Spreadsheet : AbstractSpreadsheet
    {
        //TODO - when teh cell contents is "", it will be empty string
        private Dictionary<string, cell> SpreadsheetCells;
        private DependencyGraph DependencyGraph;

        public Spreadsheet()
        {
            SpreadsheetCells = new Dictionary<string, cell>();
            DependencyGraph = new DependencyGraph();
        }

        public override object GetCellContents(string name)
        {
            throw new NotImplementedException();
        }

        public override IEnumerable<string> GetNamesOfAllNonemptyCells()
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, double number)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, string text)
        {
            throw new NotImplementedException();
        }

        public override ISet<string> SetCellContents(string name, Formula formula)
        {
            throw new NotImplementedException();
        }

        protected override IEnumerable<string> GetDirectDependents(string name)
        {
            throw new NotImplementedException();
        }
    }

    internal class cell
    {
        public object Value { get; set; }

        public cell()
        {
            Value = "";
        }
    }
}
