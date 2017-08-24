using System.Collections.Generic;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;

namespace NUnit.Tests2.TestSuite_HtmlSlotParser {
    public class TestHtmlSlotParser_SampleData_FAM_2017_2ndSem : Test_HtmlSlotParserBase {
        public TestHtmlSlotParser_SampleData_FAM_2017_2ndSem() : base("SampleData-FAM-2017-2ndSem.html") { }
        protected override List<Slot> GenerateExpectedSlots() {
            return new List<Slot>()
            {
				new Slot(1, "MPU3113", "HUBUNGAN ETNIK (FOR LOCAL STUDENTS)".Beautify(), "1", "L", Day.Monday, "KB521",new TimePeriod(Time.CreateTime_24HourFormat(9, 00), Time.CreateTime_24HourFormat(12, 00)),new WeekNumber(new List<int>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14})),
            };
        }
    }
}
