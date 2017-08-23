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
    }
}
