using System.Collections.Generic;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.Interfaces {
    public interface IHashedList<T> : IHashable, ICollection<T>, ConvertibleToListOf<T> { }
}