using SpreadsheetUtilities;

namespace FormulaTests
{
    [TestClass]
    public class FormulaTests
    {
        /// <summary>
        /// Specific Token Rule
        /// </summary>
        [TestMethod, Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorSyntaxError1()
        {
            Formula wrongFormat = new Formula("2&3");
        }

        /// <summary>
        /// One Token Rule
        /// </summary>
        [TestMethod, Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorSyntaxError2()
        {
            Formula wrongFormat = new Formula("");
        }

        /// <summary>
        /// Right Parentheses Rule
        /// </summary>
        [TestMethod, Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorSyntaxError3()
        {
            Formula wrongFormat = new Formula("(2))");
        }

        /// <summary>
        /// Balanced Parentheses Rule
        /// </summary>
        [TestMethod, Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorSyntaxError4()
        {
            Formula wrongFormat = new Formula("(((2))");
        }

        /// <summary>
        /// Starting Token Rule
        /// </summary>
        [TestMethod, Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorSyntaxError5()
        {
            Formula wrongFormat = new Formula("*(2)");
        }

        /// <summary>
        /// Ending Token Rule
        /// </summary>
        [TestMethod, Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorSyntaxError6()
        {
            Formula wrongFormat = new Formula("(2)^");
        }

        /// <summary>
        /// Parenthesis/Operator Following Rule
        /// </summary>
        [TestMethod, Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorSyntaxError7()
        {
            Formula wrongFormat = new Formula("=(2)");
        }

        /// <summary>
        /// Extra Following Rule
        /// </summary>
        [TestMethod, Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorSyntaxError8()
        {
            Formula wrongFormat = new Formula("9(");
        }

        /// <summary>
        /// Test all illegal variable Format
        /// </summary>
        [TestMethod, Timeout(5000)]
        public void TestConstructorSyntaxError9()
        {
            try
            {
                Formula wrongVariable1 = new Formula("ewrw");
                throw new ArgumentException("Formula Constructor Cannot Find Wrong Format");
            }
            catch (FormulaFormatException)
            {
            }

            try
            {
                Formula wrongVariable1 = new Formula("ewr3423ewrw");
                throw new ArgumentException("Formula Constructor Cannot Find Wrong Format");
            }
            catch (FormulaFormatException)
            {
            }

            try
            {
                Formula wrongVariable1 = new Formula("3453ewrw");
                throw new ArgumentException("Formula Constructor Cannot Find Wrong Format");
            }
            catch (FormulaFormatException)
            {
            }

            try
            {
                Formula wrongVariable1 = new Formula("440de9ufwjiojd");
                throw new ArgumentException("Formula Constructor Cannot Find Wrong Format");
            }
            catch (FormulaFormatException)
            {
            }
        }
        /// <summary>
        /// These are all right format and should never throw a Exception
        /// </summary>
        [TestMethod, Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestConstructorSyntaxRight()
        {
            Formula rightFormat1 = new Formula("(((((8)))))");
            Formula rightFormat2 = new Formula("a1+i2-d1*(2)/2+9-2-2/(((((w3)))))*2");
            Formula rightFormat3 = new Formula("1+22/3*4-2+y2");
            Formula rightFormat4 = new Formula("2");
            Formula rightFormat5 = new Formula("_a_1");
            Formula rightFormat6 = new Formula("qwe_123");
            Formula rightFormat7 = new Formula("qwe_123____33awe_8");
            Formula rightFormat8 = new Formula("____3");
        }

        /// <summary>
        /// Throw a exception when normalized variables are illegal!
        /// </summary>
        [TestMethod, Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestWhenNormalizeToWrong()
        {
            Formula wrongFormat = new Formula("9+y3", n => "3Y", v => true);
        }

        /// <summary>
        /// Throw a exception when normalized variables are illegal!
        /// </summary>
        [TestMethod, Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestWhenValidorIsWrong()
        {
            Formula wrongFormat = new Formula("9+y3", n => n, v => false);
        }

        /// <summary>
        /// The Normalized Result can be different with not normliazed result.
        /// </summary>
        [TestMethod, Timeout(5000)]
        public void TestIfLookUpNormalized()
        {
            Formula formula1 = new Formula("9+y3+Y3", n => n.ToUpper(), v => true);
            double result1 = (double)formula1.Evaluate(a => { if (a == "y3") { return 1000; } else { return 10000; } } );
            Assert.AreEqual(20009, result1);

            Formula formula2 = new Formula("9+y3+Y3");
            double result2 = (double)formula1.Evaluate(a => { if (a == "y3") { return 1000; } else { return 10000; } });
            Assert.AreEqual(11009, result2 );
        }

        /// <summary>
        /// This will test edge cases;
        ///     1. when lookup throws a exception
        ///     2. when there is divide by zero
        /// Those cases will all return a Formula Error
        /// </summary>
        [TestMethod, Timeout(5000)]
        public void TestEvaluateReturnError()
        {
            Formula divdeBy0 = new Formula("8/(1-1)");
            Formula normal = new Formula("d1");
            Assert.AreEqual(divdeBy0.Evaluate(s=>1).GetType(), typeof(FormulaError));
            Assert.AreEqual(normal.Evaluate(s=>{ throw new ArgumentException(); }).GetType(), typeof(FormulaError));
        }

        /// <summary>
        /// This will test Get Variables
        /// and the variables are must be normalized
        /// </summary>
        [TestMethod, Timeout(5000)]
        public void TestGetVariables()
        {
            Formula formula1 = new Formula("a1+A1+c3+d4");
            Formula formula2 = new Formula("a1+A1+c3+d4", n=>n.ToUpper(), v => true);
            HashSet<string> variables1 = new HashSet<string> {"a1", "A1", "c3", "d4"};
            HashSet<string> variables2 = new HashSet<string> {"A1", "C3", "D4"};
            
            Assert.AreEqual(formula1.GetVariables(), variables1);
            Assert.AreEqual(formula2.GetVariables(), variables2);
        }

    }
}