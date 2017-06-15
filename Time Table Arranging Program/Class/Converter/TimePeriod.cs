using System;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class.Converter {
    public interface ITimePeriod : IIntersectionCheckable<ITimePeriod>, IToConstructionString {
        Time StartTime { get; set; }
        Time EndTime { get; set; }
    }

    [Serializable]
    public class TimePeriod : ConvertibleToString, ITimePeriod, IDuplicable<TimePeriod> {
        public TimePeriod() {
            StartTime = Time.CreateTime_24HourFormat(0, 0);
            EndTime = Time.CreateTime_24HourFormat(0, 0);
        }

        public TimePeriod(ITime startTime, ITime endTime) {
            StartTime = (Time) startTime;
            EndTime = (Time) endTime;
        }

        public TimePeriod GetDuplicate() {
            var t = new TimePeriod();
            t.StartTime = StartTime;
            t.EndTime = EndTime;
            return t;
        }

        public Time StartTime { get; set; }
        public Time EndTime { get; set; }

        public bool IntersectWith(ITimePeriod other) {
            if (StartTime.MoreThanOrEqual(other.EndTime)) return false;
            if (EndTime.LessThanOrEqual(other.StartTime)) return false;
            return true;
        }

        public string ToConstructionString() {
            return $"new TimePeriod({StartTime.ToConstructionString()},{EndTime.ToConstructionString()})";
        }

        protected override string StringValue() {
            return $" {StartTime} - {EndTime}";
        }
    }
}