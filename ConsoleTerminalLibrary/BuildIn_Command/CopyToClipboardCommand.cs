using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ConsoleTerminalLibrary.Console;

namespace ConsoleTerminalLibrary.BuildIn_Command {
    public class CopyToClipboardCommand : CommandWithArgument{
        public CopyToClipboardCommand(object commandee) : base(commandee) { }
        public override string Execute(string argument) {
            try {
                Clipboard.SetDataObject(argument);
                return $"'{argument}' is copied to clipboard.";
            }
            catch (Exception e) {
                return $"Failed to copy.\nERROR={e.Message}";

            }
        }

        public override string Keyword() {
            return "copy-to-clipboard";
        }

        public override string Help() {
            return "Format : copy <arg1>\n" + "Copy <arg1> to system clipboard";
        }

        public override string[] Options() {
            throw new NotImplementedException();
        }
    }
}
