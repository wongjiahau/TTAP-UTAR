using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Parser;
using Time_Table_Arranging_Program.Class.TokenParser;


namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_StudentIdFinder {
        private static readonly string input =
            "Home\t\t\r\n   Log Out  \t\r\n \r\nWelcome, LOW KE LI (15UKB04769)   User Guide\r\n\r\n Course Timetable Preview\r\n\r\n\tCourse Timetable Preview\t\tMy Course Registration\t\r\n\r\n\r\nSESSION\t201705\tCLASS TYPE\tFull-time\tFACULTY\tFAM\tCAMPUS\tSungai Long Campus\tDURATION (WEEKS)\t29/05/2017 - 03/09/2017 (14)\r\n\r\nCOURSE\t\r\n  Search\r\nDAY\t";

        [Test]
        public void Test_Tokenizer_SandBox() {
            Console.WriteLine("This test is to make sure that the tokens produced by tokenizer contains no empty string and non-readable chars");
            var tokenStream = new TokenStream(Tokenizer.Tokenize(input));
            int i = 0;
            while (true) {
                byte[] asciiBytes = Encoding.ASCII.GetBytes(tokenStream.CurrentToken().Value());                
                Console.Write(i + "\t :");
                foreach (var value in asciiBytes) {
                    Console.Write(value + " ");
                }
                Console.WriteLine();
                i++;
                if (tokenStream.IsAtLastToken()) return;
                tokenStream.GoToNextToken();                
            }            
        }
        [Test]
        public void Test_StudentIdFinder_1() {
            var parser = new StudentIdFinder(input);

            Assert.AreEqual("15UKB04769" , parser.GetStudentId());
        }

        [Test]
        public void Test_StudentIdFinder_2() {
            string htmlText = Helper.RawStringOfTestFile("Sample HTML.txt");
            string plain = ExtensionMethods.RemoveTags(htmlText);
            var parser = new StudentIdFinder(plain);
            Assert.AreEqual("15UEB00181" , parser.GetStudentId());            
        }

    }

}
