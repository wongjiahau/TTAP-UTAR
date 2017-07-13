using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Time_Table_Arranging_Program.Interfaces;


namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_CyclicIteratableList {

        [Test]
        public void Test_GetCurrent() {
            var input = new CyclicIteratableList<int>(new List<int>() { 1 , 2 , 3 });
            Assert.IsTrue(input.GetCurrent() == 1);
        }

        [Test]
        public void Test_Counts() {
            var input = new CyclicIteratableList<int>(new List<int>() { 1 , 2 , 3 });
            Assert.IsTrue(input.Counts == 3);
        }

        [Test]
        public void Test_GoToNext() {
            var input = new CyclicIteratableList<int>(new List<int>() { 1 , 2 , 3 });
            input.GoToNext();
            Assert.IsTrue(input.GetCurrent() == 2);
            input.GoToNext();
            Assert.IsTrue(input.GetCurrent() == 3);
            input.GoToNext();
            Assert.IsTrue(input.GetCurrent() == 1);
        }

        [Test]
        public void Test_GoToPrevious() {
            var input = new CyclicIteratableList<int>(new List<int>() { 1 , 2 , 3 });
            input.GoToPrevious();
            Assert.IsTrue(input.GetCurrent() == 3);
            input.GoToPrevious();
            Assert.IsTrue(input.GetCurrent() == 2);
            input.GoToPrevious();
            Assert.IsTrue(input.GetCurrent() == 1);
        }

        [Test]
        public void Test_Index() {
            var input = new CyclicIteratableList<int>(new List<int>() { 1 , 2 , 3 });
            Assert.IsTrue(input.MaxIndex() == 2);
            Assert.IsTrue(input.CurrentIndex() == 0);
        }

        [Test]
        public void Test_AtFirst() {
            var input = new CyclicIteratableList<int>(new List<int>() { 1 , 2 , 3 });            
            Assert.IsTrue(input.AtFirst());
        }
        [Test]
        public void Test_AtLast() {
            var input = new CyclicIteratableList<int>(new List<int>() { 1 , 2 , 3 });
            input.GoToPrevious();            
            Assert.IsTrue(input.AtLast());
        }

    }
}
