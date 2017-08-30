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
        public static async void SendIssue(Exception ex,string body) {
            var client = new GitHubClient(new ProductHeaderValue("ttap-bug-report"));
            string encryptedToken = "WeLRINbTL+o7ae7LJyJguK6MNKeqCwfC6GEYreE4GJhCV0A0spfaRxoBjgQWgCeOdaaNSIp2f2dHBMEuMOUDAN0lxZOWyzP1T2dd5JQRcgUlk2Yjzz7uKbf1w31XCpj9QT7F6iNqhqW44TNigHTb5Pb1t2DAiUiz2MxozJtgsEE=";
            var basicAuth = new Credentials(StringCipher.Decrypt(encryptedToken, "TTAP")); // NOTE: not real credentials
            client.Credentials = basicAuth;
            var createIssue = new NewIssue("Bug report #" + DateTime.Now.GetHashCode());
            createIssue.Body = ex.Message + "\n====================\n";
            createIssue.Body += ex.StackTrace+ "\n====================\n";
            createIssue.Body += body;
            var issue = await client.Issue.Create("wongjiahau" , "TTAP-Bug-Report" , createIssue);
        }

    }
}
