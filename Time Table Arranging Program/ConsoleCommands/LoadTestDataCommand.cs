using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using ConsoleTerminalLibrary.Console;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Helper;
using Time_Table_Arranging_Program.Class.TokenParser;
using Time_Table_Arranging_Program.Pages;

namespace Time_Table_Arranging_Program.ConsoleCommands {
    public class LoadTestDataCommand : ConsoleCommandWithArgument {
        public LoadTestDataCommand(object commandee) : base(commandee) {
        }

        readonly string leadingNamespace = "Time_Table_Arranging_Program.SampleData";
        public override string[] Arguments() {
            string[] embeddedResources = Assembly.GetAssembly(this.GetType()).GetManifestResourceNames();
            string[] sampleDataFiles = embeddedResources.ToList().Where(x => x.EndsWith("html")).ToArray();
            for (int i = 0 ; i < sampleDataFiles.Length ; i++) {
                sampleDataFiles[i] = sampleDataFiles[i].Substring(leadingNamespace.Length + 1);
            }
            return sampleDataFiles;
        }

        protected override string Execute(string resourceName) {
            string raw = Helper.RawStringOfTestFile(resourceName, leadingNamespace + ".");
            try
            {
                var parser = new StartDateEndDateFinder(raw);
                Global.TimetableStartDate = parser.GetStartDate();
                Global.TimetableEndDate = parser.GetEndDate();
            }
            catch { }
            return "Starting date: " + Global.TimetableStartDate + " EndingDate: " + Global.TimetableEndDate;
        }

        public override string Keyword() {
            return "load-test-data";

        }

        public override string Help() {
            return "Load sample data of timeslots";
        }
    }
}
