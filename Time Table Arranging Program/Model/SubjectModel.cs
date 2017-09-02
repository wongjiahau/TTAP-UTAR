using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.AbstractClass;

namespace Time_Table_Arranging_Program.Model {
    public class SubjectModel : ObservableObject {
        public SubjectModel() {
            Name = "Testing Subject 123";
            CodeAndNameInitials = "MPU329999";
            Slots = TestData.GetSlotRange(3 , 9);
        }
        public SubjectModel(string name , string code , int creditHour , List<Slot> slots) {
            Name = name;
            Code = code;
            CodeAndNameInitials = code + " [" + name.GetInitial() + "]";
            CreditHour = creditHour;
            Slots = slots;
        }

        public string Name { get; }
        public string CodeAndNameInitials { get; private set; }
        public string Code { get; }
        public int CreditHour { get; private set; }

        public event EventHandler Selected;
        public event EventHandler Deselected;
        private bool _isSelected;
        public bool IsSelected {
            get => _isSelected;
            set {
                SetProperty(ref _isSelected , value);
                if (value) Selected?.Invoke(this , null);
                else Deselected?.Invoke(this , null);
            }
        }

        public List<Slot> Slots { get; private set; }

        public List<Slot> GetSelectedSlots() {
            var result = new List<Slot>();
            for (int i = 0 ; i < Slots.Count ; i++) {
                if (Slots[i].IsSelected)
                    result.Add(Slots[i]);
            }
            return result;
        }

        public static List<SubjectModel> Parse(List<Slot> slots) {
            var result = new List<SubjectModel>();
            var dic = new Dictionary<string, List<Slot>>();
            for (var i = 0; i < slots.Count; i++) {
                Slot s = slots[i];
                if (!dic.ContainsKey(s.Code)) {
                    dic.Add(s.Code, new List<Slot>());
                }
                dic[s.Code].Add(s);
            }
            foreach (KeyValuePair<string , List<Slot>> entry in dic) {
                var v = entry.Value[0];
                result.Add(new SubjectModel(v.SubjectName , v.Code , 0 , entry.Value));
            }
            result = result.OrderBy(o => o.Name).ToList();
            return result;
        }
    }
}
