using NUnit.Framework;
using System;
using System.IO;
using Time_Table_Arranging_Program;
using Time_Table_Arranging_Program.Class;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_StartDateEndDateFinder {
        readonly string input1 = Helper.RawStringOfTestFile("SampleData-FAM-2017-2ndSem.html");
        readonly string input2 = Helper.RawStringOfTestFile("SampleData-FCI-2017-2ndSem.html");
        readonly string input3 = Helper.RawStringOfTestFile("Sample HTML.html");

        [Test]
        public void Test_StartDateEndDateFinder_GetStartDate1() {           
            var parser = new StartDateEndDateFinder(input1);
            Assert.True(parser.GetStartDate() == new DateTime(2017 , 10 , 16 , 0 , 0 , 0));
        }

        [Test]
        public void Test_StartDateEndDateFinder_GetStartDate2()
        {
            var parser = new StartDateEndDateFinder(input2);
            Assert.True(parser.GetStartDate() == new DateTime(2017, 10, 16, 0, 0, 0));
        }

        [Test]
        public void Test_StartDateEndDateFinder_GetStartDate3()
        {
            var parser = new StartDateEndDateFinder(input3);
            Assert.True(parser.GetStartDate() == new DateTime(2017, 5, 29, 0, 0, 0));
        }

        [Test]
        public void Test_StartDateEndDateFinder_GetEndDate1()
        {
            var parser = new StartDateEndDateFinder(input1);
            Assert.True(parser.GetEndDate() == new DateTime(2017, 12, 3, 0, 0, 0));
        }

        [Test]
        public void Test_StartDateEndDateFinder_GetEndDate2()
        {
            var parser = new StartDateEndDateFinder(input2);
            Assert.True(parser.GetEndDate() == new DateTime(2017, 12, 3, 0, 0, 0));
        }

        [Test]
        public void Test_StartDateEndDateFinder_GetEndDate3()
        {
            var parser = new StartDateEndDateFinder(input3);
            Assert.True(parser.GetEndDate() == new DateTime(2017, 9, 3, 0, 0, 0));
        }
    }
}
