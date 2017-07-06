using System;
using System.Collections.Generic;
using System.Linq;
using Time_Table_Arranging_Program.Class.Helper;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class.Converter {
    public interface IWeekNumber : IToConstructionString, IConvertibleToBinary {
        void Clear();
        int Max();
    }

    [Serializable]
    public class WeekNumber : ConvertibleToString, IWeekNumber, IEquatable<IWeekNumber>,
        IIntersectionCheckable<WeekNumber> {
        private List<int> _weekNumberList;
        public int _weekNumberInBinary; //0101 means week 2 and week 4


        protected WeekNumber() {
            _weekNumberList = new List<int>();
        }

        public WeekNumber(List<int> weekNumberList) : this() {
            _weekNumberList = weekNumberList;
            _weekNumberInBinary = weekNumberList.ToBitArray().ToInt();

        }
        
        public WeekNumber(WeekNumber w) : this() {
            _weekNumberList = w._weekNumberList;
            _weekNumberInBinary = w._weekNumberInBinary;
        }

        public List<int> WeekNumberList {
            get { return _weekNumberList; }
            set
            {
                _weekNumberList = value;
                _weekNumberInBinary = _weekNumberList.ToBitArray().ToInt();
            } //for serialization purpose only
        }


        public bool Equals(IWeekNumber other) {
            return _weekNumberList.ScrambledEquals(((WeekNumber)other)._weekNumberList);
        }

        public bool IntersectWith(WeekNumber w) {
            return (_weekNumberInBinary & w._weekNumberInBinary) > 0;
            //return _weekNumberList.Intersect(w._weekNumberList).Any();
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
            var result = new List<int>();
            var tokens = s.Split(',');
            foreach (var t in tokens) {
                if (t.IsInteger()) {
                    result.Add(int.Parse(t));
                }
                else //is range of value 
                {
                    var toks = t.Split('-');
                    var first = int.Parse(toks.First());
                    var last = int.Parse(toks.Last());
                    for (var i = first ; i <= last ; i++) {
                        result.Add(i);
                    }
                }
            }
            return new WeekNumber(result);
        }

        protected override string StringValue() {
            if (_weekNumberList.Count == 0) return "error";
            var w = _weekNumberList.ToArray();
            var result = "" + w[0];
            for (var i = 0 ; i < w.Length - 1 ; i++) {
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

        public int ToBinary() {
            return _weekNumberInBinary;
        }
    }

    public class NullWeekNumber : WeekNumber {
        protected override string StringValue() {
            return "-";
        }
    }
}