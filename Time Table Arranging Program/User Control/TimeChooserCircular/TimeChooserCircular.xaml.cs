using System.Windows;
using System.Windows.Controls;

namespace Time_Table_Arranging_Program.User_Control.TimeChooserCircular {
    /// <summary>
    ///     Interaction logic for TimeChooserCircular.xaml
    /// </summary>
    public partial class TimeChooserCircular : UserControl, ITimeChooser {
        private ITime _chosenTime = Time.CreateTime_24HourFormat(13, 0);
        private ITimeChooserDialog _timeChooserDialog;

        public TimeChooserCircular() {
            InitializeComponent();
        }

        public event RoutedEventHandler TimeChanged;

        public ITime GetChosenTime() {
            return _chosenTime;
        }

        private void ChooseTimeButton_OnClick(object sender, RoutedEventArgs e) {
            _timeChooserDialog = TimeChooserDialog.GetInstance(_chosenTime);
            _timeChooserDialog.ShowDialog();

            _chosenTime = _timeChooserDialog.ChosenTime;
            ChooseTimeButton.Content = _chosenTime.ToString();
        }
    }
}