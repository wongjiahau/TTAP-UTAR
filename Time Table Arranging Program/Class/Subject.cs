using System.Collections.Generic;

namespace Time_Table_Arranging_Program.Class {
    public interface ISubject {
        Timetable_Obsolete GetClashlessTimetable(int iterator);
        int GetUpperLimitOfIterator();
    }

    public class Subject : ISubject {
        private readonly List<Timetable_Obsolete> _clashlessCombination = new List<Timetable_Obsolete>();
        private string _subjectCode;


        public Timetable_Obsolete GetClashlessTimetable(int iterator) {
            return _clashlessCombination[iterator];
        }

        public int GetUpperLimitOfIterator() {
            return _clashlessCombination.Count - 1;
        }

        public static Subject CreateNewInstance(Slot[] s, string subjectCode) {
            var subject = new Subject();
            subject._subjectCode = subjectCode;
            var combinations = PermutateV4.Run_v2(s);
            for (var i = 0; i < combinations.Count; i++) {
                subject._clashlessCombination.Add(Timetable_Obsolete.CreateNew(combinations[i]));
            }
            //  subject._clashlessCombination =  PermutateV4.Run_v2(s);
            return subject;
        }
    }
}