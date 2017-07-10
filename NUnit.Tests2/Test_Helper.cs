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
        public void Test_ToBitArray_1() {
            int input = Convert.ToInt32("1100",2);
            var result = input.ToBitArray();
            var expected = new List<bool>() { false,false,true,true};
            for (int i = 0; i < 4; i++) {
               Assert.IsTrue(result[i]== expected[i]);
            }
        }
    }
}
