using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Annotations;

namespace Time_Table_Arranging_Program.Class.AbstractClass {
    public abstract class ObservableObject : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            Debug.Assert(GetType().GetProperty(propertyName) != null);
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propName = null) {
            if (!EqualityComparer<T>.Default.Equals(field, value)) {
                field = value;
                OnPropertyChanged(propName);
                return true;
            }
            return false;
        }
    }

    public class ExampleObservable : ObservableObject {
        private int _age;

        public int Age {
            get { return _age; }
            set { SetProperty(ref _age, value); }
        }
    }
}