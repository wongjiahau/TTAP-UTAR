using System;
using System.Collections.Generic;
using ConsoleTerminalLibrary.Console;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleTerminalLibrary.BuildIn_Command {
    public class HistoryCommand : ConsoleCommandBase {
        public HistoryCommand(object commandee) : base(commandee) { }
        public override string Execute() {
            var inputHistory = Commandee as List<string>;
            Assert.IsNotNull(inputHistory);
            string result = "History of executed command(s) :\n";
            for (int i = 0 ; i < inputHistory.Count ; i++) {
                result += $"\t{i + 1}. {inputHistory[i]}\n";
            }
            return result;
        }

        public override string Keyword() {
            return "/history";
        }

        public override string Help() {
            return "List history of executed commands.";
        }
    }
}
