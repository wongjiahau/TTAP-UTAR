using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Pages.Login {
    public class UrlProvider {
        public string LoginPageUrl => "https://unitreg.utar.edu.my/portal/courseRegStu/login.jsp";
        public string LoginFailedUrl => "https://unitreg.utar.edu.my/portal/courseRegStu/login.jsp?message=invalidSecurity";
        public string CourseTimetablePreviewUrl => "https://unitreg.utar.edu.my/portal/courseRegStu/schedule/masterSchedule.jsp";
        public string KaptchaUrl => "https://unitreg.utar.edu.my/portal/Kaptcha.jpg";
        public string TestServerUrl => "http://localhost/ttap_testdata/";

    }
}
