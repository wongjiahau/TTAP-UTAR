namespace Time_Table_Arranging_Program.Class.UndoFramework {
    public interface IUndoableAction {
        string ToolTipMessage { get; }
        void Execute();
        void Undo();
    }

    public abstract class UndoableAction : IUndoableAction {
        public UndoableAction(object target) {
            Target = target;
        }

        public object Target { get; }

        public abstract void Execute();
        public abstract void Undo();
        public abstract string ToolTipMessage { get; }
    }
}