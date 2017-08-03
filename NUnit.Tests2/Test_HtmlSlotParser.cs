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
        public void Test_HtmlSlotParser_GrepLecturerName() {
            string input = "07005(LooJL), 09066(TanYQ)";
            var expected = "LooJL, TanYQ";
            var actual = HtmlSlotParser_FGO.GrepLecturerName(input);
            Assert.AreEqual(expected,actual);
        }

        [Test]
        public void Test_HtmlSlotParser_2() {
            string input = Helper.RawStringOfTestFile("FGO.html");
            var result = new HtmlSlotParser_FGO().Parse(input);
            var expectedUids = new HashSet<int>();
            int lastUid = 1660;
            for (int i = 1 ; i <= lastUid ; i++) {
                expectedUids.Add(i);
            }
            var actualUids = new HashSet<int>();
            for (int i = 0 ; i < result.Count ; i++) {
                actualUids.Add(result[i].UID);
            }
            Assert.IsTrue(expectedUids.SetEquals(actualUids));
        }       
    }
}
