using System;
using System.Collections.Generic;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class {
    public class Slot : IEquatable<Slot>, IComparable<Slot>, IIntersectionCheckable<Slot>, IDuplicable<Slot>, IHashable,
        IToConstructionString {
        private static int _nextOid;

        private IDay _day;

        public Slot() {
            WeekNumber = new WeekNumber(new List<int>());
            OID = _nextOid++;
            IsSelected = false;
            StartTime = Time.CreateTime_24HourFormat(0, 0);
            EndTime = Time.CreateTime_24HourFormat(0, 0);
            _day = Day.Unassigned;
        }

        public Slot(int uid, string code, string subjectName, string number, string type, Day day, string venue,
                    TimePeriod timePeriod, WeekNumber weekNumber) {
            OID = _nextOid++;
            UID = uid;
            Code = code;
            SubjectName = subjectName;
            Number = number;
            Type = type;
            Day = day;
            Venue = venue;
            TimePeriod = timePeriod;
            WeekNumber = weekNumber;
            IsSelected = true;
        }

        public Slot(Slot s) {
            OID = _nextOid++;
            UID = s.UID;
            Code = s.Code;
            SubjectName = s.SubjectName;
            Number = s.Number;
            Type = s.Type;
            Day = s.Day;
            Venue = s.Venue;
            TimePeriod = s.TimePeriod;
            WeekNumber = s.WeekNumber;
            IsSelected = s.IsSelected;
        }

        public int OID { get; } //OID is unique for each object
        public int UID { get; set; } //public setters of UID is for serialization purpose only!
        public string Code { get; set; }
        public string SubjectName { get; set; }
        public string Number { get; set; }
        public string Type { get; set; }

        public Day Day {
            get { return (Day) _day; }
            set { _day = value; }
        }

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
            s.UID = UID;
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
                UID == b.UID &&
                SubjectName == b.SubjectName &&
                Code == b.Code &&
                Number == b.Number &&
                Type == b.Type &&
                Venue == b.Venue &&
                Day.Equals(b.Day) &&
                StartTime.Equals(b.StartTime) &&
                EndTime.Equals(b.EndTime) &&
                WeekNumber.Equals(b.WeekNumber);
        }

        public int GetUid() {
            return UID;
        }

        public bool IntersectWith(Slot other) {
            if (Code == other.Code && Type == other.Type && Number != other.Number) return true;
            if (!Day.Equals(other.Day)) return false;
            if (!TimePeriod.IntersectWith(other.TimePeriod)) return false;
            return WeekNumber.IntersectWith(other.WeekNumber);
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


        public bool EqualsExceptNumberAndVenue(Slot b) {
            return
                SubjectName == b.SubjectName &&
                Type == b.Type &&
                Day.Equals(b.Day) &&
                StartTime.Equals(b.StartTime) &&
                EndTime.Equals(b.EndTime) &&
                WeekNumber.Equals(b.WeekNumber)
                ;
        }

        public string ToFullString() {
            return
                $"({UID}) {Code} [{SubjectName}] {Type}{Number} {Day} {Venue} {TimePeriod} {WeekNumber}";
        }
    }
}