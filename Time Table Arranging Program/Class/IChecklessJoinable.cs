namespace Time_Table_Arranging_Program.Class {
    public interface IChecklessJoinable<T> {
        T JoinWithoutChecking(T other);
    }
}