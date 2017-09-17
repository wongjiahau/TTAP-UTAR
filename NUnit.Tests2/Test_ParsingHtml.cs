using System;
using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class.TokenParser;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_ParsingHtml {
        [Test]
        public void Test_ParsingHtml_1() {        
            string input = Helper.RawStringOfTestFile("Sample HTML.txt");
            string plain = ExtensionMethods.RemoveTags(input);
            var actual = new SlotParser().Parse(plain);
            int expected = new SlotParser().Parse(Helper.RawStringOfTestFile("CopiedTextFromSampleHTML.txt")).Count;
            Console.WriteLine("Actual count is " + actual.Count);
            Console.WriteLine("Expected count is " + expected);
            Assert.True(actual.Count == expected);
        }
    }
}
