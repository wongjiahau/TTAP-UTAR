namespace Time_Table_Arranging_Program.Interfaces {
    public interface IIndexable<T> {
        T this[int index] { get; set; }
    }
}