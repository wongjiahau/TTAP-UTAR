using System;
using System.Collections.ObjectModel;
using System.ComponentModel;

public class ConsoleContent : INotifyPropertyChanged {
    string _consoleInput = string.Empty;
    ObservableCollection<string> _consoleOutput = new ObservableCollection<string>() { "Console Emulation Sample..." };

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