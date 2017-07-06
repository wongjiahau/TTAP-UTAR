using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.Model
{
    public class SubjectModel
    {
        public SubjectModel(string name, string code, int creditHour, List<Slot> slots)
        {
            Name = name;
            Code = code;
            CreditHour = creditHour;
            Slots = slots;
        }

        public string Name { get; private set; }
        public string Code { get; private set; }
        public int CreditHour { get; private set; }

        public List<Slot> Slots { get; private set; }
    }
}
