using System;
using System.Collections.Generic;
using System.IO;
using HtmlAgilityPack;
using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Class.TokenParser;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_HtmlSlotParser {
        [Test]
        public void Test_HtmlSlotParser_1() {
            string input = Helper.RawStringOfTestFile("Sample HTML.txt");
            var result = new HtmlSlotParser().Parse(input);
            Test_SlotParser.TestForResultCorrectness(result);
        }

        [Test]
        public void Test_HtmlSlotParser_2() {
            string input = Helper.RawStringOfTestFile("Sample HTML.txt");
            var result = new HtmlSlotParser().Parse(input);
            var expectedUids = new HashSet<int>();
            for (int i = 1; i <= 130; i++) {
                expectedUids.Add(i);
            }
            var actualUids = new HashSet<int>();
            for (int i = 0; i < result.Count; i++) {
                actualUids.Add(result[i].UID);
            }
            Assert.IsTrue(expectedUids.SetEquals(actualUids));
        }

        [Test]
        public void Test_HtmlSlotParser_3() {
            string input = Helper.RawStringOfTestFile("Sample HTML.txt");
            var actual = new HtmlSlotParser().Parse(input);
            var expected = new List<Slot>()
            {
                new Slot(1, "MPU3113", "HUBUNGAN ETNIK (FOR LOCAL STUDENTS)".Beautify(), "1", "L", Day.Monday, "KB521",
                    new TimePeriod(Time.CreateTime_24HourFormat(9, 00), Time.CreateTime_24HourFormat(12, 00)),
                    new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(2, "MPU3113", "HUBUNGAN ETNIK (FOR LOCAL STUDENTS)".Beautify(), "2", "L", Day.Monday, "KB521",
                    new TimePeriod(Time.CreateTime_24HourFormat(14, 00), Time.CreateTime_24HourFormat(17, 00)),
                    new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(3, "MPU3113", "HUBUNGAN ETNIK (FOR LOCAL STUDENTS)".Beautify(), "3", "L", Day.Tuesday, "KB520",
                    new TimePeriod(Time.CreateTime_24HourFormat(14, 00), Time.CreateTime_24HourFormat(17, 00)),
                    new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(4, "MPU3113", "HUBUNGAN ETNIK (FOR LOCAL STUDENTS)".Beautify(), "4", "L", Day.Thursday,
                    "KB316",
                    new TimePeriod(Time.CreateTime_24HourFormat(8, 00), Time.CreateTime_24HourFormat(11, 00)),
                    new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(5, "MPU3123", "TAMADUN ISLAM DAN TAMADUN ASIA (TITAS)".Beautify(), "1", "L", Day.Monday,
                    "KB520",
                    new TimePeriod(Time.CreateTime_24HourFormat(9, 00), Time.CreateTime_24HourFormat(12, 00)),
                    new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(11, "MPU32013", "BAHASA KEBANGSAAN A".Beautify(), "1", "L", Day.Tuesday, "KB314",
                    new TimePeriod(Time.CreateTime_24HourFormat(9, 00), Time.CreateTime_24HourFormat(12, 00)),
                    new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
                new Slot(12, "MPU32033", "ENGLISH FOR PROFESSIONALS".Beautify(), "1", "L", Day.Monday, "KB301",
                    new TimePeriod(Time.CreateTime_24HourFormat(12, 00), Time.CreateTime_24HourFormat(13, 00)),
                    new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14}))
            };
            for (int i = 0; i < expected.Count; i++) {
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

            Dictionary<int, Slot> GetDictionary(List<Slot> list) {
                var result = new Dictionary<int, Slot>();
                foreach (var slot in list) {
                    result.Add(slot.UID, slot);
                }
                return result;
            }
        }
    }
}
