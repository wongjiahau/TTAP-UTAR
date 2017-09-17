using System.Collections.Generic;

namespace Time_Table_Arranging_Program.Class.UndoFramework {
    public class UndoManager {
        private readonly Stack<IUndoableAction> _redoableActions = new Stack<IUndoableAction>();
        private readonly Stack<IUndoableAction> _undoableActions = new Stack<IUndoableAction>();

        public void Execute(IUndoableAction action) {
            action.Execute();
            _undoableActions.Push(action);
            _redoableActions.Clear();
        }

        public void ClearHistory() {
            _undoableActions.Clear();
            _redoableActions.Clear();
        }

        public void Undo() {
            if (_undoableActions.Count == 0) return;
            var lastAction = _undoableActions.Pop();
            lastAction.Undo();
            _redoableActions.Push(lastAction);
        }

        public void Redo() {
            if (_redoableActions.Count == 0) return;
            var lastUndoneAction = _redoableActions.Pop();
            lastUndoneAction.Execute();
            _undoableActions.Push(lastUndoneAction);
        }
    }
}