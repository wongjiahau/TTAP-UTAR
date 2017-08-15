using System.Net;

namespace ConsoleTerminalLibrary.Console {
    public interface IConsoleCommand {
        string Execute();
        object Commandee { get; }
        string Keyword { get; }
        string Help { get; }
    }

    public abstract class ConsoleCommandBase : IConsoleCommand {
        public object Commandee { get; private set; }
        public string Keyword { get; private set; }
        public string Help { get; private set; }
        protected ConsoleCommandBase(object commandee , string keyword , string help) {
            Commandee = commandee;
            Keyword = keyword;
            Help = help;
        }
        public abstract string Execute();
    }
}
