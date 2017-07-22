using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Threading;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.SlotGeneralizer;
using Time_Table_Arranging_Program.Class.TokenParser;
using Time_Table_Arranging_Program.UserInterface;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program.Pages {
    /// <summary>
    ///     Interaction logic for Page_AddSlot.xaml
    /// </summary>
    public partial class Page_AddSlot : Page {
        private readonly List<string> _previousInputString = new List<string>();

        public Page_AddSlot() {
            InitializeComponent();
            InnerSp.Height = 0;
        }

        private void InputSlotsListView_MouseRightButtonDown(object sender , MouseButtonEventArgs e) { }

        private void InputSlotsListView_KeyDown(object sender , KeyEventArgs e) {
            if (e.Key == Key.Delete)
                DeleteSelectedItem();
        }

        private void MenuItemDelete_Click(object sender , RoutedEventArgs e) {
            if (InputSlotsListView.SelectedIndex == -1) {
                DialogBox.Show("Hey..." , "You selected nothing to be deleted.", "OK");
                return;
            }
            DeleteSelectedItem();
        }

        private void DeleteSelectedItem() {
            var toBeDeleted = new Slot();
            foreach (var s in Global.InputSlotList) {
                if (s.ToString() == InputSlotsListView.SelectedValue.ToString())
                    toBeDeleted = s;
            }
            Global.InputSlotList.Remove(toBeDeleted);
            UpdateListView(Global.InputSlotList);
        }

        private void UpdateListView(List<Slot> input) {
            InputSlotsListView.Items.Clear();
            for (int i = 0 ; i < input.Count ; i++) {
                var s = input[i];
                InputSlotsListView.Items.Add(
                    new Slot {
                        SubjectName = s.SubjectName ,
                        Code = s.Code ,
                        Day = s.Day ,
                        StartTime = s.StartTime ,
                        EndTime = s.EndTime ,
                        Type = s.Type ,
                        Number = s.Number ,
                        WeekNumber = s.WeekNumber
                    });
            }
            Dispatcher.Invoke(new Action(() => { AnimateHiddenContent(true); }) , DispatcherPriority.ContextIdle , null);
            // AnimateInnerSp(true);
        }


        private void AddSlotButton_Click(object sender , RoutedEventArgs e) {
            var input = Clipboard.GetText();
            if (_previousInputString.Any(s => s.Contains(input))) {
                DialogBox.Show("Erm..." , "The content you copied is already added to the program just now.");
                return;
            }
            var previousCount = Global.InputSlotList.Count;
            var bg = CustomBackgroundWorker<string , List<Slot>>.RunAndShowLoadingScreen
                (new SlotParser().Parse , input , "Loading time slots . . .");
            Global.InputSlotList.AddRange(bg.GetResult());
            if (Global.InputSlotList.Count == previousCount) {
                DialogBox.Show("Please use GOOGLE CHROME" ,
                    "Unable to load data, please make sure you copied the correct content from the course registration website using Google Chrome.");
                return;
            }
            NOAS_Label.Content = Global.InputSlotList.Count;
            _previousInputString.Add(input);
            UpdateListView(Global.InputSlotList);
            CountingBadge.Badge = Global.InputSlotList.Count;
            Global.MaxTime = FindMaxTime(Global.InputSlotList);
            GetStartDateAndEndDate(input);
            //new Window_GetConstructionStringOfSlots(Database.inputSlots).Show();
        }

        private void GetStartDateAndEndDate(string input) {
            try {
                var parser = new StartDateEndDateFinder(input);
                Global.TimetableStartDate = parser.GetStartDate();
                Global.TimetableEndDate = parser.GetEndDate();
            }
            catch { }
        }

        private int FindMaxTime(List<Slot> inputSlots) {
            ITime max = inputSlots[0].EndTime;
            for (int i = 1 ; i < inputSlots.Count ; i++) {
                Slot s = inputSlots[i];
                if (s.EndTime.MoreThan(max))
                    max = s.EndTime;
            }
            return max.Hour > 0 ? max.Hour + 1 : max.Hour;
        }


        private void ResetButton_Click(object sender , RoutedEventArgs e) {
            Global.InputSlotList.Clear();
            Global.InputSlotList.SelectedSubjectNames.Clear();
            _previousInputString.Clear();
            UpdateListView(Global.InputSlotList);
            NOAS_Label.Content = "0";
            AnimateHiddenContent(false);
            CountingBadge.Badge = null;
        }

        private void AnimateHiddenContent(bool IsExpand) {
            double totalHeight = CreateTimetableButton.ActualHeight;
            if (IsExpand) {
                if (InnerSp.Height == 0) {
                    InnerSp.BeginAnimation(HeightProperty ,
                        CustomAnimation.GetEnteringScreenAnimation(0 , totalHeight , false));
                    ResetButton.IsEnabled = true;
                    AddSlotButton.Content = "Add more slots";
                }
            }
            else {
                InnerSp.BeginAnimation(HeightProperty , CustomAnimation.GetLeavingScreenAnimation(totalHeight , 0 , false));
                ResetButton.IsEnabled = false;
                AddSlotButton.Content = "Add slots";
            }
        }


        private void CreateTimetableButton_Click(object sender , RoutedEventArgs e) {
            NavigationService.Navigate(Page_CreateTimetable.GetInstance(Global.Settings.SearchByConsideringWeekNumber,
                Global.Settings.GeneralizeSlot));   
        }
    }
}