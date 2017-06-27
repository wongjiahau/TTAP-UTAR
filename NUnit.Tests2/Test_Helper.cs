using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using Time_Table_Arranging_Program.Class.Helper;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_Helper {
        [Test]
        public void Test_BitArray() {
            var i = new BitArray(14);
            i[0] = true;
            i[2] = true;
            i[3] = true;

            int result = i.ToInt();
            Console.WriteLine("result is " + result);
            Assert.True(result == 13);




        }

        [Test]
        public void Test_ConvertToBitArray_1() {
            var input1 = new List<int> { 1 , 2 , 3 , 4 , 5 };
            int actual = Helper.ToBitArray(input1).ToInt();
            int expected = Convert.ToInt32("11111" , 2);
            Assert.True(actual == expected);




        }
    }
}
