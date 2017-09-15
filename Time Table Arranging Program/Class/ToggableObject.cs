using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Class {
    public interface IToggableObject {
        bool IsToggledOn { get; }
        void Toggle();
    }

    public class ToggableObject : IToggableObject {
        private bool _isToggledOn = false;

        public ToggableObject(bool isToggledOn) {
            _isToggledOn = isToggledOn;
        }

        public void Toggle() {
            _isToggledOn = !_isToggledOn;
        }

        public bool IsToggledOn => _isToggledOn;
    }
}