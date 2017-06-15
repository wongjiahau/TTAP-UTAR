using System;
using System.Collections.Generic;
using System.Linq;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class.Converter {
    public interface IWeekNumber : IToConstructionString {
        void Clear();
        int Max();
    }

    [Serializable]
    public class WeekNumber : ConvertibleToString, IWeekNumber, IEquatable<IWeekNumber>,
        IIntersectionCheckable<WeekNumber> {
        private List<int> _weekNumberList;

        private WeekNumber() {
            _weekNumberList = new List<int>();
        }

        public WeekNumber(List<int> weekNumberList) : this() {
            _weekNumberList = weekNumberList;
        }

        public WeekNumber(WeekNumber w) : this() {
            _weekNumberList = w._weekNumberList;
        }

        public List<int> WeekNumberList {
            get { return _weekNumberList; }
            set { _weekNumberList = value; } //for serialization purpose only
        }


        public bool Equals(IWeekNumber other) {
            return _weekNumberList.ScrambledEquals(((WeekNumber) other)._weekNumberList);
        }

        public bool IntersectWith(WeekNumber w) {
            return _weekNumberList.Intersect(w._weekNumberList).Any();
        }

        public void Clear() {
            _weekNumberList = new List<int>();
        }

        public string ToConstructionString() {
            return $"WeekNumber.Parse(\"{StringValue()}\")";
        }

        public int Max() {
            return _weekNumberList.Max();
        }

        public static WeekNumber Parse(string s) {
            var result = new WeekNumber();
            var tokens = s.Split(',');
            foreach (var t in tokens) {
                if (t.IsInteger()) {
                    result._weekNumberList.Add(int.Parse(t));
                }
                else //is range of value 
                {
                    var toks = t.Split('-');
                    var first = int.Parse(toks.First());
                    var last = int.Parse(toks.Last());
                    for (var i = first; i <= last; i++) {
                        result._weekNumberList.Add(i);
                    }
                }
            }
            return result;
        }

        protected override string StringValue() {
            if (_weekNumberList.Count == 0) return "error";
            var w = _weekNumberList.ToArray();
            var result = "" + w[0];
            for (var i = 0; i < w.Length - 1; i++) {
                if (w[i] + 1 == w[i + 1]) {
                    if (result[result.Length - 1] != '-')
                        result += "-";
                    if (i == w.Length - 2 || w[i + 1] + 1 != w[i + 2]) {
                        result += w[i + 1];
                    }
                }
                else {
                    if (result[result.Length - 1] != '-') {
                        result += ",";
                        result += w[i + 1];
                    }
                }
            }

            return result;
        }
    }
}