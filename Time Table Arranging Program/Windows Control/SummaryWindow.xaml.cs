using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.Windows_Control {
    /// <summary>
    /// Interaction logic for SummaryWindow.xaml
    /// </summary>
    public partial class SummaryWindow : Window {
        private static SummaryWindow _singletonInstance;

        private int _currentIndex;
        private int _maxIndex;

        private ITimetableList _timetableList;

        private SummaryWindow() {
            InitializeComponent();
        }

        public static SummaryWindow GetSingletonInstance(ITimetableList timetableList) {
            if (_singletonInstance == null) {
                _singletonInstance = new SummaryWindow();
            }
            _singletonInstance._timetableList = timetableList;
            return _singletonInstance;
        }

        public void ShowWindow() {
            if (_timetableList.IsEmpty()) {
                return;
            }

            Show();
            Global.MainWindow.Hide();
            _currentIndex = 0;
            _maxIndex = _timetableList.ToList().Count - 1;
            DescriptionViewer.Update(_timetableList.ToList()[_currentIndex].ToList());
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e) {
            Global.MainWindow.Show();
            Hide();
        }

        private void SummaryWindow_OnClosing(object sender, CancelEventArgs e) {
            e.Cancel = true;
            if (Visibility == Visibility.Hidden) return;
            Global.MainWindow.Show();
            Hide();
        }


        private void ToggleViewButton_OnClick(object sender, RoutedEventArgs e) {
            TipsLabel.Visibility = Visibility.Collapsed;
            if (MainBorder.Visibility == Visibility.Visible) {
                MainBorder.Visibility = Visibility.Collapsed;
                ToggleViewButton.Content = "Maximize";
            }
            else {
                MainBorder.Visibility = Visibility.Visible;
                ToggleViewButton.Content = "Minimize";
            }
        }


        private void LeftButton_OnClick(object sender, RoutedEventArgs e) {
            DecrementCurrentIndex();
            DescriptionViewer.Update(_timetableList.ToList()[_currentIndex].ToList());
        }

        private void DecrementCurrentIndex() {
            _currentIndex--;
            if (_currentIndex < 0)
                _currentIndex = _maxIndex;
        }

        private void RightButton_OnClick(object sender, RoutedEventArgs e) {
            IncrementCurrentIndex();
            DescriptionViewer.Update(_timetableList.ToList()[_currentIndex].ToList());
        }

        private void IncrementCurrentIndex() {
            _currentIndex++;
            if (_currentIndex > _maxIndex)
                _currentIndex = 0;
        }
    }
}