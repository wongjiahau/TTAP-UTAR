namespace ConsoleTerminalLibrary.Console {
    public interface IConsoleCommand {
        void Execute();
    }

    public abstract class ConsoleCommandBase : IConsoleCommand {
        protected object _target;
        protected ConsoleCommandBase(object target) {
            _target = target;
        }
        public abstract void Execute();
    }
}
