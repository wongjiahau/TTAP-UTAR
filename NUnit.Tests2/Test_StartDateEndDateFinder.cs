using NUnit.Framework;
using System;
using System.IO;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_StartDateEndDateFinder {
        string input = Helper.RawStringOfTestFile("SampleData-FAM-2017-2ndSem.html");

        [Test]
        public void Test_1() {
            var parser = new StartDateEndDateFinder(input);
            Assert.True(parser.GetStartDate() == new DateTime(2017 , 10 , 16 , 0 , 0 , 0));
            Assert.True(parser.GetEndDate() == new DateTime(2017 , 12 , 3 , 0 , 0 , 0));
        }

      
    }
}
