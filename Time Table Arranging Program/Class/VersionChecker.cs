using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Class {
    public class VersionChecker {
        public Version CurrentVersion() {
            return Version.Parse(Helper.Helper.RawStringOfTestFile("version.txt", "Time_Table_Arranging_Program."));
        }

        public Version LatestVersion() {
            return
                Version.Parse(DownloadFile(
                    "https://raw.githubusercontent.com/wongjiahau/TTAP-UTAR/master/version.txt"));
        }

        private static string DownloadFile(string sourceUrl) //https://gist.github.com/nboubakr/7812375
                {
            long existLen = 0;
            var httpReq = (HttpWebRequest)WebRequest.Create(sourceUrl);
            httpReq.AddRange((int)existLen);
            var httpRes = (HttpWebResponse)httpReq.GetResponse();
            var responseStream = httpRes.GetResponseStream();
            if (responseStream == null) return "Fail to fetch file";
            var streamReader = new StreamReader(responseStream);
            return streamReader.ReadToEnd();

        }

        public bool ThisVersionIsUpToDate() {
            try {
                return CurrentVersion().Equals(LatestVersion());
            }
            catch {
                //Exception shold be surpress, because this feature is not always needed                
            }
            return true;
        }

    }
}
