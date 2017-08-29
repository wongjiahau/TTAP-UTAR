using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using ConsoleTerminalLibrary.BuildIn_Command;
using ConsoleTerminalLibrary.HelperClass;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ConsoleTerminalLibrary.Console {
    public class ConsoleTerminalModel : INotifyPropertyChanged {
        private string _consoleInput = string.Empty;
        private ObservableCollection<string> _consoleOutput = new ObservableCollection<string>() { "Type '/help' to list available commands." };
        private readonly List<IConsoleCommand> _commandList;
        private readonly IBoundedIteratable<string> _inputHistory = new BoundedIteratable<string>();

        public ConsoleTerminalModel() {
            //Adding built in command
            _commandList = new List<IConsoleCommand>();
            _commandList.AddRange(
                new List<IConsoleCommand>()
                {
                    new HelpCommand(_commandList),
                    new ClearScreenCommand(_consoleOutput),
                    //new CopyToClipboardCommand(null),
                    new HistoryCommand(_inputHistory.ToList())
                });
        }
        public ConsoleTerminalModel(List<IConsoleCommand> commandList) : this() {
            _commandList.AddRange(commandList);
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
            _inputHistory.RemoveAll(x => x == "");
            _inputHistory.Add(input);
            _inputHistory.Add("");
            _inputHistory.GoToLast();
            ConsoleOutput.Add(input);
            if (input == "") return;
            if (input != "" && input.Last() == '?') {
                ShowHelp(input);
                return;
            }
            string commandKeyword = input.Split(' ')[0];
            var command = _commandList.Find(x => x.Keyword() == commandKeyword);
            if (command != null)
                if (command is ConsoleCommandWithArgument) {
                    if (input.Split(' ').Length != 2) {
                        ConsoleOutput.Add($"'{input}' must be invoked with one argument.");
                    }
                    else {
                        ConsoleOutput.Add((command as ConsoleCommandWithArgument).ExecuteCommand(input.Split(' ')[1]));
                    }
                }
                else {
                    ConsoleOutput.Add($"{command.Execute()}");
                }
            else ConsoleOutput.Add($"'{input}' is not a recognizable command.");
            ConsoleInput = "";
        }

        private void ShowHelp(string input) {
            Assert.IsTrue(input.Last() == '?');
            input = input.TrimEnd('?');
            var command = _commandList.Find(x => x.Keyword() == input);
            if (command == null) {
                ConsoleOutput.Add($"'{input}' is not a recognizable command.");
            }
            else {
                ConsoleOutput.Add(command.Help());
            }

        }

        public void ShowMatchingCommand(string input) {
            if (input == "") return;
            if (input.Contains(' ')) {
                ShowPossibleArgument(input);
                return;
            }
            var matched = _commandList.FindAll(x => x.Keyword().StartsWith(input , true , CultureInfo.InvariantCulture));
            if (matched.Count == 0) {
                ConsoleOutput.Add($"Error : No matching command starts with '{input}'");
            }
            else if (matched.Count == 1) {
                ConsoleInput = matched[0].Keyword();
            }
            else {
                List<string> matchedCommands = matched.Select(consoleCommand => consoleCommand.Keyword()).ToList();
                ConsoleInput = GetCommonStartingSubstring(matchedCommands);
                ConsoleOutput.Add("Matching commands : ");
                foreach (var consoleCommand in matched) {
                    ConsoleOutput.Add("\t" + consoleCommand.Keyword());
                }
            }
        }

        private string GetCommonStartingSubstring(List<string> matched) {
            string result = "";
            string shortestString = GetShortestString(matched);
            for (int i = 0 ; i < shortestString.Length ; i++) {
                char currentChar = shortestString[i];
                for (int j = 1 ; j < matched.Count ; j++) {
                    if (matched[j][i] != currentChar) return result;
                }
                result += currentChar;
            }
            return result;
        }

        private string GetShortestString(List<string> matched) {
            int minLength = matched[0].Length;
            int index = 0;
            for (int i = 1 ; i < matched.Count ; i++) {
                if (matched[i].Length < minLength) {
                    minLength = matched[i].Length;
                    index = i;
                }
            }
            return matched[index];
        }

        private void ShowPossibleArgument(string input) {
            string command = input.Split(' ')[0];
            var matched = _commandList.Find(x => x.Keyword() == command);
            if (matched == null) {
                ConsoleOutput.Add($"Error : '{input}' is not a recognizable command.");
                return;
            }
            if (!(matched is ConsoleCommandWithArgument)) {
                ConsoleOutput.Add($"Error : '{input}' does not take any arguments.");
                return;
            }
            string arg = input.Split(' ')[1];
            var possibleArguments = (matched as ConsoleCommandWithArgument).Arguments();
            var matchedArg = possibleArguments.ToList().FindAll(x => x.StartsWith(arg , true , CultureInfo.InvariantCulture));
            if (matchedArg.Count == 0) {
                ConsoleOutput.Add($"Error : No matching arguments with '{arg}'");
            }
            else if (matchedArg.Count == 1) {
                ConsoleInput = command + " " + matchedArg[0];
            }
            else {
                string result = "";
                foreach (var x in matchedArg) {
                    result += "\t" + x + "\n";
                }
                ConsoleInput = command + " " + GetCommonStartingSubstring(matchedArg);
                ConsoleOutput.Add("Matching arguments: ");
                ConsoleOutput.Add(result);
            }
        }

        public void GoToPreviousCommand() {
            _inputHistory.GoToPrevious();
            if (!_inputHistory.IsEmpty())
                ConsoleInput = _inputHistory.GetCurrent();
        }

        public void GoToNextCommand() {
            _inputHistory.GoToNext();
            if (!_inputHistory.IsEmpty())
                ConsoleInput = _inputHistory.GetCurrent();
        }
    }
}