using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Model;

namespace Time_Table_Arranging_Program.Class.SubjectParser
{
    public class SubjectModelParser
    {
        public List<SubjectModel> Parse(List<Slot> slots)
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
            return result;
        }
    }
}
