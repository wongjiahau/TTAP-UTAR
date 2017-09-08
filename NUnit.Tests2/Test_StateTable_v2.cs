using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class.StateSummary;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_StateTable_v2 {
        [Test]
        public void Test_StateTable_ParseString_1() {
            string input =
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000~" +
                "00000000000000000000000000000000";
            var expected = new int[7] { 0 , 0 , 0 , 0 , 0 , 0 , 0 };
            var actual = StateTable.ParseString_AsStateOfDefinitelyOccupied(input);
            Assert.IsTrue(Enumerable.SequenceEqual(actual , expected));
        }

        [Test]
        public void Test_StateTable_ParseString_2() {
            string input =
                "00000000000000000000000000000001~" +
                "00000000000000000000000000000001~" +
                "00000000000000000000000000000001~" +
                "00000000000000000000000000000001~" +
                "00000000000000000000000000000001~" +
                "00000000000000000000000000000001~" +
                "00000000000000000000000000000001";
            var expected = new int[7] { 1 , 1 , 1 , 1 , 1 , 1 , 1 };
            var actual = StateTable.ParseString_AsStateOfDefinitelyOccupied(input);
            Assert.IsTrue(Enumerable.SequenceEqual(actual , expected));
        }
    }
}
