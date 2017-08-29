using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Class {
    public class BugReportSender {
        public void Send(string body) {
            var fromAddress = new MailAddress("ttaputar@gmail.com" , "TTAP");
            var toAddress = new MailAddress("ttaputar@gmail.com" , "TTAP");
            const string fromPassword = "842684268426ttap";
            string subject = "Bug report " + body.GetHashCode();

            var smtp = new SmtpClient {
                Host = "smtp.gmail.com" ,
                Port = 587 ,
                EnableSsl = true ,
                DeliveryMethod = SmtpDeliveryMethod.Network ,
                UseDefaultCredentials = false ,
                Credentials = new NetworkCredential(fromAddress.Address , fromPassword)
            };
            using (var message = new MailMessage(fromAddress , toAddress) {
                Subject = subject ,
                Body = body
            }) {
                smtp.Send(message);
            }
        }

    }
}
