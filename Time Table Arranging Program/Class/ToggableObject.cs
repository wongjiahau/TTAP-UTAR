namespace Time_Table_Arranging_Program.Class {
    public interface IToggableObject {
        bool IsToggledOn { get; }
        void Toggle();
    }

    public class ToggableObject : IToggableObject {
        private bool _isToggledOn;

        public ToggableObject(bool isToggledOn) {
            _isToggledOn = isToggledOn;
        }

        public void Toggle() {
            _isToggledOn = !_isToggledOn;
        }

        public bool IsToggledOn => _isToggledOn;
    }
}