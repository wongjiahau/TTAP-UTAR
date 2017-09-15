using System.Collections.Generic;
using System.Linq;
using Time_Table_Arranging_Program.Class.Converter;

namespace Time_Table_Arranging_Program.Class.SlotGeneralizer {
    public interface ISlotGeneralizer {
        List<Slot> Generalize(List<Slot> slots);
    }

    public class SlotGeneralizer : ISlotGeneralizer {
        public List<Slot> Generalize(List<Slot> slots) {
            var shouldNotBeGeneralized = GetSlotsThatShouldNotBeGeneralized(slots);
            var shallBeGenerazlied = SetDifference(slots, shouldNotBeGeneralized);
            var duplicate = new List<Slot>();
            foreach (Slot s in shallBeGenerazlied) {
                duplicate.Add(s.GetDuplicate());
            }
            var generalized = new Dictionary<string, Slot>();
            for (int i = 0; i < duplicate.Count; i++) {
                var s = duplicate[i];
                string key = $"{s.Day}{s.Code}{s.Type}{s.TimePeriod}";
                if (!generalized.ContainsKey(key)) {
                    generalized.Add(key, s);
                }
                else {
                    generalized[key].Number += $"/{s.Number}";
                }
            }
            var result = generalized.Values.ToList();
            result.AddRange(shouldNotBeGeneralized);
            return result;
        }

        public List<Slot> SetDifference(List<Slot> setA, List<Slot> setB) {
            //A-B
            var result = new List<Slot>();
            foreach (var x in setA) {
                foreach (var y in setB) {
                    if (x.OID == y.OID) goto here;
                }
                result.Add(x);
                here:
                ;
            }
            return result;
        }

        public List<Slot> GetSlotsThatShouldNotBeGeneralized(List<Slot> slots) {
            var dic = new Dictionary<string, List<Slot>>();
            for (int i = 0; i < slots.Count; i++) {
                var s = slots[i];
                string key = $"{s.Code}{s.Type}{s.Number}";
                if (!dic.ContainsKey(key)) {
                    dic.Add(key, new List<Slot>());
                }
                dic[key].Add(s);
            }
            var result = new List<Slot>();
            foreach (var key in dic.Keys) {
                if (dic[key].Count > 1) {
                    result.AddRange(dic[key]);
                }
            }
            return result;
        }

        public IEnumerable<Slot> GeneralizeBySubject(List<Slot> slots) {
            var dic = new Dictionary<string, Slot>();
            for (int i = 0; i < slots.Count; i++) {
                var s = slots[i];
                string key = $"{s.Day}{s.Code}{s.Type}{s.TimePeriod}";
                if (!dic.ContainsKey(s.Code)) {
                    dic.Add(s.Code, s);
                }
                else {
                    dic[s.Code].Number += $"/{s.Number}({s.UID})";
                }
            }
            return dic.Values.ToList();
        }

        public List<List<Slot>> PartitionBySubject(List<Slot> slots) {
            var dic = new Dictionary<string, List<Slot>>();
            for (int i = 0; i < slots.Count; i++) {
                var s = slots[i];
                if (!dic.ContainsKey(s.Code)) {
                    dic.Add(s.Code, new List<Slot>());
                }
                dic[s.Code].Add(s);
            }
            return dic.Values.ToList();
        }
    }

    /// <summary>
    /// This generalizer will basically do nothing
    /// </summary>
    public class NullGeneralizer : ISlotGeneralizer {
        public List<Slot> Generalize(List<Slot> slots) {
            return slots;
        }
    }
}