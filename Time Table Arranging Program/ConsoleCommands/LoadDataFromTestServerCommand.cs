using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTerminalLibrary.Console;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Time_Table_Arranging_Program.Pages;

namespace Time_Table_Arranging_Program.ConsoleCommands {
    public class LoadDataFromTestServerCommand : ConsoleCommandBase{
        public LoadDataFromTestServerCommand(object commandee) : base(commandee) { }
        public override string Execute() {
            var page_login = Commandee as MainWindow;
            Assert.IsNotNull(page_login);
            try {
                page_login.LoadDataFromTestServer();
                return "";
            }
            catch (Exception e) {
                return "An error occurred :\n" + e.Message;
            }
        }

        public override string Keyword() {
            return "load-data-from-test-server";
        }

        public override string Help() {
            return "Load sample timeslots from local server (IIS).";
        }

        public override string[] Options() {
            throw new NotImplementedException();
        }
    }
}
