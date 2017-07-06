using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.Model {
    public class BinarizeSlotModel : IIntersectionCheckable<BinarizeSlotModel> {
        private int _day;
        private int _timePeriod;
        private int _weekNumber;
        public BinarizeSlotModel(Slot s) {
            _day = s.Day.ToBinary();
            _timePeriod = s.TimePeriod.ToBinary();
            _weekNumber = s.WeekNumber.ToBinary();
        }

        public bool IntersectWith(BinarizeSlotModel other) {
            throw new NotImplementedException();
        }
    }
}
