using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Class.TokenParser;
using Time_Table_Arranging_Program.Model;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_SlotParser {
        [Test]
        public void Test_SlotParser_0() {
            string input =
                "MPU3113 - HUBUNGAN ETNIK (FOR LOCAL STUDENTS) [3.00]\r\n1\tL\t1\t100\tMon\t09:00 AM - 12:00 PM\t3.0\t1-14\tKB521\t\r\n2\tL\t2\t100\tMon\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB521\t\r\n3\tL\t3\t90\tTue\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB520\t\r\n4\tL\t4\t90\tThu\t08:00 AM - 11:00 AM\t3.0\t1-14\tKB316\t";
            var tp = new SlotParser();
            var result = tp.Parse(input);
            var expected = new List<Slot>()
            {
          new Slot(1,"MPU3113","HUBUNGAN ETNIK (FOR LOCAL STUDENTS)".Beautify(), "1", "L",Day.Monday, "KB521", new TimePeriod(Time.CreateTime_24HourFormat(9,00), Time.CreateTime_24HourFormat(12,00)),new WeekNumber(new List<int>(){1,2,3,4,5,6,7,8,9,10,11,12,13,14})  )
            };
            Assert.IsTrue(result.Count == 4);
            Assert.IsTrue(result[0].Equals(expected[0]));

        }


        [Test]
        public void Test_SlotParser_1() {
            string input =
                "MPU3113 - HUBUNGAN ETNIK (FOR LOCAL STUDENTS) [3.00]\r\n1\tL\t1\t100\tMon\t09:00 AM - 12:00 PM\t3.0\t1-14\tKB521\t\r\n2\tL\t2\t100\tMon\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB521\t\r\n3\tL\t3\t90\tTue\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB520\t\r\n4\tL\t4\t90\tThu\t08:00 AM - 11:00 AM\t3.0\t1-14\tKB316\t";
            var tp = new SlotParser();
            var result = tp.Parse(input);
            Assert.IsTrue(result.Count == 4);

        }

        public static string TestFilePath() {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\";
            string path =
                desktopPath + @"TTAPv7.7\NUnit.Tests2\TestFiles\CopiedTextFromSampleHTML.txt";
            return path;
        }

        [Test]
        public void Test_SlotParser_2() {
            string text = File.ReadAllText(TestFilePath());
            var result = new SlotParser().Parse(text);
            int expectedCount = 130;
            Assert.AreEqual(expectedCount , result.Count);
        }

        [Test]
        public void Test_SlotParser_3() {
            string text = File.ReadAllText(TestFilePath());
            var result = new SlotParser().Parse(text);
            var subjects = SubjectModel.Parse(result);
            int expectedCount = 39; // counted by eyes

            var c = CodesOfListedSubjects();
            Console.WriteLine("Code not foundd : ");
            foreach (var code in c) {
                if (subjects.All(x => x.Code != code))
                    Console.WriteLine(code);
            }

            Assert.AreEqual(expectedCount , subjects.Count);
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
