using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using ConsoleTerminalLibrary.HelperClass;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleTerminalLibrary.Console {
    public class ConsoleTerminalModel : INotifyPropertyChanged {
        private string _consoleInput = string.Empty;
        private ObservableCollection<string> _consoleOutput = new ObservableCollection<string>() { "Console Emulation Sample..." };
        private readonly List<IConsoleCommand> _commandList = new List<IConsoleCommand>();
        private readonly IBoundedIteratable<string> _inputHistory = new BoundedIteratable<string>();

        public ConsoleTerminalModel() {
            
        }
        public ConsoleTerminalModel(List<IConsoleCommand> commandList) {
            _commandList = commandList;
        }
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

        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged(string propertyName) {
            PropertyChanged?.Invoke(this , new PropertyChangedEventArgs(propertyName));
        }

        public void ExecuteCommand(string input) {
            _inputHistory.Add(_consoleInput);
            if (input.Last() == '?') {
                ShowHelp(input);
                return;
            }
            var command = _commandList.Find(x => x.Keyword == input);
            if (command == null) {
                ConsoleOutput.Add($"'{input}' is not a recognizable command.");
            }
            else {
                ConsoleOutput.Add(command.Execute());
            }
            ConsoleInput = "";
        }

        private void ShowHelp(string input) {
            Assert.IsTrue(input.Last() == '?');
            input = input.TrimEnd('?');
            var command = _commandList.Find(x => x.Keyword == input);
            if (command == null) {
                ConsoleOutput.Add($"'{input}' is not a recognizable command.");
            }
            else {
                ConsoleOutput.Add(command.Help);
            }

        }

        public void ShowMatchingCommand(string input) {
            var matched = _commandList.FindAll(x => x.Keyword == input);
            if (matched.Count == 0) {
                ConsoleOutput.Add($"No matching command starts with '{input}'");
            }
            else if (matched.Count == 1) {
                ConsoleInput = matched[0].Keyword;
            }
            else {
                ConsoleOutput.Add("Matching commands : ");
                foreach (var consoleCommand in matched) {
                    ConsoleOutput.Add("\t" + consoleCommand);
                }
            }
        }


        public void GoToPreviousCommand() {
            _inputHistory.GoToPrevious();
            ConsoleInput = _inputHistory.GetCurrent();
        }

        public void GoToNextCommand() {
            _inputHistory.GoToNext();
            ConsoleInput = _inputHistory.GetCurrent();
        }
    }
}