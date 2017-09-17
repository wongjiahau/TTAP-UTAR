using System;
using System.Collections.Generic;
using System.Linq;

namespace Time_Table_Arranging_Program.Class {
    public static class Filterer {
        public static Slot[] Filter(Slot[] inputSlots, List<Predicate<Slot>> predicateList) {
            var copy = inputSlots.ToList();
            foreach (var p in predicateList) {
                var i = 0;
                while (true) {
                    if (i == copy.Count) break;
                    var s = copy[i];
                    if (p(s)) {
                        copy = RemoveRelated(s, copy);
                        if (copy == null) return null;
                        i = -1;
                    }
                    i++;
                }
            }
            return copy.ToArray();
        }

        private static List<Slot> RemoveRelated(Slot toBeRemoved, List<Slot> slotList) {
            var copy = new List<Slot>(slotList);
            copy.RemoveAll(
                x =>
                    x.SubjectName == toBeRemoved.SubjectName &&
                    x.Type == toBeRemoved.Type &&
                    x.Number == toBeRemoved.Number);
            if (!copy.Any(x => x.SubjectName == toBeRemoved.SubjectName && x.Type == toBeRemoved.Type))
                return null;
            return copy;
        }

        public static List<List<Slot>> Filter(TimetablesAndPredicates package) {
            return Filter(package.Timetables, package.Predicates);
        }

        public static List<List<Slot>> Filter(List<List<Slot>> timetables, List<Predicate<Slot>> predicateList) {
            var result = new List<List<Slot>>();
            for (int i = 0; i < timetables.Count; i++) {
                for (int j = 0; j < timetables[i].Count; j++) {
                    for (int k = 0; k < predicateList.Count; k++) {
                        var slot = timetables[i][j];
                        if (predicateList[k](slot)) goto here;
                    }
                }
                result.Add(timetables[i]);
                here:
                ;
            }
            return result;
        }
    }

    public class TimetablesAndPredicates {
        public List<Predicate<Slot>> Predicates;

        public List<List<Slot>> Timetables;

        public TimetablesAndPredicates(List<List<Slot>> timetables, List<Predicate<Slot>> predicates) {
            Timetables = timetables;
            Predicates = predicates;
        }
    }
}