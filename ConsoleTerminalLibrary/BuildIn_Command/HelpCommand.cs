using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTerminalLibrary.Console;

namespace ConsoleTerminalLibrary.BuildIn_Command {
    public class HelpCommand : ConsoleCommandBase {
        public HelpCommand(object commandee) : base(commandee, "help", "Show list of available commands.") { }

        public override string Execute() {
            var commandList = Commandee as List<IConsoleCommand>;
            string result = "Available commands :\n";
            foreach (IConsoleCommand t in commandList) {
                result += "\t" + t.Keyword + "\n";
            }
            return result;
        }
    }
}
