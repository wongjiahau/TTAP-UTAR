using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class.TokenParser;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_ParsingFGO {
        [Test]
        public void SandBox_Test() {
            var input =
                "No\tType\tGroup\tClass Size\tDay\tTime\tHour\tWeek\tRoom\tLecturer\tReg.\tAvail\tReserve\tRemark\r\nBarred List by week 5th/12th [by Hour: 0]MPU3113 - HUBUNGAN ETNIK (FOR LOCAL STUDENTS) - [View All] [437]\r\n1\tL\t1\t100\tMon\t09:00 AM - 12:00 PM\t3.0\t1-14\tKB521\t05177(Azlili)\t\r\n52\r\n48\r\n2\tL\t2\t100\tMon\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB521\t04214(ChinYM)\t\r\n72\r\n28\r\n3\tL\t3\t100\tTue\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB520\t09267(HoYC)\t\r\n95\r\n5\r\n4\tL\t4\t110\tThu\t08:00 AM - 11:00 AM\t3.0\t1-14\tKB316\t04214(ChinYM)\t\r\n110\r\n0\r\n5\tL\t5\t120\tThu\t08:00 AM - 11:00 AM\t3.0\t1-14\tKB520\t05177(Azlili)\t\r\n108";
            var tokens = Tokenizer.Tokenize(input);
            foreach (var t in tokens) {
                Console.WriteLine(t.Value());
            }
        }
    }
}
