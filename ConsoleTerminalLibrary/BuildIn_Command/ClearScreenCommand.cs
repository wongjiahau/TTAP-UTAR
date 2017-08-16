using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTerminalLibrary.Console;

namespace ConsoleTerminalLibrary.BuildIn_Command {
    public class ClearScreenCommand : ConsoleCommandBase {
        public ClearScreenCommand(object commandee, string keyword, string help) : base(commandee, keyword, help) { }
        public override string Execute() {
            throw new NotImplementedException();
        }
    }
}
