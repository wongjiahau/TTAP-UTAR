using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleTerminalLibrary.Console;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleTerminalLibrary.BuildIn_Command {
    public class ClearScreenCommand : ConsoleCommandBase {
        public ClearScreenCommand(object commandee) : base(commandee) {

        }

        public override string Execute() {
            var output = Commandee as ObservableCollection<string>;
            Assert.IsTrue(output!=null);
            output.Clear();
            return "";
        }

        public override string Keyword() {
            return "/clear";
        }

        public override string Help() {
            return "Clear the screen of the console.";
        }
    }
}
