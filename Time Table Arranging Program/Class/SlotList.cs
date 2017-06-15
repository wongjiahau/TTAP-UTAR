using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Class {
    public interface ISlotList {
        List<string> SelectedSubjectNames { get; set; }
        string[] GetNamesOfAllSubjects();
        string[] GetCodesOfAllSubjects();
        Slot[] GetSlotsOf(HashSet<int> uIDofSelectedSlots);
        List<Slot> GetSlotsOf(string subjectName);
        bool NoSlotIsChosen();
    }

    /// <summary>
    /// Decorator for List of Slot
    /// </summary>           
    [Serializable]
    public class SlotList : List<Slot>, ISlotList {
        public string[] GetNamesOfAllSubjects() {
            var result = new HashSet<string>();
            foreach (var s in this) {
                result.Add(s.SubjectName);
            }
            var r = result.ToList();
            r.Sort();
            return r.ToArray();
        }

        public string[] GetCodesOfAllSubjects() {
            var result = new HashSet<string>();
            foreach (var s in this) {
                result.Add(s.Code);
            }
            var r = result.ToList();
            r.Sort();
            return r.ToArray();
        }

        public Slot[] GetSlotsOf(HashSet<int> uIDofSelectedSlots) {
            var result = new List<Slot>();
            foreach (var s in this) {
                if (uIDofSelectedSlots.Contains(s.UID))
                    result.Add(s);
            }
            return result.ToArray();
        }

        public List<Slot> GetSlotsOf(string subjectName) {
            var result = new List<Slot>();
            foreach (var s in this) {
                if (s.SubjectName == subjectName) result.Add(s);
            }
            return result;
        }

        public bool NoSlotIsChosen() {
            var result = new List<Slot>();
            foreach (var s in this) {
                if (SelectedSubjectNames.Any(name => s.SubjectName == name)) {
                    result.Add(s);
                }
            }
            return result.Count == 0;
        }


        public List<string> SelectedSubjectNames { get; set; } = new List<string>();
    }
}