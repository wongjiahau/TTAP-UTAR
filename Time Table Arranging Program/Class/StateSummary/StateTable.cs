using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Class.Helper;

namespace Time_Table_Arranging_Program.Class.StateSummary {
    public interface IStateTable : IEnumerable<IStateCell> { }

    public class StateTable : IStateTable {
        private readonly List<IStateCell> _stateCells;
        private int[][] _definitelyOccupiedState = new int[7][];

        private StateTable(List<List<Slot>> outputTimetables) {
            _stateCells = NewAlgorithmUsingBinary(outputTimetables);
        }

        public IEnumerator<IStateCell> GetEnumerator() {
            return _stateCells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public static StateTable Parse(List<List<Slot>> x) {
            return new StateTable(x);
        }

        /// <summary>
        /// 1 means DefinitelyOccupied
        /// 0 means not DefinitelyOccupied
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static int[] ParseString_AsStateOfDefinitelyOccupied(string input) {
            var result = new int[7];
            string[] tokens = input.Split('~');
            if (tokens.Length != 7) throw new Exception("Make sure that input contains six tilde (~).");
            for (int i = 0; i < tokens.Length; i++) {
                result[i] = Convert.ToInt32(tokens[i].Reverse(), 2);
            }
            return result;
        }


        public static int[][] GetStateOfAll(List<List<Slot>> timetables) {
            throw new NotImplementedException();
        }

        public static int[][] GetStateOfDefinitelyUnoccupied(List<List<Slot>> timetables) {
            throw new NotImplementedException();
        }

        public static int[] GetStateOfDefinitelyOccupied(List<List<Slot>> timetables) {
            var states = new int[7]
                {-1, -1, -1, -1, -1, -1, -1}; //Note : -1 = 11111111111111111111111111111111 in binary 
            var o = timetables;
            if (o == null) return null;
            for (int i = 0; i < o.Count; i++) {
                int[] result = new[] {0, 0, 0, 0, 0, 0, 0};
                for (int j = 0; j < o[i].Count; j++) {
                    var slot = o[i][j];
                    int day = slot.Day.IntValue - 1;
                    int timeperiodInBinary = slot.TimePeriod.ToBinary();
                    result[day] |= timeperiodInBinary;
                }
                for (int j = 0; j < 7; j++) {
                    states[j] &= result[j];
                }
            }
            return states;
        }

        public static int[][] GetStateOfUncertain(List<List<Slot>> timetables) {
            throw new NotImplementedException();
        }

        private List<IStateCell> NewAlgorithmUsingBinary(List<List<Slot>> outputTimetables) {
            var states = new Dictionary<int, int[]>();
            for (int i = 0; i < 7; i++) {
                states.Add(i, new int[2] {Convert.ToInt32("11111111111111111111111111111111", 2), 0});
                //1st array means AND-ed value, 2nd array means OR-ed value
                //1st cell is 32-bit of TRUE
                //2nd cell is 32-bit of FALSE
            }
            var o = outputTimetables;
            if (o == null) return new List<IStateCell>();
            for (int i = 0; i < o.Count; i++) {
                int[] result = new[] {0, 0, 0, 0, 0, 0, 0};
                for (int j = 0; j < o[i].Count; j++) {
                    var slot = o[i][j];
                    int day = slot.Day.IntValue - 1;
                    int timeperiodInBinary = slot.TimePeriod.ToBinary();
                    result[day] |= timeperiodInBinary;
                }
                for (int j = 0; j < 7; j++) {
                    states[j][0] &= result[j];
                    states[j][1] |= result[j];
                }
            }
            var stateCells = new List<IStateCell>();
            int length = 32;
            for (int i = 0; i < 7; i++) {
                var and_value = states[i][0].ToBitArray();
                var or_value = states[i][1].ToBitArray();
                Day day = Day.GetDay(i + 1);
                Time time = Time.CreateTime_24HourFormat(7, 0);
                for (int j = 0; j < length; j++) {
                    var c = new StateCell(i, j, day, time);
                    if (and_value[j] == true) c.State = CellState.DefinitelyOccupied;
                    else if (or_value[j] == false) c.State = CellState.DefinitelyUnoccupied;
                    else c.State = CellState.MaybeUnoccupied;
                    stateCells.Add(c);
                    time = (Time) time.Add(Time.CreateTime_24HourFormat(0, 30));
                }
            }
            return stateCells;
        }

        public static List<Slot> Filter(List<Slot> slots, int[] state) {
            var result = new List<Slot>();
            for (int i = 0; i < slots.Count; i++) {
                var currentSlot = slots[i];
                if ((currentSlot.TimePeriod.ToBinary() & state[currentSlot.Day.IntValue - 1]) == 0)
                    result.Add(currentSlot);
            }
            return result;
        }
    }

    public interface IStateCell {
        int RowIndex { get; }
        int ColumnIndex { get; }
        CellState State { get; }
        Predicate<Slot> ConstraintPredicate { get; }
    }

    public enum CellState {
        DefinitelyOccupied,
        MaybeUnoccupied,
        DefinitelyUnoccupied
    }

    public class StateCell : IStateCell {
        public const int ColumnSpanOfEachCell = 1;
        private readonly Day _day;
        private readonly Time _startTime;

        public StateCell(int rowIndex, int columnIndex, Day day, Time startTime) {
            RowIndex = rowIndex;
            ColumnIndex = columnIndex;
            _day = day;
            _startTime = startTime;
        }

        public CellState State { get; set; }
        public int RowIndex { get; private set; }
        public int ColumnIndex { get; private set; }


        public Predicate<Slot> ConstraintPredicate => BetweenPredicate;

        private bool BetweenPredicate(Slot s) {
            var timePeriod = new TimePeriod(_startTime, _startTime.Add(Time.CreateTime_24HourFormat(0, 30)));
            return
                s.Day.Equals(_day) &&
                s.TimePeriod.IntersectWith(timePeriod);
        }

        public override string ToString() {
            return
                $"Click me if you don't want to have class from {_startTime}-{_startTime.Add(Time.CreateTime_24HourFormat(0, 30))} on {_day}";
        }
    }
}