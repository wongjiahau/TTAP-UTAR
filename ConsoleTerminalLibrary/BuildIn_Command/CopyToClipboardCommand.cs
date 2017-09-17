using System;
using System.Windows;
using ConsoleTerminalLibrary.Console;

namespace ConsoleTerminalLibrary.BuildIn_Command {
    public class CopyToClipboardCommand : ConsoleCommandWithArgument{
        public CopyToClipboardCommand(object commandee) : base(commandee) { }
        protected override string Execute(string argument) {
            try {
                Clipboard.SetDataObject(argument);
                return $"'{argument}' is copied to clipboard.";
            }
            catch (Exception e) {
                return $"Failed to copy.\nERROR={e.Message}";

            }
        }

        protected override bool ArgumentIsValid(string arg) {
            return true;
        }

        public override string Keyword() {
            return "/copy-to-clipboard";
        }

        public override string Help() {
            return "Format : copy <arg1>\n" + "Copy <arg1> to system clipboard";
        }

        public override string[] Arguments() {
            throw new NotImplementedException();
        }
    }
}
