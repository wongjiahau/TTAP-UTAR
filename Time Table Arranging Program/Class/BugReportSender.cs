using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Octokit;

namespace Time_Table_Arranging_Program.Class {
    public class BugReportSender {
        public void Send( string body) {
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

        public static async void SendIssue(Exception ex,string body) {
            var client = new GitHubClient(new ProductHeaderValue("ttap-bug-report"));
            var basicAuth = new Credentials("ecce578659b3b1d071db571c1120b870cbb06767"); // NOTE: not real credentials
            client.Credentials = basicAuth;
            var createIssue = new NewIssue("Bug report #" + DateTime.Now.GetHashCode());
            createIssue.Body = ex.Message + "\n====================\n";
            createIssue.Body += ex.StackTrace+ "\n====================\n";
            createIssue.Body += body;
            var issue = await client.Issue.Create("wongjiahau" , "TTAP-Bug-Report" , createIssue);
        }

    }
}
