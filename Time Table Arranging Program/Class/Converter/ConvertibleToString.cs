namespace Time_Table_Arranging_Program.Class.Converter {
    public abstract class ConvertibleToString {
        public sealed override string ToString() {
            return StringValue();
        }

        protected abstract string StringValue();
    }
}