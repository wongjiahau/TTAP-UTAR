using System;
using System.Collections.Generic;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class {
    public class Slot : IEquatable<Slot>, IComparable<Slot>, IIntersectionCheckable<Slot>, IDuplicable<Slot>, IHashable,
        IToConstructionString {
        private static int _nextUid;

        private IDay _day;

        public Slot() {
            WeekNumber = new WeekNumber(new List<int>());
            UID = _nextUid++;
            IsSelected = false;
            StartTime = Time.CreateTime_24HourFormat(0, 0);
            EndTime = Time.CreateTime_24HourFormat(0, 0);
            _day = Day.Unassigned;
        }

        public Slot(int uid, string code, string subjectName, string number, string type, Day day, string venue,
                    TimePeriod timePeriod, WeekNumber weekNumber, bool isSelected) {
            UID = uid;
            Code = code;
            SubjectName = subjectName;
            Number = number;
            Type = type;
            Day = day;
            Venue = venue;
            TimePeriod = timePeriod;
            WeekNumber = weekNumber;
            IsSelected = isSelected;
        }

        public int UID { get; set; } //public setters of UID is for serialization purpose only!
        public string Code { get; set; }
        public string SubjectName { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }

        public Day Day {
            get { return (Day) _day; }
            set { _day = value; }
        }

        //public string Day { get; set; }
        public string Venue { get; set; }

        public Time StartTime {
            get { return TimePeriod.StartTime; }
            set { TimePeriod.StartTime = value; }
        }

        public Time EndTime {
            get { return TimePeriod.EndTime; }
            set { TimePeriod.EndTime = value; }
        }

        public TimePeriod TimePeriod { get; set; } = new TimePeriod();
        //public setters are for serialization purpose only

        public WeekNumber WeekNumber { get; set; }

        public bool IsSelected { get; set; }

        public int CompareTo(Slot obj) {
            var s = obj;
            return StringComparer.Ordinal.Compare(SubjectName, s.SubjectName);
        }

        public Slot GetDuplicate() {
            var s = new Slot();
            s.UID = _nextUid++;
            s.Code = Code;
            s.SubjectName = SubjectName;
            s.Number = Number;
            s.Type = Type;
            s.Day = Day;
            s.Venue = Venue;
            s.TimePeriod = TimePeriod.GetDuplicate();
            s.WeekNumber = new WeekNumber(WeekNumber);
            s.IsSelected = IsSelected;
            return s;
        }


        public bool Equals(Slot b) {
            return
                SubjectName == b.SubjectName &&
                Number == b.Number &&
                Type == b.Type &&
                Day.Equals(b.Day) &&
                StartTime.Equals(b.StartTime) &&
                EndTime.Equals(b.EndTime);
        }

        public int GetUid() {
            return UID;
        }

        public bool IntersectWith(Slot other) {
            if (Code == other.Code && Type == other.Type && Number != other.Number) return true;

            if (!Day.Equals(other.Day)) return false;
            if (TimePeriod.IntersectWith(other.TimePeriod)) {
                if (WeekNumber.IntersectWith(other.WeekNumber)) return true;
            }
            return false;
        }

        public string ToConstructionString() {
            new Slot();
            return
                $"new Slot({UID},\"{Code}\", \"{SubjectName}\", \"{Number}\", \"{Type}\", {Day.ToConstructionString()}, \"{Venue}\", {TimePeriod.ToConstructionString()}, {WeekNumber.ToConstructionString()}, {IsSelected.ToString().ToLower()})";
        }

        public override string ToString() {
            return
                Code + " : \t" +
                Type + " " +
                Number + " " +
                SubjectName + " ";
        }


        public bool EqualsExceptNumber(Slot b) {
            return
                SubjectName == b.SubjectName &&
                Type == b.Type &&
                Day.Equals(b.Day) &&
                StartTime.Equals(b.StartTime) &&
                EndTime.Equals(b.EndTime) &&
                WeekNumber == b.WeekNumber
                ;
        }
    }
}