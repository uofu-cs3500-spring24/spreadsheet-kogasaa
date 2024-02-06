using SpreadsheetUtilities;

namespace SS
{
    [TestClass]
    public class SpreadsheetTests
    {
        /// <summary>
        /// THis is a test to get non empty cells it should return a Enumberable a groups 
        /// of cells names without duplication.
        /// </summary>
        [TestMethod]
        public void TestGetNamesOfAllNonEmptyCells()
        {
            Spreadsheet Spreadsheet = new Spreadsheet();
            Spreadsheet.SetCellContents("a1",1);
            Spreadsheet.SetCellContents("a2", new Formula("10+3"));
            Spreadsheet.SetCellContents("a3", "Oh man this is a text");
            Spreadsheet.SetCellContents("a1", 2);
            IEnumerable<string> result = Spreadsheet.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(new HashSet<string> { "a1", "a2", "a3" }, result);
            Spreadsheet.SetCellContents("a1", "");
            IEnumerable<string> result2 = Spreadsheet.GetNamesOfAllNonemptyCells();
            Assert.AreEqual(new HashSet<string> { "a2", "a3" }, result);
        }

        /// <summary>
        /// Test Invalid name exception in get cell contents
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetNamesOfAllNonEmptyCellsInvalidName()
        {
            Spreadsheet Spreadsheet = new Spreadsheet();
            Spreadsheet.SetCellContents("a1", 1);
            Spreadsheet.SetCellContents("a2", new Formula("10+3"));
            Spreadsheet.SetCellContents("a3", "Oh man this is a text");
            Spreadsheet.SetCellContents("a1", 2);
            Spreadsheet.GetCellContents("999Wrong");
        }

        /// <summary>
        /// Right test for Get Contents of Cell
        /// </summary>
        [TestMethod]
        public void TestGetContentsOfCell()
        {
            Spreadsheet Spreadsheet = new Spreadsheet();
            Spreadsheet.SetCellContents("a1", 1);
            Assert.AreEqual(1.0, (double)Spreadsheet.GetCellContents("a1"));
            Spreadsheet.SetCellContents("a2", new Formula("10+3"));
            Assert.AreEqual(new Formula("10+3"), (Formula)Spreadsheet.GetCellContents("a2"));
            Spreadsheet.SetCellContents("a3", "Oh man this is a text");
            Assert.AreEqual("Oh man this is a text", (string)Spreadsheet.GetCellContents("a3"));
            Spreadsheet.SetCellContents("a1", 2);
            Assert.AreEqual(2.0, (double)Spreadsheet.GetCellContents("a1"));

            // all cells should contains a empty string initially
            Assert.AreEqual("", (string)Spreadsheet.GetCellContents("Infinite"));
        }

        /// <summary>
        /// Invalid name in set a string in a cell
        /// </summary>
        [TestMethod]
        public void TestSetStringWrong()
        {
            
        }
    }
}