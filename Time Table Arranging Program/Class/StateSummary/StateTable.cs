using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Class.Helper;

namespace Time_Table_Arranging_Program.Class.StateSummary {
    public interface IStateTable : IEnumerable<IStateCell>{
        void Add(IndexedSlot s);
        bool ClashesWithCurrentState(IndexedSlot s);
        void Reset();
        string Draw(int maxCount);
    }  

    public class StateTable : IStateTable {
        private List<IStateCell> _stateCells = new List<IStateCell>();

        public static StateTable Parse(List<List<Slot>> x) {
            return new StateTable(x);
        }
        public StateTable(List<List<Slot>> outputTimetables) : this() {  
            if(outputTimetables == null) outputTimetables = new List<List<Slot>>();          
            StateCell.MaxValue =  outputTimetables.Count;
            var r = outputTimetables;
            for (int i = 0 ; i < r.Count ; i++) {
                for (int j = 0 ; j < r[i].Count ; j++) {
                    Add(new IndexedSlot(r[i][j]));
                }
            }
        }

        [Obsolete("Incomplete")]
        private void NewAlgorithmUsingBinary(List<List<Slot>> outputTimetables ) {
            Dictionary<int,int[]> states =  new Dictionary<int, int[]>();
            for (int i = 1; i <= 7; i++) {
             states.Add(1, new int[2] {1,0});   
                //1st array means AND-ed value, 2nd array means OR-ed value
            }
            var o = outputTimetables;
            for (int i = 0; i < o.Count; i++) {
                for (int j = 0; j < o[i].Count; j++) {
                    var slot = o[i][j];
                    int day = slot.Day.IntValue;
                    int timeperiodInBinary = slot.TimePeriod.ToBinary();
                    states[day][0] &= timeperiodInBinary;
                    states[day][1] |= timeperiodInBinary;
                }
            }
            _stateCells = new List<IStateCell>();
            int length = 32;
            for (int i = 1; i <= 7; i++) {
                var and_value = states[i][0].ToBitArray();
                var or_value = states[i][1].ToBitArray();
                var xor_value = and_value.Xor(or_value);
                for (int j = 0; j < length; j++) {
                    
                }

            }
        }

        private StateTable() {
            Reset();
        }

        public void Add(IndexedSlot s) {
            for (int i = 0 ; i < s.ColumnSpan ; i++) {
                var targetCell = _stateCells.Find(
                    x => x.RowIndex == s.RowIndex &&
                    x.ColumnIndex == s.ColumnIndex + i
                    );
                if (targetCell != null) targetCell.CellValue++;
                else throw new Exception("Target cell shouldn't be null");
            }
        }

        public bool ClashesWithCurrentState(IndexedSlot s) {
            throw new System.NotImplementedException();
        }

        public void Reset() {
            const int numberOfDays = 7;
            const int maxNumberOfColumns = StateRow.MaxNumberOfColumn;
            for (int i = 0 ; i < numberOfDays ; i++) {
                Day day = Day.GetDay(i + 1);
                Time time = Time.CreateTime_24HourFormat(7,0);
                for (int j = 0 ; j < maxNumberOfColumns ; j++) {
                    _stateCells.Add(new StateCell(i , j, day, time));
                    time = (Time)time.Add(Time.CreateTime_24HourFormat(0, 30));
                }
            }                               


        }

        public string Draw(int maxCount) {
            throw new NotImplementedException();
        }


        public IEnumerator<IStateCell> GetEnumerator() {
            return _stateCells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }

    public interface IStateCell {
        int RowIndex { get; }
        int ColumnIndex { get; }
        int CellValue { get; set; }
        CellState State { get; }
        Predicate<Slot> ConstraintPredicate { get; }

    }

    public enum CellState { DefinitelyOccupied, MaybeUnoccupied, DefinitelyUnoccupied }

    public class StateCell : IStateCell {
        public StateCell(int rowIndex , int columnIndex , Day day , Time startTime) {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            _day = day;
            _startTime = startTime;
        }

        private Day _day;
        private Time _startTime;
        private bool BetweenPredicate(Slot s) {
            var timePeriod = new TimePeriod(_startTime , _startTime.Add(Time.CreateTime_24HourFormat(0 , 30)));
            return
                s.Day.Equals(_day) &&
            s.TimePeriod.IntersectWith(timePeriod);

        }
        public const int ColumnSpanOfEachCell = 1;
        public static int MaxValue;

        public int RowIndex { get; private set; }
        public int ColumnIndex { get; private set; }
        public int CellValue { get; set; } = 0;

        public CellState State {
            get {
                if (CellValue >= MaxValue) return CellState.DefinitelyOccupied;
                else if (CellValue > 0) return CellState.MaybeUnoccupied;
                else if (CellValue == 0) return CellState.DefinitelyUnoccupied;
                else throw new Exception("Cell value should not be negative");
            }

        }

        public Predicate<Slot> ConstraintPredicate => BetweenPredicate;
        public override string ToString() {
            return $"Click me if you don't want to have class from {_startTime}-{_startTime.Add(Time.CreateTime_24HourFormat(0,30))} on {_day}";
        }
    }
}
