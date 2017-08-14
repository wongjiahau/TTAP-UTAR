using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace Time_Table_Arranging_Program.Console {
    public class ConsoleContent : INotifyPropertyChanged {
        private string _consoleInput = string.Empty;
        private ObservableCollection<string> _consoleOutput = new ObservableCollection<string>() { "Console Emulation Sample..." };

        public string ConsoleInput {
            get {
                return _consoleInput;
            }
            set {
                _consoleInput = value;
                OnPropertyChanged("ConsoleInput");
            }
        }

        public ObservableCollection<string> ConsoleOutput {
            get {
                return _consoleOutput;
            }
            set {
                _consoleOutput = value;
                OnPropertyChanged("ConsoleOutput");
            }
        }

        public void RunCommand() {
            ConsoleOutput.Add(ConsoleInput);
            // do your stuff here.
            ConsoleInput = String.Empty;
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this , new PropertyChangedEventArgs(propertyName));
        }
    }
}