using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Tests2;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.Model
{
    public class SubjectModel
    {
        public SubjectModel()
        {
            Name = "Testing Subject 123";
            CodeAndNameInitials = "MPU329999";
            Slots = TestData.GetSlotRange(3,9);
        }
        public SubjectModel(string name, string code, int creditHour, List<Slot> slots)
        {
            Name = name;
            Code = code;
            CodeAndNameInitials = code + " [" + name.GetInitial() + "]";
            CreditHour = creditHour;
            Slots = slots;
        }

        public string Name { get; private set; }
        public string CodeAndNameInitials { get; private set; }
        public string Code { get; private set; }
        public int CreditHour { get; private set; }

        public List<Slot> Slots { get; private set; }

        public static List<SubjectModel> Parse(List<Slot> slots)
        {
            var result = new List<SubjectModel>();
            var dic = new Dictionary<string, List<Slot>>();
            foreach (Slot s in slots)
            {
                if (!dic.ContainsKey(s.Code))
                {
                    dic.Add(s.Code, new List<Slot>());
                }
                dic[s.Code].Add(s);
            }
            foreach (KeyValuePair<string, List<Slot>> entry in dic)
            {
                var v = entry.Value[0];
                result.Add(new SubjectModel(v.SubjectName, v.Code, 0, entry.Value));                
            }
            result = result.OrderBy(o => o.Name).ToList();
            return result;
        }
    }
}
