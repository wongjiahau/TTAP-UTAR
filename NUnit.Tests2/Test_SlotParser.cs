using NUnit.Framework;
using Time_Table_Arranging_Program.Class.TokenParser;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_SlotParser {
        [Test]
        public void Test_SlotParser_1() {
            string input =
                "MPU3113 - HUBUNGAN ETNIK (FOR LOCAL STUDENTS) [3.00]\r\n1\tL\t1\t100\tMon\t09:00 AM - 12:00 PM\t3.0\t1-14\tKB521\t\r\n2\tL\t2\t100\tMon\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB521\t\r\n3\tL\t3\t90\tTue\t02:00 PM - 05:00 PM\t3.0\t1-14\tKB520\t\r\n4\tL\t4\t90\tThu\t08:00 AM - 11:00 AM\t3.0\t1-14\tKB316\t";
            var tp = new SlotParser();
            var result = tp.Parse(input);
            Assert.IsTrue(result.Count == 4);

        }
    }
}
