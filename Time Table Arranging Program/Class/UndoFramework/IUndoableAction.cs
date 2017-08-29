using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Class.UndoFramework {
    public interface IUndoableAction {
        void Execute();
        void Undo();
        string ToolTipMessage { get; }
    }

    public abstract class UndoableAction : IUndoableAction {
        public object Target { get; }
        public UndoableAction(object target) {
            Target = target;
        }

        public abstract void Execute();
        public abstract void Undo();
        public abstract string ToolTipMessage { get; }
    }


}

