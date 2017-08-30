using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTerminalLibrary.Console;

namespace Time_Table_Arranging_Program.ConsoleCommands {
    public class ThrowExceptionCommand : ConsoleCommandBase {
        public ThrowExceptionCommand(object commandee) : base(commandee) { }
        public override string Execute() {
            throw new Exception("This is a test exception.");
        }

        public override string Keyword() {
            return "throw-exception";
        }

        public override string Help() {
            return "Throw an exception.";
        }
    }
}
