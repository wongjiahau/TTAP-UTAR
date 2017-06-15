using System.Collections.Generic;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class {
    public interface ITimetableRow {
        bool Add(Slot s);
        void Clear();
        List<Slot> ToListOfSlot();
    }

    public class TimetableRow : ITimetableRow, IDuplicable<TimetableRow>, IIntersectionCheckable<TimetableRow>,
        IChecklessJoinable<TimetableRow> {
        private List<Slot> _slots;

        public TimetableRow() {
            _slots = new List<Slot>();
        }

        public TimetableRow JoinWithoutChecking(TimetableRow other) {
            var result = new TimetableRow();
            result._slots.AddRange(_slots);
            result._slots.AddRange(other._slots);
            return result;
        }

        public TimetableRow GetDuplicate() {
            var result = new TimetableRow();
            result._slots = new List<Slot>(_slots);
            return result;
        }

        public bool IntersectWith(TimetableRow other) {
            for (var i = 0; i < _slots.Count; i++) {
                for (var j = 0; j < other._slots.Count; j++) {
                    if (!_slots[i].TimePeriod.IntersectWith(other._slots[j].TimePeriod)) continue;
                    if (_slots[i].WeekNumber.IntersectWith(other._slots[j].WeekNumber))
                        return true;
                }
            }
            return false;
        }

        public bool Add(Slot s) {
            for (var i = 0; i < _slots.Count; i++) {
                if (!_slots[i].TimePeriod.IntersectWith(s.TimePeriod)) continue;
                if (_slots[i].WeekNumber.IntersectWith(s.WeekNumber))
                    return false;
            }
            _slots.Add(s);
            return true;
        }

        public void Clear() {
            _slots.Clear();
        }

        public List<Slot> ToListOfSlot() {
            return _slots;
        }
    }
}