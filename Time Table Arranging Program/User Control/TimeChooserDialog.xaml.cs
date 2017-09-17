using System;
using System.Windows;
using MaterialDesignThemes.Wpf;
using Time_Table_Arranging_Program.User_Control.TimeChooserCircular;

namespace Time_Table_Arranging_Program.User_Control {
    /// <summary>
    /// Interaction logic for TimeChooserDialog.xaml
    /// </summary>
    public partial class TimeChooserDialog : Window, ITimeChooserDialog {
        private static TimeChooserDialog _singleton;

        private ITime _previousChosenTime;

        private TimeChooserDialog() {
            InitializeComponent();
        }

        public ITime ChosenTime {
            get { return _previousChosenTime; }
            set { _previousChosenTime = value; }
        }

        public new void ShowDialog() {
            base.ShowDialog();
        }

        public static TimeChooserDialog GetInstance(ITime initialTime) {
            if (_singleton == null)
                _singleton = new TimeChooserDialog();
            _singleton.Clock.DisplayMode = ClockDisplayMode.Hours;
            _singleton.Clock.Time = new DateTime(1, 1, 1, initialTime.Hour, initialTime.Minute, 0);
            _singleton._previousChosenTime = initialTime;
            return _singleton;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e) {
            Hide();
            Clock.Time = new DateTime(1, 1, 1, _previousChosenTime.Hour, _previousChosenTime.Minute, 0);
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e) {
            _previousChosenTime.Hour = Clock.Time.Hour;
            _previousChosenTime.Minute = Clock.Time.Minute;
            Hide();
        }
    }
}