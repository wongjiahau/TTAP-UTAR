using System;
using System.Linq;

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

        protected virtual bool ArgumentIsValid(string arg) {
            return Arguments().ToList().Contains(arg);
        }

        protected abstract string Execute(string arg);
    }
}
