using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;

namespace NUnit.Tests2.TestSuite_HtmlSlotParser {
    [TestFixture]
    public class TestHtmlSlotParser_MainFrame {
        [Test]
        public void TestFor_SampleHtml() {
            new TestHtmlSlotParser_SampleHtml().Run();
        }
        [Test]
        public void TestFor_SampleData_FAM_2017_2ndSem() {
            new TestHtmlSlotParser_SampleData_FAM_2017_2ndSem().Run();
        }
        [Test]
        public void TestFor_SampleData_FCI_2017_2ndSem() {
            new TestHtmlSlotParser_SampleData_FCI_2017_2ndSem().Run();
        }
    }
}
