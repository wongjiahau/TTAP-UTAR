using System;
using System.Collections.Generic;
using System.Linq;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.StateSummary;
using Time_Table_Arranging_Program.Model;

namespace Time_Table_Arranging_Program.TimetableFinder {
    public interface ITimetableFinder {
        List<List<Slot>> GetPossibleTimetables(List<SubjectModel> subjects , Func<Slot[] , List<List<Slot>>> permutator);
    }

    public class TimetableFinder : ITimetableFinder {
        public List<List<Slot>> GetPossibleTimetables(List<SubjectModel> subjects , Func<Slot[] , List<List<Slot>>> permutator) {
            throw new NotImplementedException();
            subjects = SortBySlotCount(subjects);
            var currentSlots = subjects[0].Slots;
            var possibleCombination = permutator.Invoke(currentSlots.ToArray());
            var state = StateTable.Parse(possibleCombination);
            for (int i = 1 ; i < subjects.Count ; i++) {
                var filtrate = StateTable.Filter(subjects[i].Slots , state);
                //currentSlots.Add(filtrate.ToArray());
            }

        }

        public List<SubjectModel> SortBySlotCount(List<SubjectModel> subjects) {
            var input = subjects;
            var result = input.OrderBy(x => x.Slots.Count).ToList();
            return result;
        }
    }
}
