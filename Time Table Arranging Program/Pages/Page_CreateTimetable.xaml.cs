using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms.VisualStyles;
using Microsoft.Win32;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Helper;
using Time_Table_Arranging_Program.Class.SlotGeneralizer;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.MVVM_Framework.Models;
using Time_Table_Arranging_Program.MVVM_Framework.ViewModels;
using Time_Table_Arranging_Program.User_Control;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program.Pages {
    /// <summary>
    ///     Interaction logic for Page_SelectSubject.xaml
    /// </summary>
    public partial class Page_CreateTimetable : Page, IDirtyObserver<IOutputTimetableModel>, IPageWithLoadedFunction {
        public static Page_CreateTimetable GetInstance(Setting searchByConsideringWeekNumber,
                                                       Setting generalizeSlot) {
            ISlotGeneralizer generalizer = generalizeSlot.IsChecked
                ? (ISlotGeneralizer)new SlotGeneralizer()
                : new NullGeneralizer();
            var permutator = searchByConsideringWeekNumber.IsChecked
                ? (Func<Slot[], List<List<Slot>>>) Permutator.Run_v2_WithConsideringWeekNumber
                : Permutator.Run_v2_withoutConsideringWeekNumber;
            var result = new SlotList();
            result.AddRange(generalizer.Generalize(Global.InputSlotList).ToArray());
            return new Page_CreateTimetable(result, permutator);
        }
        private readonly MutableObservable<ITimetable> _currentViewedTimetable = new ObservableTimetable(Timetable.Empty);
        private CyclicIndex _cyclicIndex;

        private readonly ObservableTimetableList _outputTimetables =
            new ObservableTimetableList(TimetableList.NoSlotsIsChosen);

        private readonly SlotList _inputSlots;
        private Window_StateSummary _windowStateSummary;
        private List<Predicate<Slot>> _predicates = new List<Predicate<Slot>>();

        private MutableObservable<IOutputTimetableModel> _timetableList;
        private readonly Func<Slot[],List<List<Slot>>> _permutator;

        private Page_CreateTimetable(SlotList inputSlots , Func<Slot[],List<List<Slot>>> permutator) {
            _inputSlots = inputSlots;
            _permutator = permutator;
            InitializeComponent();
            FavouriteButton.SetObservedThings(_outputTimetables);
            FavouriteButton.SetObservedThings(_currentViewedTimetable);
            TimetableViewer.SetObservedThings(_outputTimetables);
            TimetableViewer.Initialize(new CyclicIndex());
            _cyclicIndex = new CyclicIndex();
            CyclicIndexView.DataContext = new CyclicIndexVM(_cyclicIndex);
            InitializeExtraComponents();
            ToolBoxPanel.Visibility = Visibility.Hidden;

        }


        public void SetObservedThings(MutableObservable<IOutputTimetableModel> x) {
            x.RegisterObserver(this);
            _timetableList = x;
        }

        public void Update() {
            _timetableList.GetCurrentState();
        }

        private List<SubjectModel> _subjectModels;
        private void InitializeExtraComponents() {
            _subjectModels = SubjectModel.Parse(_inputSlots);
            SelectSubjectPanel.SetDataContext(_subjectModels);
            SelectSubjectPanel.SetDrawerHost(this.DrawerHost);
            FavouriteButton.CheckedMessage = "Added this timetable to favorites ";
            FavouriteButton.UncheckedMessage = "Removed this timetable from favorites";
        }

        private void UpdateGUI(List<List<Slot>> result) {
            _raw = result;
            _cyclicIndex = new CyclicIndex();
            if (result == null || result.Count == 0) {
                if (_inputSlots.NoSlotIsChosen()) {
                    _outputTimetables.SetState(TimetableList.NoSlotsIsChosen);
                    AutoCloseNotificationBar.Show("No subject selected.");
                }
                else {
                    _outputTimetables.SetState(TimetableList.NoPossibleCombination);
                    NotificationBar.Show("No possible timetable found.", "Tell me why", ()=>
                        {
                            DialogBox.Show("Why no possible combination?", new ClashFinder(_subjectModels, _permutator).Message);
                        }, false);
                }
                ToolBoxPanel.Visibility = Visibility.Hidden;
                _cyclicIndex.Reset();
            }
            else {
                _outputTimetables.SetState(new TimetableList(result));
                ToolBoxPanel.Visibility = Visibility.Visible;
                _cyclicIndex.MaxValue = result.Count - 1;
                _cyclicIndex.CurrentValue = 0;
                AutoCloseNotificationBar.Show(result.Count + " possible timetables found.");
            }
            TimetableViewer.Initialize(_cyclicIndex);
            CyclicIndexView.DataContext = new CyclicIndexVM(_cyclicIndex);
        }


        private List<List<Slot>> _raw;
        private void SetTimeConstraintButton_OnClick(object sender , RoutedEventArgs e) {
            if (_windowStateSummary == null)
                _windowStateSummary = new Window_StateSummary(_inputSlots.GetSlotsOf(SelectSubjectPanel.UIDofSelectedSlots).ToList() , _raw);
            _windowStateSummary.ShowDialog();
            if (_windowStateSummary.UserClickedDone) {
                _predicates = _windowStateSummary.Predicates;
                UpdateGUI(RunPermutation(_inputSlots.GetSlotsOf(SelectSubjectPanel.UIDofSelectedSlots)));
            }
        }


        private void TimetableViewer_OnViewChanged(object sender , EventArgs e) {
            _currentViewedTimetable.SetState(TimetableViewer.GetCurrentTimetable());
            if (!TimetableViewer.JustBuilded()) ;
            //SelectSubjectPanel.Collapse();
        }

        private void FavoriteButton_OnChecked(object sender , RoutedEventArgs e) {
            TimetableViewer.GetCurrentTimetable().IsLiked = true;
        }

        private void FavoriteButton_OnUnchecked(object sender , RoutedEventArgs e) {
            TimetableViewer.GetCurrentTimetable().IsLiked = false;
        }


        private void SelectSubjectPanel_OnSlotSelectionChanged(object sender , EventArgs e) {
            _predicates.Clear();
            _inputSlots.SelectedSubjectNames = SelectSubjectPanel.GetNamesOfCheckedSubject().ToList();
            var selectedSlots = _inputSlots.GetSlotsOf(SelectSubjectPanel.UIDofSelectedSlots);
            SetTimeConstraintButton.Visibility = selectedSlots.Length == 0 ? Visibility.Hidden : Visibility.Visible;
            List<List<Slot>> result = RunPermutation(selectedSlots);
            _windowStateSummary = new Window_StateSummary(selectedSlots.ToList() , result);
            UpdateGUI(result);
        }

        private List<List<Slot>> RunPermutation(Slot[] input) {
            var filteredSlot = Filterer.Filter(input , _predicates);
            var bg = CustomBackgroundWorker<Slot[] , List<List<Slot>>>.
                RunAndShowLoadingScreen(_permutator , filteredSlot , "Finding possible combination . . . ");
            return bg.GetResult();
        }

        private void ShowSummaryButton_OnClick(object sender , RoutedEventArgs e) {
            new SummaryWindow(_outputTimetables.GetCurrentState() , _cyclicIndex).ShowWindow();
        }

        private void SaveToGoogleCalendarButton_OnClick(object sender , RoutedEventArgs e) {
            DrawerHost.IsBottomDrawerOpen = false;
            NavigationService.Navigate(new Page_AddToGoogleCalendar(TimetableViewer.GetCurrentTimetable() ,
                Global.TimetableStartDate));
        }

        private void ViewSelector_OnSelectionChanged(object sender , SelectionChangedEventArgs e) {
            if (ViewSelector.SelectedIndex == 0) {
                var allTimetables = _outputTimetables.GetPreviousState();
                _outputTimetables?.SetState(allTimetables);
                var ci = new CyclicIndex(allTimetables.Count - 1);
                TimetableViewer?.Initialize(ci);
                if (CyclicIndexView != null)
                    CyclicIndexView.DataContext = new CyclicIndexVM(ci);
                // Global.Snackbar.MessageQueue.Enqueue("Showing ALL timetables");
            }
            else {
                var likedTimetable = _outputTimetables.GetLikedTimetableOnly();
                _outputTimetables?.SetState(likedTimetable);
                var ci = new CyclicIndex(likedTimetable.Count - 1);
                TimetableViewer?.Initialize(ci);
                CyclicIndexView.DataContext = new CyclicIndexVM(ci);
                // Global.Snackbar.MessageQueue.Enqueue("Showing FAVORITE timetables");
            }
        }

        private void ShowAllTimetable_Checked(object sender , RoutedEventArgs e) {
            var allTimetables = _outputTimetables.GetPreviousState();
            _outputTimetables?.SetState(allTimetables);
            var ci = new CyclicIndex(allTimetables.Count - 1);
            TimetableViewer?.Initialize(ci);
            if (CyclicIndexView != null) CyclicIndexView.DataContext = new CyclicIndexVM(ci);
            //Global.Snackbar.MessageQueue.Enqueue("Showing ALL timetables");
        }

        private void ShowFavoriteTimetable_Checked(object sender , RoutedEventArgs e) {
            var likedTimetable = _outputTimetables.GetLikedTimetableOnly();
            _outputTimetables?.SetState(likedTimetable);
            var ci = new CyclicIndex(likedTimetable.Count - 1);
            TimetableViewer?.Initialize(ci);
            CyclicIndexView.DataContext = new CyclicIndexVM(ci);
            //Global.Snackbar.MessageQueue.Enqueue("Showing FAVORITE timetables");
        }

        private bool _leftDrawerIsOpened = false;
        public void ExecuteLoadedFunction() {
            if (!_leftDrawerIsOpened) {
                DrawerHost.IsLeftDrawerOpen = true;
                _leftDrawerIsOpened = true;
                SelectSubjectPanel.Focus();
                SelectSubjectPanel.FocusSearchBox();
            }
        }

        private void SaveAsPicture_OnClick(object sender , RoutedEventArgs e) {
            DrawerHost.IsBottomDrawerOpen = false;
            var p = new Window()
            {
                SizeToContent = SizeToContent.WidthAndHeight,
                Content = new Page_SaveTimetableAsImage(TimetableViewer.GetCurrentTimetable())
            };
            p.Show();
            p.Close();
            return;
            this.NavigationService.Navigate(
            new Page_SaveTimetableAsImage(TimetableViewer.GetCurrentTimetable())
            );

        }
    
        private void SaveAsNotepadFile_OnClick(object sender , RoutedEventArgs e) {
            DrawerHost.IsBottomDrawerOpen = false;
            var slots = TimetableViewer.GetCurrentTimetable().ToList();
            var subjects = SubjectSummaryModel.GroupIntoSubjects(slots);
            var p = new SaveFileDialog() {
                Filter = "Notepad file (*.txt)|*.txt" ,
                FileName = "MyTimetableSummary"
            };
            if (p.ShowDialog() == false) return;
            string result = "";
            foreach (var s in subjects) {
                result += s.ToString() + "\r\n\r\n";
            }
            try {
                File.WriteAllText(p.FileName , result);
                Global.Snackbar.MessageQueue.Enqueue("File saved at " + p.FileName , "OPEN" ,
                    () => { Process.Start(p.FileName); });
            }
            catch (Exception ex) {
                Global.Snackbar.MessageQueue.Enqueue("Failed to save file." , "SHOW DETAILS" , 
                    () => {MessageBox.Show(ex.Message);});
            }
        }
    }
}
