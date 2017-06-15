using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}