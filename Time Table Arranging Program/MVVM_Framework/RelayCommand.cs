using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xaml;

namespace Time_Table_Arranging_Program.MVVM_Framework {
    public class RelayCommand<T> : ICommand {
        private readonly Func<T, bool> _canExecute;

        private readonly Action<T> _execute;

        public RelayCommand(Action<T> execute, Func<T, bool> canExecute = null) {
            if (execute == null) throw new ArgumentNullException("Execute cannot be null");
            _execute = execute;
            _canExecute = canExecute ?? CanExecute;
        }

        public bool CanExecute(object parameter) {
            return _canExecute(TranslateParameter(parameter));
        }

        public event EventHandler CanExecuteChanged {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter) {
            _execute(TranslateParameter(parameter));
        }

        private static bool CanExecute(T parameter) {
            return true;
        }

        private T TranslateParameter(object parameter) {
            T value = default(T);
            if (parameter != null && typeof(T).IsEnum)
                value = (T) Enum.Parse(typeof(T), (string) parameter);
            else
                value = (T) parameter;
            return value;
        }
    }

    public class RelayCommand : RelayCommand<object> {
        public RelayCommand(Action execute, Func<bool> canExecute = null) :
            base(obj => execute(),
                canExecute == null ? null : new Func<object, bool>(obj => canExecute())) {}
    }
}