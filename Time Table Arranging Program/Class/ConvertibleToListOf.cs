using System.Collections.Generic;

namespace Time_Table_Arranging_Program.Class {
    public interface ConvertibleToListOf<T> {
        List<T> ToList();
    }
}