using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTerminalLibrary.Console;
using Time_Table_Arranging_Program.Pages;

namespace Time_Table_Arranging_Program.ConsoleCommands {
    public class LoadTestDataCommand : ConsoleCommandBase {
        public override string Execute() {
            ((MainWindow)Commandee).LoadTestData();
            return "Loaded test data.";
        }

        public LoadTestDataCommand(object commandee) : base(commandee , "load-test-data" , "Load test data of timeslots.") { }
    }
}
