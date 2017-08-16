using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTerminalLibrary.Console;

namespace ConsoleTerminalLibrary.BuildIn_Command {
    public class ClearScreenCommand : ConsoleCommandBase {
        public ClearScreenCommand(object commandee) : base(commandee) {

        }

        public override string Execute() {
            throw new NotImplementedException();
        }

        public override string Keyword() {
            return "clear";
        }

        public override string Help() {
            return "Clear the screen of the console.";
        }

        public override string[] Options() {
            throw new NotImplementedException();
        }
    }
}
