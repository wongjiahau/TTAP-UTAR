using System.Net;

namespace ConsoleTerminalLibrary.Console {
    public interface IConsoleCommand {
        string Execute();
    }

    public abstract class ConsoleCommandBase : IConsoleCommand {
        protected object Commandee;
        protected string Keyword;
        protected string Help;
        protected ConsoleCommandBase(object target, object commandee, string keyword, string help) {
            Commandee = commandee;
            Keyword = keyword;
            Help = help;
        }
        public abstract string Execute();
    }
}
