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
            string encryptedToken = "n5eW48VENjjGwihIdstsCAyRHf8Jp1cTiGd9x1E8qUrTluvMFZVX4i/w4dRJrIIga4ZKWZPjOa7nn4GwWVvSB7YjB+NzHj+xh/4EiMZnByCIeF0myABJux1OhLu7Yam8ZfI84YMunzv7bot87P2a38CiOKJtoSBYg4ztxh7byp0=";
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
