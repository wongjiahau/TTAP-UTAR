using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Console {
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
