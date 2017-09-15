using System;
using System.Windows.Input;
using Time_Table_Arranging_Program.MVVM_Framework.Models;

namespace Time_Table_Arranging_Program.MVVM_Framework.ViewModels {
    public class CyclicIndexVM : ViewModelBase<CyclicIndex> {
        private ICommand _decrementCommand;
        private ICommand _incrementCommand;

        private ICommand _toFirstCommand;

        private ICommand _toLastCommand;

        private ICommand _toRandomCommand;


        public CyclicIndexVM() { }

        public CyclicIndexVM(CyclicIndex model) : base(model) {
            model.CurrentValueChanged += Model_CurrentValueChanged;
        }

        public ICommand IncrementCommand {
            get {
                return
                    _incrementCommand ??
                    (_incrementCommand = new RelayCommand(() => { CurrentValue++; }))
                    ;
            }
        }

        public ICommand DecrementCommand {
            get {
                return
                    _decrementCommand ??
                    (_decrementCommand = new RelayCommand(() => { CurrentValue--; }));
            }
        }

        public ICommand ToRandomCommand {
            get {
                return
                    _toRandomCommand ??
                    (_toRandomCommand = new RelayCommand(() => {
                        CurrentValue = new Random().Next(0, Model.MaxValue);
                    }))
                    ;
            }
        }

        public ICommand ToFirstCommand {
            get {
                return _toFirstCommand ??
                       (_toFirstCommand = new RelayCommand(() => { CurrentValue = 1; }))
                    ;
            }
        }

        public ICommand ToLastCommand {
            get {
                return _toLastCommand ??
                       (_toFirstCommand = new RelayCommand(() => { CurrentValue = MaxValue; }))
                    ;
            }
        }


        public int CurrentValue {
            get { return Model?.CurrentValue + 1 ?? 0; }
            set {
                Model.CurrentValue = value - 1;
                OnPropertyChanged();
            }
        }

        public int MaxValue => Model?.MaxValue + 1 ?? 0;

        private void Model_CurrentValueChanged(object sender, EventArgs e) {
            OnPropertyChanged("CurrentValue");
        }
    }
}