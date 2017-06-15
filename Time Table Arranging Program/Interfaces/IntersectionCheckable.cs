namespace Time_Table_Arranging_Program.Class {
    public interface IIntersectionCheckable<T> {
        bool IntersectWith(T other);
    }
}