using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTerminalLibrary.Console;
using Time_Table_Arranging_Program.Pages;

namespace Time_Table_Arranging_Program.ConsoleCommands {
    public class LoadTestDataCommand : ConsoleCommandWithArgument{
        public LoadTestDataCommand(object commandee) : base(commandee) {
        }

        public override string[] Arguments() {
            return new[] {"hello", "banana"};
        }

        public override string Execute(string s) {
            ((MainWindow)Commandee).LoadTestData();
            return "Loaded test data.";
        }

        public override string Keyword() {
            return "load-test-data";

        }

        public override string Help() {
            return "Load sample data of timeslots";
        }
    }
}
