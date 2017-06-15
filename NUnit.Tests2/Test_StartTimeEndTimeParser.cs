using NUnit.Framework;
using System;
using Time_Table_Arranging_Program.Class;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_StartTimeEndTimeParser {
        private static string input =
     "Home\t\t\r\n   Log Out  \t\r\n \r\nWelcome, LOW KE LI (15UKB04769)   User Guide\r\n\r\n Course Timetable Preview\r\n\r\n\tCourse Timetable Preview\t\tMy Course Registration\t\r\n\r\n\r\nSESSION\t201705\tCLASS TYPE\tFull-time\tFACULTY\tFAM\tCAMPUS\tSungai Long Campus\tDURATION (WEEKS)\t29/05/2017 - 03/09/2017 (14)\r\n\r\nCOURSE\t\r\n  Search\r\nDAY\t";

        [Test]
        public void Test_1() {
            var parser = new StartDateEndDateParser(input);
            Assert.True(parser.GetStartDate() == new DateTime(2017 , 5 , 29 , 0 , 0 , 0));
            Assert.True(parser.GetEndDate() == new DateTime(2017 , 9 , 3 , 0 , 0 , 0));
        }
    }
}
