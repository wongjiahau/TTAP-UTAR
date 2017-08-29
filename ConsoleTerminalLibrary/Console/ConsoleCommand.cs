using System;
using System.Linq;
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

        public string ExecuteCommand(string arg) {
            return ArgumentIsValid(arg) ?
                Execute(arg) : 
                $"'{arg}' is not a valid argument for '{Keyword()}'" ;
        }

        protected virtual bool ArgumentIsValid(string arg) {
            return Arguments().ToList().Contains(arg);
        }

        protected abstract string Execute(string arg);
    }
}
