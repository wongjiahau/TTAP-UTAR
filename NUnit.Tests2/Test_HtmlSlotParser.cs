using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;


namespace NUnit.Tests2 {
    [TestFixture]
   public class Test_HtmlSlotParser {
        [Test]
        public void Test_HtmlSlotParser_1() {
            string input = Helper.RawStringOfTestFile("Sample HTML.txt");
            /// var result = new HtmlSlotParser.Parse(input)
            var result = new List<Slot>();
            result.AddRange(TestData.GetSlotsByName(TestData.Subjects.AdvancedStructuralSteelDesign));
            foreach (var slot in result) {
                Console.WriteLine(slot.ToFullString());
            }


        }
    }
}
