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
/// This is the test file for testing all the methods in spreadsheet. Including all the 
/// wrong and right situations for the set cell contents. and all the dependency 
/// related test to make sure after add many cells the dependency is still correct
/// </summary>

using SpreadsheetUtilities;
using System.Threading.Channels;
using System.Xml;

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
            Spreadsheet.SetContentsOfCell("a1", "1");
            Spreadsheet.SetContentsOfCell("a2", "=10+3");
            Spreadsheet.SetContentsOfCell("a3", "Oh man this is a text");
            Spreadsheet.SetContentsOfCell("a1", "2");
            IEnumerable<string> result = Spreadsheet.GetNamesOfAllNonemptyCells();
            Assert.IsTrue(new HashSet<string> { "a1", "a2", "a3" }.SetEquals(result));
            Spreadsheet.SetContentsOfCell("a1", "");
            IEnumerable<string> result2 = Spreadsheet.GetNamesOfAllNonemptyCells();
            Assert.IsTrue(new HashSet<string> { "a2", "a3" }.SetEquals(result2));
        }

        /// <summary>
        /// Test Invalid name exception in get cell contents
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestGetNamesOfAllNonEmptyCellsInvalidName()
        {
            Spreadsheet Spreadsheet = new Spreadsheet();
            Spreadsheet.SetContentsOfCell("a1", "1");
            Spreadsheet.SetContentsOfCell("a2", "=10+3");
            Spreadsheet.SetContentsOfCell("a3", "Oh man this is a text");
            Spreadsheet.SetContentsOfCell("a1", "2");
            Spreadsheet.GetCellContents("999Wrong");
        }

        /// <summary>
        /// Right test for Get Contents of Cell
        /// </summary>
        [TestMethod]
        public void TestGetContentsOfCell()
        {
            Spreadsheet Spreadsheet = new Spreadsheet();
            Spreadsheet.SetContentsOfCell("a1", "1");
            Assert.AreEqual(1.0, (double)Spreadsheet.GetCellContents("a1"));
            Spreadsheet.SetContentsOfCell("a2", "=10+3");
            Assert.AreEqual(new Formula("10+3"), (Formula)Spreadsheet.GetCellContents("a2"));
            Spreadsheet.SetContentsOfCell("a3", "Oh man this is a text");
            Assert.AreEqual("Oh man this is a text", (string)Spreadsheet.GetCellContents("a3"));
            Spreadsheet.SetContentsOfCell("a1", "2");
            Assert.AreEqual(2.0, (double)Spreadsheet.GetCellContents("a1"));

            
        }



        /// <summary>
        /// This name is wrong
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestNameWrong()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            Assert.AreEqual("", spreadsheet.GetCellContents("Infinite").ToString());
        }

        /// <summary>
        /// Set Double Wrong1: the name is invlaid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetDoubleWrong1()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("", "3.0");
        }

        /// <summary>
        /// Set Double Wrong2: the name is invlaid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetDoubleWrong2()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell(null, "3.0");
        }

        /// <summary>
        /// Set Double Wrong3: the name is invlaid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetDoubleWrong3()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("666", "3.0");
        }

        /// <summary>
        /// Set Double Right: the name is right and value is also right.
        /// </summary>
        [TestMethod]
        public void TestSetDoubleRight()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("a1", "1.0");
            spreadsheet.SetContentsOfCell("a2", "2.0");
            spreadsheet.SetContentsOfCell("a3", "3.0");
            Assert.AreEqual(1.0, spreadsheet.GetCellContents("a1"));
            Assert.AreEqual(2.0, spreadsheet.GetCellContents("a2"));
            Assert.AreEqual(3.0, spreadsheet.GetCellContents("a3"));
            HashSet<string> result = (HashSet<string>)spreadsheet.GetNamesOfAllNonemptyCells();
            HashSet<string> expected = new HashSet<string> { "a2", "a3", "a1"};
            Assert.IsTrue(expected.SetEquals(result));
        }


        /// <summary>
        /// Set String Wrong1: the name is invalid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetStringWrong1()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("", "w");
        }

        /// <summary>
        /// Set Double Wrong2: the name is invalid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetStringWrong2()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell(null, "what");
        }

        /// <summary>
        /// Set String Wrong3: the name is invalid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetStringWrong3()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("666", "what");
        }

        /// <summary>
        /// Set String Wrong4: the text is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestSetStringWrong4()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            string nullable = null;
            spreadsheet.SetContentsOfCell("a1", nullable);
        }

        /// <summary>
        /// Set String Right1: It will set string rightly and without any exception
        /// </summary>
        [TestMethod]
        public void TestSetStringRight1()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            HashSet<string> expectedNonNullCellNames = new HashSet<string>();
            for (int i = 0; i < 10; i++)
            {
                spreadsheet.SetContentsOfCell("a" + i, "string: " + i);
                expectedNonNullCellNames.Add("a" + i);
            }
            Assert.IsTrue(expectedNonNullCellNames.SetEquals(spreadsheet.GetNamesOfAllNonemptyCells()));
            for (int i = 0;i < 10;i++)
            {
                Assert.AreEqual("string: "+i, spreadsheet.GetCellContents("a"+i));
            }
        }

        /// <summary>
        /// Set String Right2: Test if get cell recalculate right
        /// 
        /// </summary>
        [TestMethod]
        public void TestSetStringRight2()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("a1", "=1");
            spreadsheet.SetContentsOfCell("a2", "=1+a1");
            spreadsheet.SetContentsOfCell("a3", "=2+a2");
            List<string> reCalCulatedCellNames = spreadsheet.SetContentsOfCell("a1", "Anni are you ok?").ToList<string>();
            Assert.IsTrue(reCalCulatedCellNames.SequenceEqual(new List<string>{ "a1", "a2", "a3"}));
        }



        /// <summary>
        /// Set Formula Wrong1: the name is invalid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetFormulaWrong1()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("", "=1");
        }

        /// <summary>
        /// Set Formula Wrong2: the name is invalid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetFormulaWrong2()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell(null, "=1");
        }

        /// <summary>
        /// Set Formula Wrong3: the name is invalid or null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestSetFormulaWrong3()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("666", "=2");
        }

        /// <summary>
        /// Set Formula Wrong4: the formula perameter is null
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void TestSetFormulaWrong4()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            string nullable = null;
            spreadsheet.SetContentsOfCell("a1", nullable);
        }

        /// <summary>
        /// Set Formula Wrong5: there is a circular error existing
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]
        public void TestSetFormulaWrong5()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            for (int i = 0; i < 2; i++) 
            {
                spreadsheet.SetContentsOfCell("a"+i, "="+new Formula("a" + (i + 1)).ToString());
            }
            spreadsheet.SetContentsOfCell("a2", "=a0");
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
                spreadsheet.SetContentsOfCell("a" + i, "=" + new Formula(i + "*" + i).ToString());
                expectedNonNullCellNames.Add("a" + i);
            }
            Assert.IsTrue(expectedNonNullCellNames.SetEquals((HashSet<string>)spreadsheet.GetNamesOfAllNonemptyCells()));
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(new Formula(i + "*" + i), spreadsheet.GetCellContents("a" + i));
            }
            for (int i = 0;i < 5; i++)
            {
                spreadsheet.SetContentsOfCell("a" + i, "");
                expectedNonNullCellNames.Remove("a" + i);
            }
            Assert.IsTrue(expectedNonNullCellNames.SetEquals((HashSet<string>)spreadsheet.GetNamesOfAllNonemptyCells()));
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
                spreadsheet.SetContentsOfCell("a" + i, "=" + new Formula("a" + (i + 1)).ToString());
                expectedNonNullCellNames.Add("a" + i);
            }
            Assert.IsTrue(expectedNonNullCellNames.SetEquals((HashSet<string>)spreadsheet.GetNamesOfAllNonemptyCells()));
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(new Formula("a" + (i + 1)), spreadsheet.GetCellContents("a" + i));
            }
            for (int i = 0; i < 5; i++)
            {
                spreadsheet.SetContentsOfCell("a" + i, "");
                expectedNonNullCellNames.Remove("a" + i);
            }
            Assert.IsTrue(expectedNonNullCellNames.SetEquals((HashSet<string>)spreadsheet.GetNamesOfAllNonemptyCells()));
            spreadsheet.SetContentsOfCell("a10", "=2*2*2");
            Assert.AreEqual(spreadsheet.GetCellContents("a5"), new Formula("a6"));
            spreadsheet.SetContentsOfCell("a10", "7.0");
            Assert.AreEqual(spreadsheet.GetCellContents("a6"), new Formula("a7"));
            spreadsheet.SetContentsOfCell("a10", "It is wrong but there is no formula format exception");
            Assert.AreEqual(spreadsheet.GetCellContents("a8"), new Formula("a9"));
        }



        /// <summary>
        /// Recover a formula cell to a string or a double cell
        /// </summary>
        [TestMethod]
        public void TestRecoverFormula()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            HashSet<string> expectedNonNullCellNames = new HashSet<string>();
            for (int i = 0; i < 10; i++)
            {
                spreadsheet.SetContentsOfCell("a" + i, "=" + new Formula("a" + (i + 1)).ToString());
                expectedNonNullCellNames.Add("a" + i);
            }
            HashSet<string> result1 = new HashSet<string> {"a0", "a1", "a2", "a3", "a4", "a5", "a6", "a7", "a8", "a9"};
            Assert.IsTrue(spreadsheet.SetContentsOfCell("a9", "11.0").ToHashSet().SetEquals(result1));
            Assert.IsTrue(spreadsheet.SetContentsOfCell("a9", "11.0").ToHashSet().SetEquals(result1));
            Assert.IsTrue(spreadsheet.SetContentsOfCell("a9", "=1+21").ToHashSet().SetEquals(result1));
            Assert.IsTrue(spreadsheet.SetContentsOfCell("a9", "=a11+a12").ToHashSet().SetEquals(result1));
            Assert.IsTrue(spreadsheet.SetContentsOfCell("a9", "cover Formula with string").ToHashSet().SetEquals(result1));
            Assert.IsTrue(spreadsheet.GetNamesOfAllNonemptyCells().ToHashSet().SetEquals(expectedNonNullCellNames));
        }

        /// <summary>
        /// Recover when old dependees disappear
        /// </summary>
        [TestMethod]
        public void TestRecoverFormula2()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("a1", "=" + new Formula("b1+b2+b3+b4").ToString());
            HashSet<string> expected = new HashSet<string> { "a1", "b4" };
                
            Assert.IsTrue(expected.SetEquals(spreadsheet.SetContentsOfCell("b4", "9293.9")));

            spreadsheet.SetContentsOfCell("a1", "string");
            Assert.IsTrue(new HashSet<string> {"b4"}.SetEquals(spreadsheet.SetContentsOfCell("b4", "9293.9")));

        }



        /// <summary>
        /// When SpreadSheet Constructor can not find a file
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ReadWriteWrong1()
        {
            Spreadsheet spreadsheet = new Spreadsheet("TestConFileNotMatch.xml", n=>true, n=>n, "This can not found!");
        }

        /// <summary>
        /// When SpreadSheet COnstractor version is not fit the version stored
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ReadWriteWrong2()
        {
            Spreadsheet SPWrong = new Spreadsheet(n => true, n => n, "OwO");
            SPWrong.Save("TestVersionWrong.xml");
            Spreadsheet spreadsheet = new Spreadsheet("TestVersionWrong.xml", n => true, n => n, "O^O");
        }

        /// <summary>
        /// When this is a file with no word inside it
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ReadWriteWrong3()
        {
            using (File.Create("Nothing.xml")) 
            { 
                // I learn how to create a empty file from stack over flow
            }
            Spreadsheet spreadsheet = new Spreadsheet("Nothing.xml", n => true, n => n, "O^O");
        }

        /// <summary>
        /// When it is a xml file but it has no Version element inside it
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ReadWriteWrong4()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            using (XmlWriter writer = XmlWriter.Create("NoVersionInside.xml", settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("SpreadsheetOOOO");
                writer.WriteEndElement();
                writer.WriteEndDocument();

            }
            Spreadsheet spreadsheet = new Spreadsheet("NoVersionInside.xml", n => true, n => n, "O^O");
        }

        /// <summary>
        /// When it is a xml file but it has wrong cell name inside it
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ReadWriteWrong5()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            using (XmlWriter writer = XmlWriter.Create("wrong2.xml", settings))
            {


                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");

                writer.WriteAttributeString("version", "wrong2");


                for (int i = 0; i < 10; i++)
                {
                    writer.WriteStartElement("cell");
                    writer.WriteElementString("name", i + " wrong2");
                    writer.WriteElementString("contents", "wrong2");
                    writer.WriteEndElement();
                    writer.Flush();
                }


                writer.WriteEndElement();
                writer.WriteEndDocument();

            }
            Spreadsheet spreadsheet = new Spreadsheet("wrong2.xml", n => true, n => n, "wrong2");
        }

        /// <summary>
        /// When it is a xml file but it has wrong formula inside it
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ReadWriteWrong6()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            using (XmlWriter writer = XmlWriter.Create("wrong3.xml", settings))
            {


                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");

                writer.WriteAttributeString("version", "wrong3");


                for (int i = 0; i < 10; i++)
                {
                    writer.WriteStartElement("cell");
                    writer.WriteElementString("name", "wrongThree"+ i);
                    writer.WriteElementString("content", "=q+sksaldkfjals");
                    writer.WriteEndElement();
                    writer.Flush();
                }


                writer.WriteEndElement();
                writer.WriteEndDocument();

            }
            Spreadsheet spreadsheet = new Spreadsheet("wrong3.xml", n => true, n => n, "wrong3");
        }

        /// <summary>
        /// When it is a xml file but it has circular dependency inside it
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void ReadWriteWrong7()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            using (XmlWriter writer = XmlWriter.Create("wrong4.xml", settings))
            {


                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");

                writer.WriteAttributeString("version", "wrong4");


                for (int i = 0; i < 10; i++)
                {
                    writer.WriteStartElement("cell");
                    writer.WriteElementString("name", "wrongFour" + i);
                    writer.WriteElementString("contents", "=wrongFour"+i+"+wrongFour"+(i+1));
                    writer.WriteEndElement();
                    writer.Flush();
                }
                writer.WriteStartElement("cell");
                writer.WriteElementString("name", "wrongFour" + 10);
                writer.WriteElementString("contents", "=wrongFour" + 0);
                writer.WriteEndElement();
                writer.Flush();


                writer.WriteEndElement();
                writer.WriteEndDocument();

            }
            Spreadsheet spreadsheet = new Spreadsheet("wrong4.xml", n => true, n => n, "wrong4");
        }

        /// <summary>
        /// create a xml file
        /// </summary>
        [TestMethod]
        public void WriteAXML1()
        {
            Spreadsheet spreadsheet = new Spreadsheet(n => true, n => n, "TheSpecificVersion");
            spreadsheet.SetContentsOfCell("a1", "34");
            spreadsheet.SetContentsOfCell("a2", "=a1+a3");
            spreadsheet.SetContentsOfCell("a4", "stringgggg");
            spreadsheet.Save("TestWrite1.xml");
        }

        /// <summary>
        /// When it is a xml file but it has formula with wrong variable inside it
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void formulaVariableWrong1()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";

            using (XmlWriter writer = XmlWriter.Create("wrongVar1.xml", settings))
            {


                writer.WriteStartDocument();
                writer.WriteStartElement("spreadsheet");

                writer.WriteAttributeString("version", "wrongVar1");


                for (int i = 0; i < 10; i++)
                {
                    writer.WriteStartElement("cell");
                    writer.WriteElementString("name", "wrongVar" + i);
                    writer.WriteElementString("contents", "=wrongVar_1");
                    writer.WriteEndElement();
                    writer.Flush();
                }


                writer.WriteEndElement();
                writer.WriteEndDocument();

            }
            Spreadsheet spreadsheet = new Spreadsheet("wrongVar1.xml", n => true, n => n, "wrongVar1");
        }

        /// <summary>
        /// When create a formula with wrong variable
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void formulaVariableWrong2()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("a2", "=w1+w_2");
        }

        /// <summary>
        /// Is Valid and Normalized worked?
        /// </summary>
        [TestMethod]
        public void formulaVariableWrong3()
        {
            Spreadsheet spreadsheet1 = new Spreadsheet(n => true, n => n.ToUpper(), "Upper");
            Spreadsheet spreadsheet2 = new Spreadsheet();
            spreadsheet1.SetContentsOfCell("a1", "10");
            spreadsheet1.SetContentsOfCell("A1", "100");
            spreadsheet1.SetContentsOfCell("a3", "=a1+A1");

            spreadsheet2.SetContentsOfCell("a1", "10");
            spreadsheet2.SetContentsOfCell("A1", "100");
            spreadsheet2.SetContentsOfCell("a3", "=a1+A1");

            Assert.AreEqual(200.0, spreadsheet1.GetCellValue("A3"));
            Assert.AreEqual(110.0, spreadsheet2.GetCellValue("a3"));
        }


        /// <summary>
        /// Test GetVersion
        /// </summary>
        [TestMethod]
        public void GetVersion1()
        {
            Spreadsheet spreadsheet = new Spreadsheet(n => true, n => n, "TheSpecificVersion");
            spreadsheet.SetContentsOfCell("a1", "34");
            spreadsheet.SetContentsOfCell("a2", "=a1+a3");
            spreadsheet.SetContentsOfCell("a4", "stringgggg");
            spreadsheet.Save("TestWrite1.xml");
            Assert.AreEqual("TheSpecificVersion", spreadsheet.GetSavedVersion("TestWrite1.xml"));
        }

        /// <summary>
        /// Test GetVersion
        /// </summary>
        [TestMethod]
        public void testGetCellValue1()
        {
            Spreadsheet spreadsheet = new Spreadsheet(n => true, n => n, "TheSpecificVersion");
            spreadsheet.SetContentsOfCell("a1", "34");
            spreadsheet.SetContentsOfCell("a2", "=a1+a3");
            spreadsheet.SetContentsOfCell("a6", "=a1+34");
            spreadsheet.SetContentsOfCell("a4", "stringgggg");
            Assert.AreEqual("stringgggg", spreadsheet.GetCellValue("a4"));
            Assert.AreEqual(34.0, spreadsheet.GetCellValue("a1"));
            Assert.IsTrue(spreadsheet.GetCellValue("a2").GetType() == typeof(FormulaError));
            Assert.AreEqual(68.0, spreadsheet.GetCellValue("a6"));

            try
            {
                spreadsheet.SetContentsOfCell("b1", "=b2");
                spreadsheet.SetContentsOfCell("b2", "=b3");
                spreadsheet.SetContentsOfCell("b3", "=b1");
            }
            catch (CircularException e)
            {
                //they should be "" because there is a circular expectation
                Assert.IsTrue(spreadsheet.GetCellValue("b1").GetType() == typeof(FormulaError));
                Assert.IsTrue(spreadsheet.GetCellValue("b2").GetType() == typeof(FormulaError));
                Assert.AreEqual("", spreadsheet.GetCellValue("b3"));
            }
        }


        /// <summary>
        /// Test ReadFileConstructor
        /// </summary>
        [TestMethod]
        public void testReadFileConstructor1()
        {
            Spreadsheet spreadsheet = new Spreadsheet("TestWrite1.xml",n => true, n => n, "TheSpecificVersion");
            Assert.AreEqual(34.0, spreadsheet.GetCellValue("a1"));
            Assert.AreEqual(34.0, spreadsheet.GetCellContents("a1"));
            Assert.IsTrue(spreadsheet.GetCellValue("a2").GetType() == typeof(FormulaError));
            Assert.AreEqual(new Formula("a1+a3"), spreadsheet.GetCellContents("a2"));
            Assert.AreEqual("stringgggg", spreadsheet.GetCellValue("a4"));
            Assert.AreEqual("stringgggg", spreadsheet.GetCellContents("a4"));
        }

        /// <summary>
        /// Test SaveAndThrowAException
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(SpreadsheetReadWriteException))]
        public void testWriteAndSaveWrong()
        {
            Spreadsheet spreadsheet = new Spreadsheet( n => true, n => n, "TheSpecificVersion");
            spreadsheet.Save("");
        }

        /*/// <summary>
        /// Test GetXML()
        /// </summary>
        [TestMethod]
        public void testGetXML()
        {
            Spreadsheet spreadsheet = new Spreadsheet("TestWrite1.xml", n => true, n => n, "TheSpecificVersion");
            string expectedXML = "<?xml version=\"1.0\" encoding=\"utf-16\"?>\r\n<spreadsheet version=\"TheSpecificVersion\">\r\n  <cell>\r\n    <name>a1</name>\r\n    <contents>34</contents>\r\n  </cell>\r\n  <cell>\r\n    <name>a2</name>\r\n    <contents>=a1+a3</contents>\r\n  </cell>\r\n  <cell>\r\n    <name>a4</name>\r\n    <contents>stringgggg</contents>\r\n  </cell>\r\n</spreadsheet>";
            Assert.AreEqual(expectedXML, spreadsheet.GetXML());
        }*/

        /// <summary>
        /// test Get changed
        /// </summary>
        [TestMethod]
        public void testRightSituation()
        {
            Spreadsheet spreadsheet1 = new Spreadsheet();
            Assert.IsFalse(spreadsheet1.Changed);
            spreadsheet1.Save("testChanged1.xml");
            Assert.IsFalse(spreadsheet1.Changed);
            Spreadsheet spreadsheet2 = new Spreadsheet("testChanged1.xml", n=> true, n => n, "default");
            Assert.IsFalse(spreadsheet2.Changed);
            spreadsheet1.SetContentsOfCell("a1", "changed, oh yeah!");
            Assert.IsTrue(spreadsheet1.Changed);
            spreadsheet1.Save("testChanged1.xml");
            Spreadsheet spreadsheet3 = new Spreadsheet("testChanged1.xml", n => true, n => n, "default");
            Assert.IsFalse(spreadsheet3.Changed);
        }


        /// <summary>
        /// test Set cell a formula to get 100% code coverage
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(CircularException))]

        public void testSetFormulaBadCase()
        {
            Spreadsheet spreadsheet = new Spreadsheet();
            spreadsheet.SetContentsOfCell("a1", "=a2");
            spreadsheet.SetContentsOfCell("a1", "=a1");
        }

        // <summary>
        /// test Get changed
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidNameException))]
        public void TestWhenDoesNOtPassInvalidor()
        {
            Spreadsheet spreadsheet = new Spreadsheet(n => { if (n == "a1") { return false; } return true;  }, n => n, "ValiderTest");
            spreadsheet.SetContentsOfCell("a1", "1");
        }
    }
}