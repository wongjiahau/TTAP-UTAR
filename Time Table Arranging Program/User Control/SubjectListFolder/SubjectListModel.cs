using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Time_Table_Arranging_Program.Class.AbstractClass;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.MVVM_Framework;

namespace Time_Table_Arranging_Program.User_Control.SubjectListFolder {
    public class SubjectListModel : ObservableObject {
        private List<SubjectModel> _subjectModels;

        public SubjectListModel() { }

        public SubjectListModel(List<SubjectModel> subjectModels) {
            _subjectModels = subjectModels;
            foreach (var subjectModel in _subjectModels) {
                subjectModel.Selected += SubjectModel_Selected;
                subjectModel.Deselected += SubjectModel_Deselected;
            }
        }


        public List<SubjectModel> ToList() {
            return _subjectModels;
        }

        #region ViewModelProperties

        #region DisplayModeProperty

        public enum DisplayModeEnum {
            DisplayAllSubject,
            DisplaySelectedSubject
        }

        private DisplayModeEnum _displayMode;

        public DisplayModeEnum DisplayMode {
            get => _displayMode;
            set {
                SetProperty(ref _displayMode , value);
                switch (value) {
                    case DisplayModeEnum.DisplayAllSubject:
                        foreach (var subjectModel in _subjectModels) subjectModel.IsVisible = true;
                        break;
                    case DisplayModeEnum.DisplaySelectedSubject:
                        foreach (var subjectModel in _subjectModels) subjectModel.IsVisible = subjectModel.IsSelected;
                        break;
                }
            }
        }

        private ICommand _toggleDisplayModeCommand;
        public ICommand ToggleDisplayModeCommand {
            get {
                return _toggleDisplayModeCommand ??
                       (_toggleDisplayModeCommand = new RelayCommand(() => {
                           switch (DisplayMode) {
                               case DisplayModeEnum.DisplayAllSubject:
                                   DisplayMode = DisplayModeEnum.DisplaySelectedSubject;
                                   break;
                               case DisplayModeEnum.DisplaySelectedSubject:
                                   DisplayMode = DisplayModeEnum.DisplayAllSubject;
                                   break;
                           }
                       }));
            }
        }

        #endregion

        #region NoSubjectSelectedProperty

        private int _selectedSubjectCount;
        public int SelectedSubjectCount {
            get => _selectedSubjectCount;
            set => SetProperty(ref _selectedSubjectCount, value);
        }
        
        private void SubjectModel_Selected(object sender , EventArgs e) {
            SelectedSubjectCount++;
        }

        private void SubjectModel_Deselected(object sender , EventArgs e) {
            SelectedSubjectCount--;
        }

        #endregion

        #endregion
    }




}

