using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class.TokenParser;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_ParsingHtml {
        [Test]
        public void Test_ParsingHtml_1() {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
            string path =
                desktopPath + @"TTAPv7.7\NUnit.Tests2\TestFiles\Sample HTML.txt";
            string htmlText = File.ReadAllText(path);
            string plain = ExtensionMethods.RemoveTags(htmlText);
            var actual = new SlotParser().Parse(plain);

            int expected = new SlotParser().Parse(File.ReadAllText(Test_SlotParser.TestFilePath())).Count;
            Console.WriteLine("Actual count is " + actual.Count);
            Console.WriteLine("Expected count is " + expected);
            Assert.True(actual.Count == expected);

        }
    }
}
