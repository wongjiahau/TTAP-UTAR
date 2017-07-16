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

    [Obsolete("Incomplete")]
    public class ClashFinder {
        private List<SubjectModelWithState> _modelWithStates = new List<SubjectModelWithState>();
        public ClashFinder(List<SubjectModel> subjectModels, Func<Slot[],List<List<Slot>>> permutator) {
            var selectedSubjects = subjectModels.FindAll(x => x.IsSelected);
            for (var i = 0; i < selectedSubjects.Count; i++) {
                SubjectModel s = selectedSubjects[i];
                int[] stateOfEachDay = GetStateOfEachDay(permutator(s.GetSelectedSlots().ToArray()));
                _modelWithStates.Add(new SubjectModelWithState(s.Name, stateOfEachDay));
            }
            if (_modelWithStates[0].ClashesWith(_modelWithStates[1]))
                Message = $"[{_modelWithStates[0].SubjectName}] clashes with [{_modelWithStates[1].SubjectName}]";
        }

        private int[] GetStateOfEachDay(List<List<Slot>> outputTimetables) {
            var finalResult = new int[7] {-1, -1, -1, -1, -1, -1, -1}; 
            for (var i = 0; i < outputTimetables.Count; i++) {
                var timetable = outputTimetables[i];
                var result = new int[7] {0, 0, 0, 0, 0, 0, 0};
                for (var j = 0; j < timetable.Count; j++) {
                    var slot = timetable[j];
                    result[slot.Day.IntValue - 1] |= slot.TimePeriod.ToBinary();
                }
                for (int k = 0; k < Day.NumberOfDaysPerWeek; k++) {
                    finalResult[k] &= result[k];
                }
            }
            return finalResult;

        }

        public string Message { private set; get; }
    }
}