using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.MVVM_Framework.ViewModels {
    public class TimetableVM : ViewModelBase<Timetable> {
        public IList<Slot> Slots => Model.Slots;
    }
}