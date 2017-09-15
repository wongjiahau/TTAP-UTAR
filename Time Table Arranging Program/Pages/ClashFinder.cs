using System;
using System.Collections.Generic;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.User_Control.CheckboxWithListDownMenuFolder.ErrorMessageType;

namespace Time_Table_Arranging_Program.Pages {
    /// <summary>
    /// This class is to store the state of each day of a subject. 
    /// StateOfEachDay example is like 100101101100000 
    /// 1 means definitely have class, 0 means else (maybe have class OR definitely no class)
    /// </summary>
    public class SubjectModelWithState {
        public int[ /*7*/] StateOfEachDay;
        public string SubjectName;

        public SubjectModelWithState(string subjectName, int[] stateOfEachDay) {
            SubjectName = subjectName;
            StateOfEachDay = stateOfEachDay;
        }

        public bool ClashesWith(SubjectModelWithState s) {
            for (int i = 0; i < Day.NumberOfDaysPerWeek; i++) {
                if ((StateOfEachDay[i] & s.StateOfEachDay[i]) > 0) return true;
            }
            return false;
        }
    }


    public class ClashFinder {
        private readonly List<SubjectModel> _subjectModels;
        private readonly List<SubjectModelWithState> _subjectStateList = new List<SubjectModelWithState>();
        private readonly SubjectModel _target;

        public ClashFinder(List<SubjectModel> subjectModels, Func<Slot[], List<List<Slot>>> permutator,
                           SubjectModel target) {
            _subjectModels = subjectModels;
            _target = target;
            var selectedSubjects = subjectModels.FindAll(x => x.IsSelected);
            for (var i = 0; i < selectedSubjects.Count; i++) {
                SubjectModel s = selectedSubjects[i];
                int[] subjectState = GetSubjectState(permutator(s.GetSelectedSlots().ToArray()));
                _subjectStateList.Add(new SubjectModelWithState(s.Name, subjectState));
            }
            for (int i = 0; i < _subjectStateList.Count; i++) {
                for (int j = 0; j < _subjectStateList.Count; j++) {
                    if (i == j) continue;
                    if (_subjectStateList[i].ClashesWith(_subjectStateList[j])) {
                        ClashingSubjects = (_subjectStateList[i], _subjectStateList[j]);
                        Message =
                            $"Because\n--{_subjectStateList[i].SubjectName}\nclashes with\n--{_subjectStateList[j].SubjectName}";
                        return;
                    }
                }
            }
            Message = $"Sorry... the reason is too complicated to be explained.";
            ClashingSubjects = null;
        }

        private (SubjectModelWithState, SubjectModelWithState)? ClashingSubjects { get; set; }
        private string Message { set; get; }

        public static int[] GetSubjectState(List<List<Slot>> outputTimetables) {
            var finalResult = new int[7] {-1, -1, -1, -1, -1, -1, -1};
            for (var i = 0; i < outputTimetables.Count; i++) {
                var timetable = outputTimetables[i];
                var stateOfThisTimetable = GetTimetableState(timetable);
                for (int k = 0; k < Day.NumberOfDaysPerWeek; k++) {
                    finalResult[k] &= stateOfThisTimetable[k];
                }
            }
            return finalResult;
        }

        public static int[] GetTimetableState(List<Slot> timetable) {
            var result = new int[7] {0, 0, 0, 0, 0, 0, 0};
            for (var j = 0; j < timetable.Count; j++) {
                var slot = timetable[j];
                result[slot.Day.IntValue - 1] |= slot.TimePeriod.ToBinary();
            }
            return result;
        }

        private SubjectModel WhoIsCrashingWithTarget() {
            if (ClashingSubjects == null) return null;
            var nameOfClashingCounterPart =
                ClashingSubjects.Value.Item1.SubjectName == _target.Name
                    ? ClashingSubjects.Value.Item2.SubjectName
                    : ClashingSubjects.Value.Item1.SubjectName;
            return _subjectModels.Find(x => x.Name == nameOfClashingCounterPart);
        }

        public ClashReport GetReport() {
            var clashingCounterpart = WhoIsCrashingWithTarget();
            return clashingCounterpart == null
                ? new ClashReport(ClashingErrorType.GroupClashingError, null)
                : new ClashReport(ClashingErrorType.SingleClashingError, clashingCounterpart);
        }
    }

    public class ClashReport {
        public ClashReport(ClashingErrorType clashingErrorType, SubjectModel clashingCounterpart) {
            ClashingErrorType = clashingErrorType;
            ClashingCounterpart = clashingCounterpart;
        }

        public ClashingErrorType ClashingErrorType { get; set; }
        public SubjectModel ClashingCounterpart { get; set; }
    }

    public class NullClashReport : ClashReport {
        public NullClashReport() : base(ClashingErrorType.NoError, null) { }
    }
}