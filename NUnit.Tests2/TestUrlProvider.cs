using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Pages.Login;

namespace NUnit.Tests2 {
    [TestFixture]
   public class TestUrlProvider {
        [Test]
        public void Test_IsAtLoginPage_1() {
            string input = "https://unitreg.utar.edu.my/portal/Kaptcha.jpg";
            var urlProvider = new UrlProvider();
            Assert.IsFalse(urlProvider.IsAtLoginPage(input));
        }
        [Test]
        public void Test_IsAtLoginPage_2() {
            string input = "https://unitreg.utar.edu.my/portal/courseRegStu/login.jsp";
            var urlProvider = new UrlProvider();
            Assert.IsTrue(urlProvider.IsAtLoginPage(input));
        }

        [Test]
        public void Test_IsAtLoginPage_3 (){
            string input = "http://unitreg.utar.edu.my/portal/courseRegStu/login.jsp";
            var urlProvider = new UrlProvider();
            Assert.IsTrue(urlProvider.IsAtLoginPage(input));
        }
    }
}
