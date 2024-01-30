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
            Formula rightFormat5 = new Formula("variable,and_this_is_super_long");
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

    }
}