using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Pages.Login {
    public class UrlProvider {
        public string LoginPageUrl => "https://unitreg.utar.edu.my/portal/courseRegStu/login.jsp";
        public string CourseTimetablePreviewUrl => "https://unitreg.utar.edu.my/portal/courseRegStu/schedule/masterSchedule.jsp";
        public string KaptchaUrl => "https://unitreg.utar.edu.my/portal/Kaptcha.jpg";
        public string TestServerUrl => "http://localhost/ttap_testdata/";
        public string EndUrl => "http://0.0.0.0/";
        public bool IsLoginFailed(string url) {
            string InvalidIdOrPasswordUrl = "https://unitreg.utar.edu.my/portal/courseRegStu/login.jsp?message=loginError";
            string InvalidCaptchaUrl = "https://unitreg.utar.edu.my/portal/courseRegStu/login.jsp?message=invalidSecurity";
            return
                url.Contains(InvalidCaptchaUrl) ||
                url.Contains(InvalidIdOrPasswordUrl);
        }

        public bool IsAtLoginPage(string url) {
            return url.Split(new string[] { "//" }, StringSplitOptions.None)[1] == LoginPageUrl.Split(new string[] { "//" }, StringSplitOptions.None)[1];
        }
    }
}
