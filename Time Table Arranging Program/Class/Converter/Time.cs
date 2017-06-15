using System;
using Time_Table_Arranging_Program.Class.Converter;

namespace Time_Table_Arranging_Program {
    [Serializable]
    public class Time : ConvertibleToString, ITime {
        private Time(int hour, int minute) {
            Hour = hour;
            Minute = minute;
        }

        private Time() {
            //for serialization purpose only
        }

        private Time(string number, string amOrpm) {
            var tokens = number.Split(':');
            Hour = int.Parse(tokens[0]);
            Minute = int.Parse(tokens[1]);

            if (amOrpm.ToLower() == "pm" && Hour != 12) {
                Hour += 12;
            }
            else if (amOrpm.ToLower() == "am" && Hour == 12) {
                Hour = 0;
            }
        }

        public int Hour { get; set; }
        public int Minute { get; set; }

        public bool Equals(ITime other) {
            if (Hour != other.Hour) return false;
            if (Minute != other.Minute) return false;
            return true;
        }

        public bool LessThan(ITime other) {
            if (Hour < other.Hour) return true;
            if (Hour > other.Hour) return false;
            if (Minute < other.Minute) return true;
            return false;
        }

        public bool MoreThan(ITime other) {
            if (Hour > other.Hour) return true;
            if (Hour < other.Hour) return false;
            if (Minute > other.Minute) return true;
            return false;
        }

        public bool LessThanOrEqual(ITime other) {
            return LessThan(other) || Equals(other);
        }

        public bool MoreThanOrEqual(ITime other) {
            return MoreThan(other) || Equals(other);
        }

        public ITime Add(ITime other) {
            var finalMinutes = Minute + other.Minute;
            var finalHour = Hour + other.Hour;
            if (finalMinutes > 59) {
                finalHour++;
                finalMinutes %= 60;
            }
            return CreateTime_24HourFormat(finalHour, finalMinutes);
        }

        public TimeSpan Minus(ITime other) {
            if (LessThan(other)) throw new Exception("a is less than b, so a cannot be subtracted by b");
            var finalHour = Hour - other.Hour;
            var finalMinute = Minute - other.Minute;
            if (finalMinute < 0) {
                finalMinute = 60 + finalMinute;
                finalHour--;
            }
            return new TimeSpan(0, finalHour, finalMinute, 0);
        }

        public string ToConstructionString() {
            return $"Time.CreateTime_24HourFormat({Hour},{Minute})";
        }

        public string To12HourFormat(bool withAmOrPmLabel) {
            var temp = Hour > 12 ? Hour - 12 : Hour;
            var hour = temp < 10 ? " " + temp : temp.ToString();
            var minute = (Minute < 10 ? "0" : "") + Minute;
            var amOrPm = "";
            if (withAmOrPmLabel)
                amOrPm = Hour >= 12 ? "PM" : "AM";
            return $"{hour}:{minute} {amOrPm}";
        }

        /// <summary>
        /// Create a time instance based on the 12-hour format
        /// </summary>
        /// <param name="number">4 digits delimited by colon. E.g. : "01:45", the hour number should be between 1 to 12 only</param>
        /// <param name="amOrpm">"AM" or "PM" (Case is insensitive)</param>
        /// <returns></returns>
        public static Time CreateTime_12HourFormat(string number, string amOrpm) {
            return new Time(number, amOrpm);
        }

        public static Time CreateTime_24HourFormat(int hour, int minute) {
            return new Time(hour, minute);
        }

        /// <summary>
        /// Create a time instance based on the 12-hour format
        /// </summary>
        /// <param name="hour">Hour is between 1 to 12 only</param>
        /// <param name="minute"></param>
        /// <param name="isPm"></param>
        /// <returns></returns>
        public static Time CreateTime_12HourFormat(int hour, int minute, bool isPm) {
            if (hour < 1 || hour > 12) throw new Exception($"Expected value is between 1 to 12, but actual is {hour}");
            if (isPm && hour != 12) hour += 12;
            if (!isPm && hour == 12) hour = 0;
            var t = CreateTime_24HourFormat(hour, minute);
            return t;
        }

        protected override string StringValue() {
            return To12HourFormat(true);
        }
    }
}