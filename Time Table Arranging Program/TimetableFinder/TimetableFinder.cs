using System;
using System.Collections.Generic;
using System.Linq;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.StateSummary;
using Time_Table_Arranging_Program.Model;

namespace Time_Table_Arranging_Program.TimetableFinder {
    public class TimetableFinder {
        public List<List<Slot>> GetPossibleTimetables(Slot[] inputSlots) {
            var subjects = SubjectModel.Parse(inputSlots.ToList());
            Func<Slot[] , List<List<Slot>>> permutator = Permutator.Run_v2_withoutConsideringWeekNumber;
            subjects = SortBySlotCount(subjects);
            var currentSlots = subjects[0].Slots;
            var possibleCombination = permutator.Invoke(currentSlots.ToArray());
            var state = StateTable.GetStateOfDefinitelyOccupied(possibleCombination);
            int last = subjects.Count - 1;
            for (int i = 1 ; i < subjects.Count ; i++) {
                var originalSchema = new SubjectSchema(subjects[i].Slots);
                var filtrate = StateTable.Filter(subjects[i].Slots , state);
                var newSchema = new SubjectSchema(filtrate);
                if (!originalSchema.Equals(newSchema)) return null;
                currentSlots.AddRange(filtrate);
                possibleCombination = permutator.Invoke(currentSlots.ToArray());
                if (i != last)
                    state = StateTable.GetStateOfDefinitelyOccupied(possibleCombination);
            }
            return possibleCombination;
        }

        public List<SubjectModel> SortBySlotCount(List<SubjectModel> subjects) {
            var input = subjects;
            var result = input.OrderBy(x => x.Slots.Count).ToList();
            return result;
        }
    }
}