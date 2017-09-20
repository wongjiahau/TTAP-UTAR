using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_Version {
        [Test]
        public void Test_Version_Compare_GreaterThan() {
            var v1 = new Version(1 , 0 , 0);
            var v2 = new Version(2 , 0 , 0);
            Assert.IsTrue(v2.CompareTo(v1) > 0);
        }

        [Test]
        public void Test_Version_Compare_LesserThan() {
            var v1 = new Version(1 , 2 , 3);
            var v2 = new Version(1 , 1 , 4);
            Assert.IsTrue(v2.CompareTo(v1) < 0);
        }

        [Test]
        public void Test_Version_Compare_Equal() {
            var v1 = new Version(1 , 2 , 3);
            var v2 = new Version(1 , 2 , 3);
            Assert.IsTrue(v1.Equals(v2));
        }

        [Test]
        public void Test_Version_Parse() {
            var expected = new Version(1 , 3 , 4);
            var actual = Version.Parse("1.3.4");
            Assert.IsTrue(expected.Equals(actual));
        }
    }
}
