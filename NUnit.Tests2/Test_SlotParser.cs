using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Class.TokenParser;
using Time_Table_Arranging_Program.Model;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_SlotParser {
        private List<SubjectModel> Input() {
            string raw = Helper.RawStringOfTestFile("CopiedTextFromSampleHTML.txt");
            var result = new SlotParser().Parse(raw);
            var subjects = SubjectModel.Parse(result);
            return subjects;
        }
        [Test]
        public void Test_SlotParser_0() {
            string input =
                "MPU3113 - HUBUNGAN ETNIK (FOR LOCAL STUDENTS) [3.00]\r\n1\tL\t1\t100\tMon\t09:00 AM - 12:00 PM\t3.0\t1-14\tKB521\t\r\n2\tL\t2\t100\tMon\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB521\t\r\n3\tL\t3\t90\tTue\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB520\t\r\n4\tL\t4\t90\tThu\t08:00 AM - 11:00 AM\t3.0\t1-14\tKB316\t";
            var tp = new SlotParser();
            var result = tp.Parse(input);
            var expected = new List<Slot>()
            {
          new Slot(1,"MPU3113","HUBUNGAN ETNIK (FOR LOCAL STUDENTS)".Beautify(), "1", "L",Day.Monday, "KB521", new TimePeriod(Time.CreateTime_24HourFormat(9,00), Time.CreateTime_24HourFormat(12,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(2,"MPU3113","HUBUNGAN ETNIK (FOR LOCAL STUDENTS)".Beautify(), "2", "L",Day.Monday, "KB521", new TimePeriod(Time.CreateTime_24HourFormat(14,00), Time.CreateTime_24HourFormat(17,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  ),
                new Slot(2,"MPU3113","HUBUNGAN ETNIK (FOR LOCAL STUDENTS)".Beautify(), "3", "L",Day.Tuesday, "KB520", new TimePeriod(Time.CreateTime_24HourFormat(14,00), Time.CreateTime_24HourFormat(17,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  )
            };
            Assert.IsTrue(result.Count == 4);
            Assert.IsTrue(result[0].Equals(expected[0]));
            Assert.IsTrue(result[1].Equals(expected[1]));
            Assert.IsTrue(result[2].Equals(expected[2]));

        }


        [Test]
        public void Test_SlotParser_1() {
            string input =
                "MPU3113 - HUBUNGAN ETNIK (FOR LOCAL STUDENTS) [3.00]\r\n1\tL\t1\t100\tMon\t09:00 AM - 12:00 PM\t3.0\t1-14\tKB521\t\r\n2\tL\t2\t100\tMon\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB521\t\r\n3\tL\t3\t90\tTue\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB520\t\r\n4\tL\t4\t90\tThu\t08:00 AM - 11:00 AM\t3.0\t1-14\tKB316\t";
            var tp = new SlotParser();
            var result = tp.Parse(input);
            Assert.IsTrue(result.Count == 4);

        }

        private class CodeAndCount {
            public string Code;
            public int SlotCount;

            public CodeAndCount(string code , int slotCount) {
                Code = code;
                SlotCount = slotCount;
            }
        }

        [Test]
        public void Test_SlotParser_3() {
            var subjects = Input();
            int numberOfIrrelevantSubjects = 3; //e.g. Study tours
            int expectedCount = 39 - numberOfIrrelevantSubjects; // counted by eyes
            Assert.AreEqual(expectedCount , subjects.Count);
        }

        [Test]
        public void Test_SlotParser_4() {
            var input = Input();
            var c = CodesOfListedSubjects();
            string[] CodeOfIrrelevantSujects = new string[] { "MPU34042" , "MPU34062" , "UECS3596" };
            Assert.IsTrue(!input.Any(x => CodeOfIrrelevantSujects.Any(y => y == x.Code)));
        }

        [Test]
        public void Test_SlotParser_5() {
            //the following expected result is obtained using pure eye sight
            List<CodeAndCount> CodeOfRelevantSubjectsAndTheirCorrespondingSlotCount = new List<CodeAndCount>();
            var a = CodeOfRelevantSubjectsAndTheirCorrespondingSlotCount;
            a.Add(new CodeAndCount("MPU3113" , 4));
            a.Add(new CodeAndCount("MPU3123" , 4));
            a.Add(new CodeAndCount("MPU3143" , 1));
            a.Add(new CodeAndCount("MPU3173" , 1));
            a.Add(new CodeAndCount("MPU32013" , 1));
            a.Add(new CodeAndCount("MPU32033" , 7));
            a.Add(new CodeAndCount("MPU34012" , 1));
            a.Add(new CodeAndCount("MPU34012" , 1));
            a.Add(new CodeAndCount("MPU34072" , 1));
            a.Add(new CodeAndCount("UALB1003" , 6));
            a.Add(new CodeAndCount("UALE1083" , 6));
            a.Add(new CodeAndCount("UALF1003" , 6));
            a.Add(new CodeAndCount("UALJ2013" , 3));
            a.Add(new CodeAndCount("UALL1063" , 3));
            a.Add(new CodeAndCount("UALL3033" , 3));
            a.Add(new CodeAndCount("UBMM1013" , 5));
            a.Add(new CodeAndCount("UECS1004" , 3));
            a.Add(new CodeAndCount("UECS1013" , 5));
            a.Add(new CodeAndCount("UECS1044" , 3));
            a.Add(new CodeAndCount("UECS1313" , 4));
            a.Add(new CodeAndCount("UECS2033" , 5));
            a.Add(new CodeAndCount("UECS2083" , 4));
            a.Add(new CodeAndCount("UECS2103" , 5));
            a.Add(new CodeAndCount("UECS2333" , 5));
            a.Add(new CodeAndCount("UECS2363" , 4));
            a.Add(new CodeAndCount("UECS2373" , 3));
            a.Add(new CodeAndCount("UECS3203" , 3));
            a.Add(new CodeAndCount("UECS3253" , 3));
            a.Add(new CodeAndCount("UECS3263" , 3));
            a.Add(new CodeAndCount("UECS3273" , 3));
            a.Add(new CodeAndCount("UECS3583" , 1));
            a.Add(new CodeAndCount("UEEN2013" , 5));
            a.Add(new CodeAndCount("UEEN3123" , 3));
            a.Add(new CodeAndCount("UJLL1093" , 3));
            a.Add(new CodeAndCount("UKMM1011" , 3));
            a.Add(new CodeAndCount("UKMM1043" , 8));


            var subjects = Input();
            foreach (var c in CodeOfRelevantSubjectsAndTheirCorrespondingSlotCount) {
                bool somethingFound = false;
                foreach (var s in subjects) {
                    if (s.Code == c.Code) {
                        somethingFound = true;
                        if (s.Slots.Count != c.SlotCount)
                            Assert.Fail($"Expect {c.Code} have {c.SlotCount} slots but actual is {s.Slots.Count} slots");
                        break;
                    }
                }
                if (!somethingFound) Assert.Fail($"{c.Code} is not found!");
            }
        }
        private static string[] CodesOfListedSubjects() {
            string[] codesOfListedSubjects = new[]
            {
                "MPU3113",
                "MPU3123",
                "MPU3143",
                "MPU3173",
                "MPU32013",
                "MPU32033",
                "MPU34012",
                "MPU34022",
                "MPU34042",
                "MPU34062",
                "MPU34072",
                "UALB1003",
                "UALE1083",
                "UALF1003",
                "UALJ2013",
                "UALL1063",
                "UALL3033",
                "UBMM1013",
                "UECS1004",
                "UECS1013",
                "UECS1044",
                "UECS1313",
                "UECS2033",
                "UECS2083",
                "UECS2103",
                "UECS2333",
                "UECS2363",
                "UECS2373",
                "UECS3203",
                "UECS3253",
                "UECS3263",
                "UECS3273",
                "UECS3583",
                "UECS3596",
                "UEEN2013",
                "UEEN3123",
                "UJLL1093",
                "UKMM1011",
                "UKMM1043",
            };
            return codesOfListedSubjects;
        }
    }
}

