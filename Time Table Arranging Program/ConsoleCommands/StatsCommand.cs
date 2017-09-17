using System.Collections.Generic;
using ConsoleTerminalLibrary.Console;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Model;

namespace Time_Table_Arranging_Program.ConsoleCommands {
    public class StatsCommand : ConsoleCommandBase {
        public StatsCommand(object commandee) : base(commandee) { }

        public override string Execute() {
            var inputSlots = Commandee as SlotList;
            var subjects = SubjectModel.Parse(inputSlots);
            int subjectCount = subjects.Count;
            if (subjectCount == 0) return "No slots is loaded yet.";
            int averageSlotsCountPerSubject = inputSlots.Count / subjectCount;
            (string subjectWithMostSlotCount, int maxSlotsCount) = GetMaxSlotCount(subjects);
            (string subjectWithLeastSlotCount, int minSlotsCount) = GetMinSlotCount(subjects);
            string result = "";
            result +=
                $"Total number of subjects = {subjectCount}\n" +
                $"Average slots count per subject = {averageSlotsCountPerSubject}\n" +
                $"Max slots count = {maxSlotsCount} ({subjectWithMostSlotCount})\n" +
                $"Min slots count = {minSlotsCount} ({subjectWithLeastSlotCount})\n"
                ;
            return result;
        }

        private (string, int) GetMinSlotCount(List<SubjectModel> subjects) {
            int min = subjects[0].Slots.Count;
            string minSlotSubjectName = subjects[0].Name;
            foreach (var subjectModel in subjects) {
                if (subjectModel.Slots.Count < min) {
                    min = subjectModel.Slots.Count;
                    minSlotSubjectName = subjectModel.Name;
                }
            }
            return (minSlotSubjectName, min);
        }

        private (string, int) GetMaxSlotCount(List<SubjectModel> subjects) {
            int max = subjects[0].Slots.Count;
            string maxSlotSubjectName = subjects[0].Name;
            foreach (var subjectModel in subjects) {
                if (subjectModel.Slots.Count > max) {
                    max = subjectModel.Slots.Count;
                    maxSlotSubjectName = subjectModel.Name;
                }
            }
            return (maxSlotSubjectName, max);
        }

        public override string Keyword() {
            return "stats";
        }

        public override string Help() {
            return "Get the stats of loaded slots.";
        }
    }
}