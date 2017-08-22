using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTerminalLibrary.Console;
using MaterialDesignThemes.Wpf;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Time_Table_Arranging_Program.ConsoleCommands {
    public class HideConsoleCommand : ConsoleCommandBase {
        public HideConsoleCommand(object commandee) : base(commandee) { }
        public override string Execute() {
            var drawerHost = Commandee as DrawerHost;
            Assert.IsNotNull(drawerHost);
            drawerHost.IsBottomDrawerOpen = false;
            return "";
        }

        public override string Keyword() {
            return "hide-console";
        }

        public override string Help() {
            return "Hide/Collapse this console";
        }

        public override string[] Options() {
            throw new NotImplementedException();
        }
    }
}
