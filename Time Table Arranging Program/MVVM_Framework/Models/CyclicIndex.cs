using System;
using Time_Table_Arranging_Program.Class.AbstractClass;

namespace Time_Table_Arranging_Program.MVVM_Framework.Models {
    public class CyclicIndex : ObservableObject {
        private int _currentValue = -1;
        private int _maxValue = -1;

        public CyclicIndex(int maxValue) {
            _maxValue = maxValue;
            if (_maxValue < 0) {
                _currentValue = -1;
            }
            else {
                _currentValue = 0;
            }
        }

        public CyclicIndex() { }

        public int CurrentValue {
            get { return _currentValue; }
            set {
                int result;
                if (value > _maxValue) {
                    result = 0;
                }
                else if (value < 0) {
                    result = _maxValue;
                }
                else {
                    result = value;
                }
                SetProperty(ref _currentValue, result);
                CurrentValueChanged?.Invoke(this, null);
            }
        }

        public int MaxValue {
            get { return _maxValue; }
            set { SetProperty(ref _maxValue, value); }
        }

        public event EventHandler CurrentValueChanged;

        public void Reset() {
            MaxValue = -1;
            CurrentValue = -1;
        }

        public override string ToString() {
            return _currentValue + "/" + _maxValue;
        }
    }
}