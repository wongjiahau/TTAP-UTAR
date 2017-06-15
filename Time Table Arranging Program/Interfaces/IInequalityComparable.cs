using System;

namespace Time_Table_Arranging_Program.Interfaces {
    public interface IInequalityComparable<T> : IEquatable<T> {
        bool LessThan(T other);
        bool MoreThan(T other);
        bool LessThanOrEqual(T other);
        bool MoreThanOrEqual(T other);
    }
}