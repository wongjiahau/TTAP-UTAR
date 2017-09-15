using System;
using System.Windows;
using System.Windows.Media;
using Time_Table_Arranging_Program.UserInterface;

namespace Time_Table_Arranging_Program.User_Control.TimeChooserCircular {
    /// <summary>
    ///     Interaction logic for TimeChooser_InnerPart_Window.xaml
    /// </summary>
    public interface ITimeChooserDialog {
        ITime ChosenTime { get; set; }
        void ShowDialog();
    }

    public partial class TimeChooser_InnerPart_Window : Window, ITimeChooserDialog {
        private ITime _initialPickedTime;
        private ITime _newChosenTime;

        public TimeChooser_InnerPart_Window(Time initialPickedTime) {
            InitializeComponent();
            _initialPickedTime = initialPickedTime;
            ChosenTime = initialPickedTime;
            Clock.TimeChanged += ClockOnTimeChanged;
            Clock.Color = TimeChooser.PrimaryColor;
            TopBorder.Background = TimeChooser.PrimaryColor;
            Circle.Fill = TimeChooser.ClockBackgroundColor;
            AmPmButton.SetContent("AM", "PM");
            AmPmButton.FontSize = 30;
            ChosenTime = Time.CreateTime_24HourFormat(13, 0);
        }

        public ITime ChosenTime {
            get { return _newChosenTime; }
            set {
                TimeLabel.Text = value.To12HourFormat(false);
                _newChosenTime = value;
                Clock.ChosenHour = value.Hour > 12 ? value.Hour - 12 : value.Hour;
                Clock.ChosenMinute = value.Minute;
                if (value.Hour >= 12) AmPmButton.SetCurrentAsValue2();
                else AmPmButton.SetCurrentAsValue1();
            }
        }

        public new void ShowDialog() {
            base.ShowDialog();
        }

        private void ClockOnTimeChanged(object sender, EventArgs eventArgs) {
            var newtime = Time.CreateTime_12HourFormat(Clock.ChosenHour, Clock.ChosenMinute,
                AmPmButton.GetCurrentValue() == "PM");
            TimeLabel.Text = newtime.To12HourFormat(false);
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e) {
            ChosenTime = Time.CreateTime_12HourFormat(Clock.ChosenHour, Clock.ChosenMinute,
                AmPmButton.GetCurrentValue() == "PM");
            _initialPickedTime = ChosenTime;
            Hide();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e) {
            ChosenTime = _initialPickedTime;
            Hide();
        }
    }
}