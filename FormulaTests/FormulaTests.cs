
using SpreadsheetUtilities;
using System.Runtime.CompilerServices;

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
        /// Test all illegal variable Format, but according to piazza a234wswer are legal :(, ask TA to make sure
        /// </summary>
        /*[TestMethod, Timeout(5000)]
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
        }*/
        

        /// <summary>
        /// These are all right format and should never throw a Exception
        /// </summary>
        [TestMethod, Timeout(5000)]
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
        /// when normalized variables are illegal!
        /// </summary>
        [TestMethod, Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestWhenNormalizeToWrongFormat()
        {
            Formula wrongFormat = new Formula("9+y3", n => "3%^&Y", v => true);
        }

        /// <summary>
        /// when normalized throw a exception!
        /// </summary>
        [TestMethod, Timeout(5000)]
        [ExpectedException(typeof(FormulaFormatException))]
        public void TestWhenNormalizeGoWrong()
        {
            Formula wrongFormat = new Formula("9+y3", n => { throw new ArgumentException("I am wrong!!"); }, v => true);
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
        ///     3. when the formula is null
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

        /// <summary>
        /// Test If it will get the right string
        /// </summary>
        [TestMethod, Timeout(5000)]
        public void TestToString()
        {
            Formula formula1 = new Formula("a1  +A1 +  c3+d4  ");
            Formula formula2 = new Formula("  a1+A1  +c3+  d4  ", n => n.ToUpper(), v => true);
            string string1 = "a1+A1+c3+d4";
            string string2 = "A1+A1+C3+D3";

            Assert.AreEqual(formula1.ToString(), string1);
            Assert.AreEqual(formula2.ToString(), string2);
        }

        /// <summary>
        /// Test Equals Method when is not equals:
        ///     1. when the obj is not a formula type
        ///     2. when just not same when normalized
        /// </summary>
        [TestMethod, Timeout(5000)]
        public void TestEqualsWhenFalse()
        {
            string notAFormula = "I am not a Formula, return false!";
            Formula formula1 = new Formula("a1+A1+c3+d4");
            Formula formula2 = new Formula("a1+A1+c3+d4", n => n.ToUpper(), v => true);

            Assert.IsFalse(formula1.Equals(notAFormula));
            Assert.IsFalse(formula1.Equals(formula2));
        }

        /// <summary>
        /// Test Equals Method when equals:
        /// when same after normalized
        /// </summary>
        [TestMethod, Timeout(5000)]
        public void TestEqualsWhenTrue()
        {
            Formula formula1 = new Formula("a1+  a1+  c3+d4");
            Formula formula2 = new Formula("a1+A1  +c3+  D4", n => n.ToLower(), v => true);
            Assert.IsTrue(formula1.Equals(formula2));
        }

        /// <summary>
        /// Test Equals Sign when using "=="
        /// </summary>
        [TestMethod, Timeout(5000)]
        public void TestEqualSign()
        {
            Formula formula1 = new Formula("a1+  a1+  c3+d4");
            Formula formula2 = new Formula("a1+A1  +c3+  D4", n => n.ToLower(), v => true);
            Formula formula3 = new Formula("a1+A1+c3+d4", n => n.ToUpper(), v => true);
            Assert.IsTrue(formula1==formula2);
            Assert.IsFalse(formula1==formula3);
        }

        /// <summary>
        /// Test UnEquals Sign when using "!="
        /// </summary>
        [TestMethod, Timeout(5000)]
        public void TestUnEqualSign()
        {
            Formula formula1 = new Formula("a1+  a1+  c3+d4");
            Formula formula2 = new Formula("a1+A1  +c3+  D4", n => n.ToLower(), v => true);
            Formula formula3 = new Formula("a1+A1+c3+d4", n => n.ToUpper(), v => true);
            Assert.IsFalse(formula1 != formula2);
            Assert.IsTrue(formula1 != formula3);
        }

        /// <summary>
        /// Test UnEquals Sign when using "!="
        /// </summary>
        [TestMethod, Timeout(5000)]
        public void TestHashCode()
        {
            Formula formula1 = new Formula("a1+a1+c3+d4");
            Formula formula2 = new Formula("a1+A1  +c3+  D4", n => n.ToLower(), v => true);
            Formula formula3 = new Formula("a1+A1+c3+d4", n => n.ToUpper(), v => true);
            Assert.IsTrue(formula1.GetHashCode() == formula2.GetHashCode());
            Assert.IsFalse(formula1.GetHashCode() == formula3.GetHashCode());
        }

    }
}