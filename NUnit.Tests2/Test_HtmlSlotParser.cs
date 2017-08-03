using System;
using System.Collections.Generic;
using System.IO;
using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Class.TokenParser;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_HtmlSlotParser_FGO {
        [Test]
        public void Test_HtmlSlotParser_ListDownResult() {
            string input = Helper.RawStringOfTestFile("FGO.html");
            var result = new HtmlSlotParser_FGO().Parse(input);
            foreach (var slot in result) {
                Console.WriteLine(slot.ToFullString());
            }

        }

        [Test]
        public void Test_HtmlSlotParser_GrepSubjectCodeAndName() {

            string input =
                "Barred List by week 5th/12th [by Hour: 0]MPU3113 - HUBUNGAN ETNIK (FOR LOCAL STUDENTS) - [View All] [437]";
            var expected = (code:"MPU3113", subjectName: "HUBUNGAN ETNIK (FOR LOCAL STUDENTS)".Beautify());
            var actual = HtmlSlotParser_FGO.GrepSubjectCodeAndName(input);
            Assert.AreEqual(expected.code,actual.code);
            Assert.AreEqual(expected.subjectName, actual.subjectName);
        }

        [Test]
        public void Test_HtmlSlotParser_2() {
            string input = Helper.RawStringOfTestFile("FGO.html");
            var result = new HtmlSlotParser_FGO().Parse(input);
            var expectedUids = new HashSet<int>();
            for (int i = 1 ; i <= 130 ; i++) {
                expectedUids.Add(i);
            }
            var actualUids = new HashSet<int>();
            for (int i = 0 ; i < result.Count ; i++) {
                actualUids.Add(result[i].UID);
            }
            Assert.IsTrue(expectedUids.SetEquals(actualUids));
        }

        [Test]
        public void Test_HtmlSlotParser_3() {
            string input = Helper.RawStringOfTestFile("FGO.html");
            var actual = new HtmlSlotParser_FGO().Parse(input);
            var expected = new List<Slot>() { };
            for (int i = 0 ; i < expected.Count ; i++) {
                if (expected[i].Equals(actual[i])) { }
                else {
                    Console.WriteLine("Error occur at Slot with UID of " + expected[i].UID);
                    Console.WriteLine("Expected is : ");
                    Console.WriteLine(expected[i].ToFullString());
                    Console.WriteLine("\n");
                    Console.WriteLine("Actual is : ");
                    Console.WriteLine(actual[i].ToFullString());
                    Console.WriteLine("--------------------------------------------");
                    Assert.Fail();
                }
            }

            Dictionary<int , Slot> GetDictionary(List<Slot> list)
            {
                var result = new Dictionary<int , Slot>();
                foreach (var slot in list) {
                    result.Add(slot.UID , slot);
                }
                return result;
            }
        }
    }
}
