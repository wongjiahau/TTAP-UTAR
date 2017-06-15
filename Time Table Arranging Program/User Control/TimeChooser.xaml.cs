using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program {
    /// <summary>
    ///     Interaction logic for TimeChooser.xaml
    /// </summary>
    public interface ITime : IInequalityComparable<ITime>, IAddable<ITime>, ISubtractable<ITime, TimeSpan>,
        IToConstructionString {
        int Hour { get; set; }
        int Minute { get; set; }
        string To12HourFormat(bool withAmOrPmLabel);
    }

    public interface ITimeChooser {
        event RoutedEventHandler TimeChanged;
        ITime GetChosenTime();
    }

    public partial class TimeChooser : UserControl, ITimeChooser {
        public static readonly SolidColorBrush PrimaryColor = Brushes.CadetBlue;
        public static readonly SolidColorBrush SecondaryColor = Brushes.White;
        public static readonly SolidColorBrush ClockBackgroundColor = Brushes.WhiteSmoke;

        public TimeChooser() {
            InitializeComponent();
            PopulateCombobox(HourCB, MinuteCB);
        }

        public event RoutedEventHandler TimeChanged;

        public ITime GetChosenTime() {
            var hour = (int) HourCB.Items[HourCB.SelectedIndex];
            if (AmOrPmCB.SelectedIndex == 1 && hour != 12) hour += 12;
            else if (AmOrPmCB.SelectedIndex == 0 && hour == 12) hour = 0;
            var minute = (int) MinuteCB.Items[MinuteCB.SelectedIndex];
            return Time.CreateTime_24HourFormat(hour, minute);
        }

        private void PopulateCombobox(ComboBox hourCb, ComboBox minuteCb) {
            for (var i = 1; i <= 12; i++) {
                hourCb.Items.Add(i);
            }
            for (var i = 0; i <= 60; i += 15) {
                minuteCb.Items.Add(i);
            }
        }

        private void ComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            TimeChanged?.Invoke(this, e);
        }
    }
}