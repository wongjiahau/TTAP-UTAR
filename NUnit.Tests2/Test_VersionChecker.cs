using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class;

namespace NUnit.Tests2 {
    [TestFixture]
    public class TestVersionChecker {
        [Test]
        public void Test_ThisVersionIsUpToDate() {
            var c = new VersionChecker();
            Console.WriteLine("Current version is " + c.CurrentVersion());
            Console.WriteLine("Latest version is " + c.LatestVersion());
            Assert.IsTrue(c.ThisVersionIsUpToDate());
        }
    }
}
