using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Time_Table_Arranging_Program.Class.Helper;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class.Converter {
    public interface IDay : IEquatable<IDay>, IToConstructionString, IConvertibleToBinary {
        string StringValue { get; set; }
        int IntValue { get; set; }
        DayOfWeek ToDayOfWeek();
    }

    [Serializable]
    public class Day : IDay {
        public static readonly Day Monday = new Day("Mon", 1);
        public static readonly Day Tuesday = new Day("Tue", 2);
        public static readonly Day Wednesday = new Day("Wed", 3);
        public static readonly Day Thursday = new Day("Thu", 4);
        public static readonly Day Friday = new Day("Fri", 5);
        public static readonly Day Saturday = new Day("Sat", 6);
        public static readonly Day Sunday = new Day("Sun", 7);
        public static readonly Day Unassigned = new Day("Unassigned", 0);
        public static readonly Day NullDay = new Day("Null", -1);
        public static readonly int NumberOfDaysPerWeek = 7;

        public static Day GetDay(int value) {
            return Days.Find(x => x.IntValue == value);
        }
        private static readonly List<Day> Days = new List<Day>
        {
            Monday,
            Tuesday,
            Wednesday,
            Thursday,
            Friday,
            Saturday,
            Sunday
        };

        private int _intValue;

        private string _stringValue;

        private Day() {
            //for serialization
        }

        private Day(string value, int intvalue) {
            _stringValue = value;
            _intValue = intvalue;
        }

        public string StringValue {
            get { return _stringValue; }
            set { _stringValue = value; } //setter is for serialization purpose
        }

        public int IntValue {
            get { return _intValue; }
            set { _intValue = value; } //setter is for serialization purpose
        }

        public bool Equals(IDay other) {
            return other.IntValue == _intValue;
        }

        public string ToConstructionString() {
            return $"Day.Parse(\"{StringValue}\")";
        }

        public DayOfWeek ToDayOfWeek() {
            switch (_intValue) {
                case 1:
                    return DayOfWeek.Monday;
                case 2:
                    return DayOfWeek.Tuesday;
                case 3:
                    return DayOfWeek.Wednesday;
                case 4:
                    return DayOfWeek.Thursday;
                case 5:
                    return DayOfWeek.Friday;
                case 6:
                    return DayOfWeek.Saturday;
                case 7:
                    return DayOfWeek.Sunday;
                default:
                    throw new Exception($"Cannot parse int value of {_intValue} to DayOfWeek");
            }
        }

        /// <summary>
        ///     Check if the string value is day
        /// </summary>
        /// <param name="value">Value are 3 characters long only, e.g. "Mon", case is insensitive</param>
        /// <returns></returns>
        public static bool IsDay(string value) {
            return Days.Any(x => x.StringValue.ToLower() == value.ToLower());
        }

        /// <summary>
        ///     Get an instance of Day based on integer value
        /// </summary>
        /// <param name="value">Value are 3 characters long, e.g. "Mon", case is insensitive</param>
        /// <returns>Return NullDay if unable to parse</returns>
        public static Day Parse(string value) {
            if (!IsDay(value)) return NullDay;
            return Days.Find(x => x.StringValue.ToLower() == value.ToLower());
        }

        /// <summary>
        ///     Get an instance of Day based on integer value
        /// </summary>
        /// <param name="value">1 means Monday, 7 means Sunday</param>
        /// <returns>Return NullDay if unable to parse</returns>
        public static Day Parse(int value) {
            if (!Days.Any(x => x.IntValue == value)) return NullDay;
            return Days.Find(x => x.IntValue == value);
        }

        public static Day Parse(DayOfWeek value) {
            switch (value) {
                case DayOfWeek.Monday:
                    return Monday;
                case DayOfWeek.Tuesday:
                    return Tuesday;
                case DayOfWeek.Wednesday:
                    return Wednesday;
                case DayOfWeek.Thursday:
                    return Thursday;
                case DayOfWeek.Friday:
                    return Friday;
                case DayOfWeek.Saturday:
                    return Saturday;
                case DayOfWeek.Sunday:
                    return Sunday;
                default:
                    return NullDay;
            }
        }

        public override string ToString() {
            return StringValue;
        }

        public int ToBinary() {
            var bitArray = new BitArray(7);
            int intValueInZeroBasedIndex = _intValue - 1;
            for (int i = 0; i < 7; i++) {
                if (i == intValueInZeroBasedIndex) {
                    bitArray[i] = true;
                }
                else {
                    bitArray[i] = false;
                }
            }
            return bitArray.ToInt();

        }
    }
}