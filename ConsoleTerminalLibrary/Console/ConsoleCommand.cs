using System;
using System.Net;

namespace ConsoleTerminalLibrary.Console {
    public interface IConsoleCommand {
        string Execute();
        object Commandee { get; }
        string Keyword();
        string Help();
    }

    public abstract class ConsoleCommandBase : IConsoleCommand {
        public object Commandee { get; private set; }
        protected ConsoleCommandBase(object commandee) {
            Commandee = commandee;
        }
        public abstract string Execute();
        public abstract string Keyword();
        public abstract string Help();
    }

    public abstract class ConsoleCommandWithArgument : ConsoleCommandBase {
        public ConsoleCommandWithArgument(object commandee) : base(commandee) { }
        public sealed override string Execute() {
            throw new NotImplementedException();
        }
        public abstract string[] Arguments();
        public abstract string Execute(string s);
    }
}
