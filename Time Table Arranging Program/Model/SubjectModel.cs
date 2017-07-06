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
        public SubjectModel()
        {
            Name = "Testing Subject 123";
            Code = "MPU329999";
            Slots = NUnit.Tests2.TestData.GetSlotRange(3,9);
        }
        public SubjectModel(string name, string code, int creditHour, List<Slot> slots)
        {
            Name = name;
            Code = code + " [" + name.GetInitial() + "]";
            CreditHour = creditHour;
            Slots = slots;
        }

        public string Name { get; private set; }
        public string Code { get; private set; }
        public int CreditHour { get; private set; }

        public List<Slot> Slots { get; private set; }
    }
}
