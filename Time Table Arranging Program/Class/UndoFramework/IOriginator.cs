using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Class.UndoFramework {
    public interface IOriginator {
        void SetMemento(Memento m);
        Memento CreateMemento();
    }

}
