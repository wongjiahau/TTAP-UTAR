using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program.Pages {
    /// <summary>
    /// Interaction logic for Page_ShowLikedSubject.xaml
    /// </summary>
    public partial class Page_ShowLikedTimetable : Page {
        private static Page_ShowLikedTimetable _singletonInstance;
        private ITimetableList _likedTimetableList;

        private Page_ShowLikedTimetable() {
            InitializeComponent();
            TimetableViewer.MessageToBeDisplayedWhenOutputTimetableIsEmpty =
                "👀 Seems like you haven't like any timetable yet ...";
        }

        public static Page_ShowLikedTimetable GetSingletonInstance(ITimetableList timetableList) {
            if (_singletonInstance == null) {
                _singletonInstance = new Page_ShowLikedTimetable();
            }
            _singletonInstance.UpdateTimetableViewer(timetableList);
            return _singletonInstance;
        }

        private void UpdateTimetableViewer(ITimetableList input) {
            List<ITimetable> temp = input.ToList().FindAll(x => x.IsLiked);
            if (_likedTimetableList == null) {
                _likedTimetableList = new TimetableList(temp);
            }
            else {
                _likedTimetableList.AddUniqueRange(temp);
                for (int i = 0; i < _likedTimetableList.ToList().Count; i++) {
                    var t = _likedTimetableList.ToList()[i];
                    if (t.IsLiked == false) {
                        _likedTimetableList.Remove(t);
                    }
                }
            }
            TimetableViewer.Update(_likedTimetableList.ToList(), false);
            ResetControlsVisibility();
        }

        private void RemoveButton_OnClick(object sender, RoutedEventArgs e) {
            ITimetable currentTimetable = TimetableViewer.GetCurrentTimetable();
            currentTimetable.IsLiked = false;
            _likedTimetableList.Remove(currentTimetable);
            UpdateTimetableViewer(_likedTimetableList);
        }

        private void ResetControlsVisibility() {
            Visibility visibility =
                _likedTimetableList.ToList().Count == 0
                    ? Visibility.Collapsed
                    : Visibility.Visible;

            TitleLabel.Visibility = visibility;
            RemoveButton.Visibility = visibility;
            ShowSummaryButton.Visibility = visibility;
        }

        private void ShowSummaryButton_OnClick(object sender, RoutedEventArgs e) {
            SummaryWindow.GetSingletonInstance(_likedTimetableList).ShowWindow();
        }
    }
}