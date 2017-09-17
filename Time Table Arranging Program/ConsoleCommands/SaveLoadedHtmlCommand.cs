using ConsoleTerminalLibrary.Console;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.ConsoleCommands {
    public class SaveLoadedHtmlCommand : ConsoleCommandBase {
        public SaveLoadedHtmlCommand(object commandee) : base(commandee) { }

        public override string Execute() {
            var togglableObject = Commandee as IToggableObject;
            togglableObject.Toggle();
            return $"Load HTML toggle is toggled " +
                   (togglableObject.IsToggledOn ? "ON" : "OFF") + ".";
        }


        public override string Keyword() {
            return "save-loaded-html";
        }

        public override string Help() {
            return "Turn on to save loaded HTML";
        }
    }
}