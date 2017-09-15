using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Interfaces {
    public interface INeedDataContext<T> {
        void SetDataContext(T subjectModels);
    }
}