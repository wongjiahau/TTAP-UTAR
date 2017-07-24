using NUnit.Framework;
using NUnit.Framework.Internal;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.TokenParser;


namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_Token {
        [TestCase("12" , ExpectedResult = true)]
        [TestCase("-12" , ExpectedResult = false)]
        [TestCase("ab12" , ExpectedResult = false)]
        public bool Test_Token_IsPositiveInteger(string s) {
            var t = new Token(s);
            return t.IsPositiveInteger();
        }

        [TestCase("12.30" , ExpectedResult = true)]
        [TestCase("-12.30" , ExpectedResult = false)]
        [TestCase("1230" , ExpectedResult = false)]
        public bool Test_Token_IsPostiveDecimalNumber(string s) {
            var t = new Token(s);
            return t.IsPositiveNumberThatContainDecimalPoint();
        }

        [TestCase("12:30" , ExpectedResult = true)]
        [TestCase("1:00" , ExpectedResult = true)]
        [TestCase("ab:12" , ExpectedResult = false)]
        [TestCase("12:" , ExpectedResult = false)]
        public bool Test_Token_IsTime(string s) {
            var t = new Token(s);
            return t.IsTime();

        }

        [TestCase("L" , ExpectedResult = true)]
        [TestCase("T" , ExpectedResult = true)]
        [TestCase("P" , ExpectedResult = true)]
        [TestCase("TT" , ExpectedResult = false)]
        public bool Test_Token_IsSlotType(string s) {
            return new Token(s).IsSlotType();
        }

        [TestCase("Mon" , ExpectedResult = true)]
        [TestCase("Tue" , ExpectedResult = true)]
        [TestCase("Wed" , ExpectedResult = true)]
        [TestCase("Thu" , ExpectedResult = true)]
        [TestCase("Fri" , ExpectedResult = true)]
        [TestCase("Sat" , ExpectedResult = true)]
        [TestCase("Sun" , ExpectedResult = true)]
        [TestCase("ZZZ" , ExpectedResult = false)]
        public bool Test_Token_IsDay(string s) {
            return new Token(s).IsDay();
        }

        [TestCase("KB203" , ExpectedResult = true)]
        [TestCase("KA302A" , ExpectedResult = true)]
        [TestCase("b1" , ExpectedResult = false)]
        public bool Test_Token_IsPossiblyVenueValue(string s) {
            return new Token(s).IsPossiblyVenueValue();
        }

        [TestCase("abc" , ExpectedResult = "abc")]
        public string Test_Token_Value(string s) {
            return new Token(s).Value();
        }
    }
}
