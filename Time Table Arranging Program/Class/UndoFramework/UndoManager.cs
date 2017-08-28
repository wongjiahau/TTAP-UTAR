using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Class.UndoFramework {
    public class UndoManager {
        private Stack<IUndoableAction> _undoableActions = new Stack<IUndoableAction>();
        private Stack<IUndoableAction> _redoableActions = new Stack<IUndoableAction>();
       public void Execute(IUndoableAction action) {
            action.Execute();
            _undoableActions.Push(action);
            _redoableActions.Clear();
        }

        public void Undo() {
            var lastAction = _undoableActions.Pop();
            lastAction.Undo();
            _redoableActions.Push(lastAction);
        }

        public void Redo() {
            var lastUndoneAction = _redoableActions.Pop();
            lastUndoneAction.Execute();
            _undoableActions.Push(lastUndoneAction);
        }
    }
}
