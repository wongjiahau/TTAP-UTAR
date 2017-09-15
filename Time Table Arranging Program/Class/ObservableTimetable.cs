using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class {
    public class ObservableTimetable : MutableObservable<ITimetable> {
        public ObservableTimetable(ITimetable initialState) : base(initialState) { }
    }
}