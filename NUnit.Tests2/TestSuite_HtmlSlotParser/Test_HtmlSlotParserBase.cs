using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.TokenParser;

namespace NUnit.Tests2.TestSuite_HtmlSlotParser {
    public abstract class Test_HtmlSlotParserBase {
        private List<Slot> _actualResult;
        private List<Slot> _expectedResult;
        private string _resourceName;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resourceName">Resource name can only be from test files</param>
        protected Test_HtmlSlotParserBase(string resourceName) {
            string html = Helper.RawStringOfTestFile(resourceName);
            _actualResult = new HtmlSlotParser().Parse(html);
            _expectedResult = GenerateExpectedSlots();
            _resourceName = resourceName;
        }

        protected abstract List<Slot> GenerateExpectedSlots();

        public void Run() {
            AssertCountAreEqual();
            AssertValueAreEqual();
        }

        private void AssertValueAreEqual() {
            for (int i = 0 ; i < _actualResult.Count ; i++) {
                if (_expectedResult[i].Equals(_actualResult[i])) { }
                else {
                    Console.WriteLine(@"Error at file : " + _resourceName);
                    Console.WriteLine("Error occur at Slot with UID of " + _expectedResult[i].UID);
                    Console.WriteLine("Expected is : ");
                    Console.WriteLine(_expectedResult[i].ToFullString());
                    Console.WriteLine("\n");
                    Console.WriteLine("_actualResult is : ");
                    Console.WriteLine(_actualResult[i].ToFullString());
                    Console.WriteLine("--------------------------------------------");
                    Assert.Fail();
                }
            }
        }

        private void AssertCountAreEqual() {
            if (_actualResult.Count != _expectedResult.Count) {
                Console.WriteLine(@"Error at file : " + _resourceName);
                Console.WriteLine($"Expected count is {_expectedResult.Count} but _actualResult count is {_actualResult.Count} ");
                Assert.Fail();
            }
        }
    }
}
