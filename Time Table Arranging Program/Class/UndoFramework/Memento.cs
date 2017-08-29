namespace Time_Table_Arranging_Program.Class.UndoFramework {
    public class Memento {
        private object _state;

        public object GetState() {
            return _state;
        }

        private void SetState(object state) {
            _state = state;
        }
    }

}
