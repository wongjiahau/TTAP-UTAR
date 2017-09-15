using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTerminalLibrary.Console;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.ConsoleCommands {
    public class ResetDataCommand : ConsoleCommandBase {
        public ResetDataCommand(object commandee) : base(commandee) { }

        public override string Execute() {
            var inputSlots = Commandee as SlotList;
            try {
                inputSlots.Clear();
                return "Cleared data in Global.InputSlotList";
            }
            catch (Exception e) {
                return e.Message;
            }
        }

        public override string Keyword() {
            return "reset-data";
        }

        public override string Help() {
            return "Clear the data in variable Global.InputSlotLists.";
        }
    }
}