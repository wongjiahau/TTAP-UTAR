using System.Collections.Generic;
using System.Linq;
using Time_Table_Arranging_Program.Class.Converter;

namespace Time_Table_Arranging_Program.Class.SlotGeneralizer {
    public class SlotGeneralizer {
        public List<Slot> GeneralizeAll(List<Slot> slots) {
            var generalized = new List<Slot>();
            foreach (Slot s in slots) {
                var toBeAdded = s.GetDuplicate();
               //toBeAdded.WeekNumber = new NullWeekNumber();
                generalized.Add(toBeAdded);
            }

            var dic = new Dictionary<string , Slot>();
            for (int i = 0 ; i < generalized.Count ; i++) {
                var s = generalized[i];
                string key = $"{s.Day}{s.Code}{s.Type}{s.TimePeriod}";
                if (!dic.ContainsKey(key)) {
                    dic.Add(key , s);
                }
                else {
                    dic[key].Number += $"/{s.Number}";

                }
            }
            return dic.Values.ToList();

        }

        public IEnumerable<Slot> GeneralizeBySubject(List<Slot> slots) {
            var dic = new Dictionary<string , Slot>();
            for (int i = 0 ; i < slots.Count ; i++) {
                var s = slots[i];
                string key = $"{s.Day}{s.Code}{s.Type}{s.TimePeriod}";
                if (!dic.ContainsKey(s.Code)) {
                    dic.Add(s.Code , s);
                }
                else {
                    dic[s.Code].Number += $"/{s.Number}({s.UID})";

                }
            }
            return dic.Values.ToList();
        }

        public List<List<Slot>> PartitionBySubject(List<Slot> slots) {
            var dic = new Dictionary<string , List<Slot>>();
            for (int i = 0 ; i < slots.Count ; i++) {
                var s = slots[i];
                if (!dic.ContainsKey(s.Code)) {
                    dic.Add(s.Code , new List<Slot>());
                }
                dic[s.Code].Add(s);
            }
            return dic.Values.ToList();
        }
    }
}
