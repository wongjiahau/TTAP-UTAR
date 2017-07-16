using System;
using System.Collections.Generic;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Converter;
using Time_Table_Arranging_Program.Model;

namespace Time_Table_Arranging_Program.Pages {


    /// <summary>
    /// This class is to store the state of each day of a subject. 
    /// StateOfEachDay example is like 100101101100000 
    /// 1 means definitely have class, 0 means else (maybe have class OR definitely no class)
    /// </summary>
    public class SubjectModelWithState {
        public string SubjectName;
        public int[/*7*/] StateOfEachDay;
        public SubjectModelWithState(string subjectName, int[] stateOfEachDay) {
            SubjectName = subjectName;
            StateOfEachDay = stateOfEachDay;
        }

        public bool ClashesWith(SubjectModelWithState s) {
            for (int i = 0; i < Day.NumberOfDaysPerWeek; i++) {
                if ((this.StateOfEachDay[i] & s.StateOfEachDay[i]) > 0) return true;
            }
            return false;
        }
    }

    [Obsolete("Untested")]
    public class ClashFinder {
        private List<SubjectModelWithState> _modelWithStates = new List<SubjectModelWithState>();
        public ClashFinder(List<SubjectModel> subjectModels, Func<Slot[],List<List<Slot>>> permutator) {
            var selectedSubjects = subjectModels.FindAll(x => x.IsSelected);
            for (var i = 0; i < selectedSubjects.Count; i++) {
                SubjectModel s = selectedSubjects[i];
                int[] stateOfEachDay = GetSubjectState(permutator(s.GetSelectedSlots().ToArray()));
                _modelWithStates.Add(new SubjectModelWithState(s.Name, stateOfEachDay));
            }
            for (int i = 0; i < _modelWithStates.Count; i++) {
                for (int j = 0; j < _modelWithStates.Count; j++) {
                    if (i == j) continue;
                    if (_modelWithStates[i].ClashesWith(_modelWithStates[j])) {
                        Message = $"[{_modelWithStates[i].SubjectName}] clashes with [{_modelWithStates[j].SubjectName}]";
                        return;
                    }
                }
            }
            Message = $"Sorry... the reason is too complicated to be explained.";
        }

        public int[] GetSubjectState(List<List<Slot>> outputTimetables) {
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

        public int[] GetTimetableState(List<Slot> timetable) {
            var result = new int[7] { 0 , 0 , 0 , 0 , 0 , 0 , 0 };
            for (var j = 0 ; j < timetable.Count ; j++) {
                var slot = timetable[j];
                result[slot.Day.IntValue - 1] |= slot.TimePeriod.ToBinary();
            }
            return result;
        }

        public string Message { private set; get; }
    }
}