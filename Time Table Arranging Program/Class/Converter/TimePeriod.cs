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
    public class TimePeriod : ConvertibleToString, ITimePeriod, IDuplicable<TimePeriod>, IIntersectionCheckable<TimePeriod> {
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
        public int DataInBinary() {
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
            return (this._dataInBinary & (other)._dataInBinary) > 0;
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

        public int ToBinary() {
            throw new NotImplementedException();
        }
    }
}