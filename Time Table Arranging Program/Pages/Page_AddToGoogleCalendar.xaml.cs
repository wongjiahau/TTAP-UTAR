using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ExtraTools;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.GoogleCalendarApi;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program.Pages {
    /// <summary>
    /// Interaction logic for Page_AddToGoogleCalendar.xaml
    /// </summary>
    public partial class Page_AddToGoogleCalendar : Page {
        private readonly ITimetable _timetable;
        private int _counter;
        private DateTime _dateOfMondayOfWeekOne;
        private BackgroundWorker _worker;

        public Page_AddToGoogleCalendar(ITimetable timetable, DateTime dateOfMondayOfWeekOne) {
            InitializeComponent();
            _timetable = timetable;
            _dateOfMondayOfWeekOne = dateOfMondayOfWeekOne;
            if (_dateOfMondayOfWeekOne.DayOfWeek != DayOfWeek.Monday) {
                InstructionLabel.Visibility = Visibility.Visible;
                return;
            }
            OpenDialogHost(_dateOfMondayOfWeekOne);
        }

        private void OpenDialogHost(DateTime dateOfMondayOfWeekOne) {
            DateTextBlock.Text = _dateOfMondayOfWeekOne.ToString("d-MMMM-yyyy");
            DialogHost.IsOpen = true;
        }


        private void ChooseDateButton_OnClick(object sender, RoutedEventArgs e) {
            DatePicker.IsDropDownOpen = true;
        }


        private void Dialog_AddButton_OnClick(object sender, RoutedEventArgs e) {
            DisableAllButton();
            ProgressBar.Visibility = Visibility.Visible;
            _worker = new BackgroundWorker();
            _worker.DoWork += _worker_DoWork;
            _worker.RunWorkerCompleted += _worker_RunWorkerCompleted;
            _worker.RunWorkerAsync();
        }

        private void _worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Error != null) {
                if (_counter == 1) {
                    DialogBox.Show("Oops . . .", "Something went wrong. . .", e.Error.Message);
                    return;
                }
                _counter++;
                _worker.RunWorkerAsync();
            }
            else {
                DialogBox.Show("Congratulations!",
                    "The timetable had been successfully added to your Google Calendar.");
                NavigationService.GoBack();
            }
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e) {
            GoogleCalendarApi.AddTimetableToCalendar(_timetable, _dateOfMondayOfWeekOne);
        }

        private void DisableAllButton() {
            CancelButton.Visibility = Visibility.Hidden;
            AddButton.Visibility = Visibility.Hidden;
            RepickDateButton.IsEnabled = false;
        }

        private void DatePicker_OnCalendarClosed(object sender, RoutedEventArgs e) {
            if (DatePicker.SelectedDate == null) return;
            if (DatePicker.SelectedDate.Value.DayOfWeek != DayOfWeek.Monday) {
                DialogBox.Show("Something is not right . . .", "The date you picked is not a Monday.",
                    "Repick date");
                DatePicker.IsDropDownOpen = true;
                return;
            }

            _dateOfMondayOfWeekOne = DatePicker.SelectedDate.Value;
            OpenDialogHost(_dateOfMondayOfWeekOne);
        }

        private void RepickDateButton_OnClick(object sender, RoutedEventArgs e) {
            DialogHost.IsOpen = false;
            DatePicker.IsDropDownOpen = false;
            DatePicker.IsDropDownOpen = true;
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e) {
            DialogHost.IsOpen = false;
            NavigationService.GoBack();
        }

        private void DatePicker_OnCalendarOpened(object sender, RoutedEventArgs e) {
            DatePicker.DisplayDateStart = DateTime.Now;
            DatePicker.DisplayDateEnd = DateTime.Now + TimeSpan.FromDays(100);
            var minDate = DatePicker.DisplayDateStart ?? DateTime.MinValue;
            var maxDate = DatePicker.DisplayDateEnd ?? DateTime.MaxValue;

            for (var d = minDate; d <= maxDate && DateTime.MaxValue > d; d = d.AddDays(1)) {
                if (d.DayOfWeek != DayOfWeek.Monday) {
                    DatePicker.BlackoutDates.Add(new CalendarDateRange(d));
                }
            }
        }
    }
}