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
            Spreadsheet.SetCellContents("a1", 1);
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
        /// Set Double Wrong1: the name is invlaid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetDoubleWrong1()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("", 3.0);
        }

        /// <summary>
        /// Set Double Wrong2: the name is invlaid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetDoubleWrong2()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents(null, 3.0);
        }

        /// <summary>
        /// Set Double Wrong3: the name is invlaid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetDoubleWrong3()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("666", 3.0);
        }

        /// <summary>
        /// Set Double Right: the name is right and value is also right.
        /// </summary>
        [TestMethod]
        public void TestSetDoubleRight()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("a1", 1.0);
            spreadsheet.SetCellContents("a2", 2.0);
            spreadsheet.SetCellContents("a3", 3.0);
            Assert.AreEqual(1.0, spreadsheet.GetCellContents("a1"));
            Assert.AreEqual(2.0, spreadsheet.GetCellContents("a2"));
            Assert.AreEqual(3.0, spreadsheet.GetCellContents("a3"));
            HashSet<string> result = (HashSet<string>)spreadsheet.GetNamesOfAllNonemptyCells();
            HashSet<string> expected = new HashSet<string> { "a2", "a3", "a1"};
            Assert.AreEqual(expected, result);
        }


        /// <summary>
        /// Set String Wrong1: the name is invalid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetStringWrong1()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("", "w");
        }

        /// <summary>
        /// Set Double Wrong2: the name is invalid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetStringWrong2()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents(null, "what");
        }

        /// <summary>
        /// Set String Wrong3: the name is invalid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetStringWrong3()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("666", "what");
        }

        /// <summary>
        /// Set String Wrong4: the text is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetStringWrong4()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            string nullable = null;
            spreadsheet.SetCellContents("a1", nullable);
        }

        /// <summary>
        /// Set String Right: It will set string rightly and without any exception
        /// </summary>
        [TestMethod]
        public void TestSetStringRight()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            HashSet<string> expectedNonNullCellNames = new HashSet<string>();
            for (int i = 0; i < 10; i++)
            {
                spreadsheet.SetCellContents("a" + i, "string: " + i);
                expectedNonNullCellNames.Add("a" + i);
            }
            Assert.AreEqual(expectedNonNullCellNames, (HashSet<string>)spreadsheet.GetNamesOfAllNonemptyCells());
            for (int i = 0;i < 10;i++)
            {
                Assert.AreEqual("string "+i, spreadsheet.GetCellContents("a"+i));
            }
        }



        /// <summary>
        /// Set Formula Wrong1: the name is invalid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetFormulaWrong1()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("", new Formula("1"));
        }

        /// <summary>
        /// Set Formula Wrong2: the name is invalid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetFormulaWrong2()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents(null, new Formula("1"));
        }

        /// <summary>
        /// Set Formula Wrong3: the name is invalid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetFormulaWrong3()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetCellContents("666", new Formula("2"));
        }

        /// <summary>
        /// Set Formula Wrong4: the formula perameter is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TestSetFormulaWrong4()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            Formula nullable = null;
            spreadsheet.SetCellContents("a1", nullable);
        }

        /// <summary>
        /// Set Formula Wrong5: there is a circular error existing
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetFormulaWrong5()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            for (int i = 0; i < 50; i++) 
            {
                spreadsheet.SetCellContents("a"+i, new Formula("a" + i + 1));
            }
            spreadsheet.SetCellContents("a51", new Formula("a1"));
        }

        /// <summary>
        /// Set String Right Simple: It will set string rightly and without any exception
        /// </summary>
        [TestMethod]
        public void TestSetFormulaRight()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            HashSet<string> expectedNonNullCellNames = new HashSet<string>();
            for (int i = 0; i < 10; i++)
            {
                spreadsheet.SetCellContents("a" + i, new Formula(i+"*"+i));
                expectedNonNullCellNames.Add("a" + i);
            }
            Assert.AreEqual(expectedNonNullCellNames, (HashSet<string>)spreadsheet.GetNamesOfAllNonemptyCells());
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual((double)i*i, spreadsheet.GetCellContents("a" + i));
            }
            for (int i = 0;i < 5; i++)
            {
                spreadsheet.SetCellContents("a" + i, "");
                expectedNonNullCellNames.Remove("a" + i);
            }
            Assert.AreEqual(expectedNonNullCellNames, (HashSet<string>)spreadsheet.GetNamesOfAllNonemptyCells());
        }

        /// <summary>
        /// Set String Right Circular: It will set Cell formula rightly and without any exception
        /// </summary>
        [TestMethod]
        public void TestSetFormulaRightCircular()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            HashSet<string> expectedNonNullCellNames = new HashSet<string>();
            for (int i = 0; i < 10; i++)
            {
                spreadsheet.SetCellContents("a" + i, new Formula("a" + i));
                expectedNonNullCellNames.Add("a" + i);
            }
            Assert.AreEqual(expectedNonNullCellNames, (HashSet<string>)spreadsheet.GetNamesOfAllNonemptyCells());
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual((double)i * i, spreadsheet.GetCellContents("a" + i));
            }
            for (int i = 0; i < 5; i++)
            {
                spreadsheet.SetCellContents("a" + i, "");
                expectedNonNullCellNames.Remove("a" + i);
            }
            Assert.AreEqual(expectedNonNullCellNames, (HashSet<string>)spreadsheet.GetNamesOfAllNonemptyCells());
            spreadsheet.SetCellContents("a10", new Formula("2*2*2"));
            Assert.AreEqual(spreadsheet.GetCellContents("a5"), new Formula("a6"));
            spreadsheet.SetCellContents("a10", 7.0);
            Assert.AreEqual(spreadsheet.GetCellContents("a6"), new Formula("a7"));
            spreadsheet.SetCellContents("a10", "It is wrong but there is no formula format exception");
            Assert.AreEqual(spreadsheet.GetCellContents("a8"), new Formula("a9"));
        }


        //TODO : to create a test to test get direct dependents
    }
}