using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTerminalLibrary.Console;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleTerminalLibrary.BuildIn_Command {
    public class HelpCommand : ConsoleCommandBase {
        public HelpCommand(object commandee) : base(commandee) {
        }

        public override string Execute() {
            var commandList = Commandee as List<IConsoleCommand>;
            Assert.IsTrue(commandList!=null);
            string result = "Available commands :\n";
            foreach (IConsoleCommand t in commandList) {
                result += "\t" + t.Keyword() + "\n";
            }
            return result;
        }

        public override string Keyword() {
            return "help";
        }

        public override string Help() {
            return "Show list of available commands";
        }

        public override string[] Options() {
            throw new NotImplementedException();
        }
    }
}
