using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Time_Table_Arranging_Program.MVVM_Framework.Models;
using Time_Table_Arranging_Program.Pages;

namespace Time_Table_Arranging_Program.MVVM_Framework.ViewModels {
    public class BoundedIndexVM : ViewModelBase<CyclicIndex> {
        private ICommand _decrementCommand;
        private ICommand _incrementCommand;

        public BoundedIndexVM() { }

        public BoundedIndexVM(CyclicIndex model) : base(model) {
            model.CurrentValueChanged += Model_CurrentValueChanged;
        }

        public ICommand IncrementCommand {
            get {
                return
                    _incrementCommand ??
                    (_incrementCommand = new RelayCommand(() => {
                        if (IncrementIsEnabled) CurrentValue++;
                    }));
            }
        }

        public ICommand DecrementCommand {
            get {
                return
                    _decrementCommand ??
                    (_decrementCommand = new RelayCommand(() => {
                        if (DecrementIsEnabled) CurrentValue--;
                    }));
            }
        }


        public bool DecrementIsEnabled => Model?.CurrentValue > 0;
        public bool IncrementIsEnabled => Model?.CurrentValue < Model?.MaxValue;


        public int CurrentValue {
            get { return Model?.CurrentValue ?? 0; }
            set {
                Model.CurrentValue = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(DecrementIsEnabled));
                OnPropertyChanged(nameof(IncrementIsEnabled));
            }
        }

        private void Model_CurrentValueChanged(object sender, EventArgs e) {
            OnPropertyChanged("CurrentValue");
        }
    }
}