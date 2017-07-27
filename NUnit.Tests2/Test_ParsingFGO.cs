using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Class.TokenParser;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_ParsingFGO {
        [Test]
        public void SandBox_Test_1() {
            var input =
                "No\tType\tGroup\tClass Size\tDay\tTime\tHour\tWeek\tRoom\tLecturer\tReg.\tAvail\tReserve\tRemark\r\nBarred List by week 5th/12th [by Hour: 0]MPU3113 - HUBUNGAN ETNIK (FOR LOCAL STUDENTS) - [View All] [437]\r\n1\tL\t1\t100\tMon\t09:00 AM - 12:00 PM\t3.0\t1-14\tKB521\t05177(Azlili)\t\r\n52\r\n48\r\n2\tL\t2\t100\tMon\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB521\t04214(ChinYM)\t\r\n72\r\n28\r\n3\tL\t3\t100\tTue\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB520\t09267(HoYC)\t\r\n95\r\n5\r\n4\tL\t4\t110\tThu\t08:00 AM - 11:00 AM\t3.0\t1-14\tKB316\t04214(ChinYM)\t\r\n110\r\n0\r\n5\tL\t5\t120\tThu\t08:00 AM - 11:00 AM\t3.0\t1-14\tKB520\t05177(Azlili)\t\r\n108";
            var tokens = Tokenizer.Tokenize(input);
            foreach (var t in tokens) {
                Console.WriteLine(t.Value());
            }
        }

        [Test]
        public void SandBox_Test_2() {
            Assert.Pass();
            var input =
                "Barred List by week 5th/12th [by Hour: 0]MPU33203 - INTRODUCTION TO LAW AND MALAYSIAN LEGAL SYSTEM - [View All] [168]\r\n52\tL\t1\t180\tWed\t04:00 PM - 07:00 PM\t3.0\t1-14\tKB209\t14252(Iqmal), 16169(ChanYE)\t\r\n168\r\n12\r\n53\tT\t1\t30\tThu\t01:00 PM - 02:30 PM\t1.5\t3,5,7,9,11,13\tKB314\t14252(Iqmal), 16169(ChanYE)\t\r\n30\r\n0\r\n54\tT\t2\t30\tThu\t01:00 PM - 02:30 PM\t1.5\t2,4,6,8,10,12\tKB314\t14252(Iqmal), 16169(ChanYE)\t\r\n30\r\n0\r\n55\tT\t3\t30\tThu\t02:30 PM - 04:00 PM\t1.5\t3,5,7,9,11,13\tKB314\t14252(Iqmal), 16169(ChanYE)\t\r\n28\r\n2\r\n56\tT\t4\t30\tThu\t02:30 PM - 04:00 PM\t1.5\t2,4,6,8,10,12\tKB314\t14252(Iqmal), 16169(ChanYE)\t\r\n29\r\n1\r\n57\tT\t5\t30\tFri\t11:00 AM - 12:30 PM\t1.5\t3,5,7,9,11,13\tKB320\t14252(Iqmal), 16169(ChanYE)\t\r\n29\r\n1\r\n58\tT\t6\t30\tFri\t11:00 AM - 12:30 PM\t1.5\t2,4,6,8,10,12\tKB320\t14252(Iqmal), 16169(ChanYE)\t\r\n22\r\n8";
            var tokens = Tokenizer.Tokenize(input);
            foreach (var t in tokens) {
                Console.WriteLine(t.Value());
            }
        }

        [Test]
        public void SandBox_Test_3() {
            Assert.Pass();
            var input = Helper.RawStringOfTestFile("FGOHtmlText.txt");
            var tokens = Tokenizer.Tokenize(input);
            for (var i = 0 ; i < tokens.Length ; i++) {
                var t = tokens[i];
                Console.WriteLine($@"[{i}] = {t.Value()}");
            }
        }

        [Test]
        public void Test_SlotParser_1() {
            string input =
                "Barred List by week 5th/12th [by Hour: 0]MPU3113 - HUBUNGAN ETNIK (FOR LOCAL STUDENTS) - [View All] [437]\r\n1\tL\t1\t100\tMon\t09:00 AM - 12:00 PM\t3.0\t1-14\tKB521\t05177(Azlili)\t\r\n52\r\n48\r\n2\tL\t2\t100\tMon\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB521\t04214(ChinYM)\t\r\n72\r\n28\r\n3\tL\t3\t100\tTue\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB520\t09267(HoYC)\t\r\n95\r\n5\r\n4\tL\t4\t110\tThu\t08:00 AM - 11:00 AM\t3.0\t1-14\tKB316\t04214(ChinYM)\t\r\n110\r\n0\r\n5\tL\t5\t120\tThu\t08:00 AM - 11:00 AM\t3.0\t1-14\tKB520\t05177(Azlili)\t\r\n108\r\n12\r\n";
            var result = new SlotParser().Parse(input);
            foreach (var slot in result) {
                Console.WriteLine(slot);
            }
            Assert.IsTrue(result.Count == 5);
            var expected = new List<Slot>()
            {
                new Slot(1, "MPU3113", "Hubungan Etnik (for Local Students) ", "1", "L", Day.Monday, "KB521",
                    new TimePeriod(Time.CreateTime_24HourFormat(09, 00), Time.CreateTime_24HourFormat(12, 00)),
                    new WeekNumber(new List<int> {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14}))
            };
            for (int i = 0 ; i < expected.Count ; i++) {
                Assert.True(result[i].Equals(expected[i]));


            }
        }

        [Test]
        public void Test_SlotParser_Part1OfTestFile() {
            var input = Helper.RawStringOfTestFile("FGOHtmlText.txt");
            input = input.Split(new string[] { "<STOP>" } , StringSplitOptions.None)[0];
            int lastUid = 477;
            var result = new SlotParser().Parse(input);
            var uidList = new Dictionary<int , bool>();
            for (int i = 0 ; i < result.Count ; i++) {
                uidList.Add(result[i].UID , true);
            }
            for (int i = 1 ; i <= 477 ; i++) {
                if (!uidList.ContainsKey(i))
                    Console.WriteLine("UID of " + i + " is not found");
            }
        }
    }
}

