using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Class.UndoFramework {
    public interface IActor {
        /// <summary>
        /// Set the current post of the actor based on the snapshot
        /// </summary>
        /// <param name="snapshot">
        /// A snapshot of the actor's previous post
        /// </param>
        void SetCurrentPost(Snapshot snapshot);

        Snapshot TakeSnapshot();
    }
}