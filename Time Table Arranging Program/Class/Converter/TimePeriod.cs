using System;
using System.Collections;
using Time_Table_Arranging_Program.Class.Helper;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class.Converter {
    public interface ITimePeriod : IToConstructionString, IConvertibleToBinary {
        Time StartTime { get; set; }
        Time EndTime { get; set; }
    }

    [Serializable]
    public class TimePeriod : ConvertibleToString, ITimePeriod, IDuplicable<TimePeriod>, IIntersectionCheckable<TimePeriod>, IEquatable<TimePeriod> {
        public TimePeriod() {
            StartTime = Time.CreateTime_24HourFormat(0 , 0);
            EndTime = Time.CreateTime_24HourFormat(0 , 0);
        }

        public TimePeriod(ITime startTime , ITime endTime) {
            StartTime = (Time)startTime;
            EndTime = (Time)endTime;
        }

        private int GenerateBinaryData(ITime startTime, ITime endTime) {
            if (startTime == null || endTime == null) return -1;
            if (endTime.LessThan(startTime)) return -1;
            var i = new SlotIndex(Day.Monday , StartTime , EndTime.Minus(StartTime));
            var bitArray = new BitArray(32);
            for (int j = 0 ; j < i.ColumnSpan ; j++) {
                bitArray[i.ColumnIndex + j] = true;
            }
            return bitArray.ToInt();
        }

        public TimePeriod GetDuplicate() {
            var t = new TimePeriod();
            t.StartTime = StartTime;
            t.EndTime = EndTime;
            t._dataInBinary = _dataInBinary;
            return t;
        }

        private int _dataInBinary;
        public int ToBinary() {
            return _dataInBinary;
        }

        private Time _startTime;
        public Time StartTime {
            get {
                return _startTime;

            }
            set {
                _startTime = value;
                _dataInBinary = GenerateBinaryData(_startTime,_endTime);
            }
        }
        private Time _endTime;
        public Time EndTime {
            get {
                return _endTime;
            }
            set {
                _endTime = value;
                _dataInBinary = GenerateBinaryData(_startTime,_endTime);
            }
        }

        public bool IntersectWith(TimePeriod other) {
            return (_dataInBinary & (other)._dataInBinary) > 0;
            // if (StartTime.MoreThanOrEqual(other.EndTime)) return false;
            // if (EndTime.LessThanOrEqual(other.StartTime)) return false;
            //  return true;
        }

        public string ToConstructionString() {
            return $"new TimePeriod({StartTime.ToConstructionString()},{EndTime.ToConstructionString()})";
        }

        protected override string StringValue() {
            return $" {StartTime} - {EndTime}";
        }

        public static TimePeriod Parse(string data) {
            var tokens = data.Split('-');
            var startTime = tokens[0].Trim();
            var endTime = tokens[1].Trim();
            var result = new TimePeriod();
            result.StartTime = Time.Parse(startTime);
            result.EndTime = Time.Parse(endTime);
            return result;
        }


        public bool Equals(TimePeriod other) {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return _dataInBinary == other._dataInBinary &&
                   _startTime.Equals(other._startTime) &&
                   _endTime.Equals(other._endTime);                                
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((TimePeriod) obj);
        }
     
    }
}