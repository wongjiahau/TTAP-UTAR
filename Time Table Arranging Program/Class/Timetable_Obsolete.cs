using System;
using System.Collections.Generic;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class {
    public interface ITimetable_Obsolete {
        bool Add(Slot s);
        bool AddRange(List<Slot> s);
        void Clear();
        List<Slot> ToListOfSlot();
    }

    [Obsolete("Use Timetable instead")]
    public class Timetable_Obsolete : ITimetable_Obsolete, IDuplicable<Timetable_Obsolete>,
        IIntersectionCheckable<Timetable_Obsolete>,
        IChecklessJoinable<Timetable_Obsolete>, IHashable {
        private static int _uid;
        private readonly TimetableRow[] _rows;
        private int UID;

        private Timetable_Obsolete() {
            _rows = new TimetableRow[Day.NumberOfDaysPerWeek];
            for (var i = 0; i < _rows.Length; i++) {
                _rows[i] = new TimetableRow();
            }
        }

        /// <summary>
        ///     Join two timetables together
        /// </summary>
        /// <param name="other"></param>
        /// <returns>Result will contain slot from both time table</returns>
        public Timetable_Obsolete JoinWithoutChecking(Timetable_Obsolete other) {
            var result = new Timetable_Obsolete();
            for (var i = 0; i < _rows.Length; i++) {
                result._rows[i] = _rows[i].JoinWithoutChecking(other._rows[i]);
            }
            return result;
        }

        public Timetable_Obsolete GetDuplicate() {
            var result = new Timetable_Obsolete();
            for (var i = 0; i < _rows.Length; i++) {
                result._rows[i] = _rows[i].GetDuplicate();
            }
            return result;
        }

        public int GetUid() {
            return _uid;
        }

        public bool IntersectWith(Timetable_Obsolete other) {
            for (var i = 0; i < _rows.Length; i++) {
                if (_rows[i].IntersectWith(other._rows[i])) return true;
            }
            return false;
        }

        public bool Add(Slot s) {
            return _rows[s.Day.IntValue - 1].Add(s);
        }

        public bool AddRange(List<Slot> s) {
            for (var i = 0; i < s.Count; i++) {
                if (!Add(s[i])) return false;
            }
            return true;
        }

        public void Clear() {
            for (var i = 0; i < _rows.Length; i++) {
                _rows[i].Clear();
            }
        }

        public List<Slot> ToListOfSlot() {
            var result = new List<Slot>();
            for (var i = 0; i < _rows.Length; i++) {
                result.AddRange(_rows[i].ToListOfSlot());
            }
            return result;
        }

        public static Timetable_Obsolete CreateNew() {
            var t = new Timetable_Obsolete();
            t.UID = _uid++;
            return t;
        }

        public static Timetable_Obsolete CreateNew(List<Slot> s) {
            var result = new Timetable_Obsolete();
            result.AddRange(s);
            return result;
        }
    }
}