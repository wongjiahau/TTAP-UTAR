using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTerminalLibrary.Console;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.ConsoleCommands {
    public class SaveLoadedHtmlCommand : ConsoleCommandBase {
        public SaveLoadedHtmlCommand(object commandee) : base(commandee) { }

        public override string Execute() {
            return "LOL";
        }


        public override string Keyword() {
            return "save-loaded-html";
        }

        public override string Help() {
            return "Turn on to save loaded HTML";
        }
    }
}
