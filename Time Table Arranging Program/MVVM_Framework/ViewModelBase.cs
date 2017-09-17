using Time_Table_Arranging_Program.Class.AbstractClass;

namespace Time_Table_Arranging_Program.MVVM_Framework {
    public abstract class ViewModelBase : ObservableObject { }

    public abstract class ViewModelBase<TModel> : ObservableObject {
        private TModel _model;

        protected ViewModelBase() { }

        protected ViewModelBase(TModel model) {
            _model = model;
        }

        public TModel Model {
            get { return _model; }
            set { SetProperty(ref _model, value); }
        }
    }
}