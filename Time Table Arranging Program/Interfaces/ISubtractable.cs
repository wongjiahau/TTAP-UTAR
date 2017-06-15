namespace Time_Table_Arranging_Program.Interfaces {
    public interface ISubtractable<Tin, Tout> {
        Tout Minus(Tin other);
    }
}