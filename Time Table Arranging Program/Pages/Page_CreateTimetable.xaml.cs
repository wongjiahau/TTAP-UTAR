using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.MVVM_Framework.Models;
using Time_Table_Arranging_Program.MVVM_Framework.ViewModels;
using Time_Table_Arranging_Program.MVVM_Framework.Views;
using Time_Table_Arranging_Program.User_Control;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program.Pages {
    /// <summary>
    ///     Interaction logic for Page_SelectSubject.xaml
    /// </summary>
    public partial class Page_CreateTimetable : Page, IDirtyObserver<IOutputTimetableModel> {
        private static Page_CreateTimetable _singletonInstance;
        private readonly MutableObservable<ITimetable> _currentViewedTimetable = new ObservableTimetable(Timetable.Empty);
        //private readonly CyclicIndex _cyclicIndex;

        private readonly ObservableTimetableList _outputTimetables =
            new ObservableTimetableList(TimetableList.NoSlotsIsChosen);

        private readonly SlotList _inputSlots;

        private List<Predicate<Slot>> _predicates = new List<Predicate<Slot>>();

        private MutableObservable<IOutputTimetableModel> _timetableList;
        private int _uidOfLastSlot = -1;

        public Page_CreateTimetable(SlotList inputSlots) {
            _inputSlots = inputSlots;
            InitializeComponent();
            FavouriteButton.SetObservedThings(_outputTimetables);
            FavouriteButton.SetObservedThings(_currentViewedTimetable);
            TimetableViewer.SetObservedThings(_outputTimetables);            
            TimetableViewer.Initialize(new CyclicIndex());
            InitializeExtraComponents();
        }


        public void SetObservedThings(MutableObservable<IOutputTimetableModel> x) {
            x.RegisterObserver(this);
            _timetableList = x;
        }

        public void Update() {
            _timetableList.GetCurrentState();
        }

        public static Page_CreateTimetable GetSingletonInstance(SlotList inputSlots) {
            if (_singletonInstance == null) {
                _singletonInstance = new Page_CreateTimetable(inputSlots);
            }
            return _singletonInstance;
        }

        private void InitializeExtraComponents() {
            SelectSubjectPanel.CreateCheckBoxes(_inputSlots);
            //            SelectSubjectPanel.CreateCheckBoxes(GetAvailableSubjects(Global.inputSlots) , Global.selectedSubject);
            FavouriteButton.CheckedMessage = "Added this timetable to favorites ";
            FavouriteButton.UncheckedMessage = "Removed this timetable from favorites";
        }

        private void UpdateGUI(Slot[] input) {
            var filteredSlot = Filterer.Filter(input, _predicates);
            var bg = CustomBackgroundWorker<Slot[], List<List<Slot>>>.
                RunAndShowLoadingScreen(PermutateV4.Run_v2, filteredSlot, "Finding possible combination . . . ");
            List<List<Slot>> result = bg.GetResult();
            var cyclicIndex = new CyclicIndex();            
            if (result == null || result.Count == 0) {
                if (_inputSlots.NoSlotIsChosen()) {
                    _outputTimetables.SetState(TimetableList.NoSlotsIsChosen);
                }
                else {
                    _outputTimetables.SetState(TimetableList.NoPossibleCombination);
                }
                ToolBoxCard.IsEnabled = false;
                cyclicIndex.Reset();
            }
            else {
                _outputTimetables.SetState(new TimetableList(result));
                ToolBoxCard.IsEnabled = true;
                cyclicIndex.MaxValue = result.Count - 1;
                cyclicIndex.CurrentValue = 0;
            }
            TimetableViewer.Initialize(cyclicIndex);
        }


        private void Page_SelectSubject_OnLoaded(object sender, RoutedEventArgs e) {
            bool inputSlotsHasChanged = _inputSlots.Last().UID != _uidOfLastSlot;
            if (inputSlotsHasChanged) {
                SelectSubjectPanel.Clear();
                if (_inputSlots.Count == 0) {
                    UpdateGUI(_inputSlots.GetSlotsOf(SelectSubjectPanel.UIDofSelectedSlots));
                }
                else
                    InitializeExtraComponents();
            }

            _uidOfLastSlot = _inputSlots.Last().UID;
            while (NavigationService?.CanGoBack == true) {
                NavigationService?.RemoveBackEntry();
            }
        }

        private void AddRuleButton_OnClick(object sender, RoutedEventArgs e) {
            _predicates = AddRuleWindow_v2.ShowWindow();
            UpdateGUI(_inputSlots.GetSlotsOf(SelectSubjectPanel.UIDofSelectedSlots));
        }


        private void TimetableViewer_OnViewChanged(object sender, EventArgs e) {
            _currentViewedTimetable.SetState(TimetableViewer.GetCurrentTimetable());
            if (!TimetableViewer.JustBuilded())
                SelectSubjectPanel.Collapsed();
        }

        private void FavoriteButton_OnChecked(object sender, RoutedEventArgs e) {
            TimetableViewer.GetCurrentTimetable().IsLiked = true;
            SelectSubjectPanel.Collapsed();
        }

        private void FavoriteButton_OnUnchecked(object sender, RoutedEventArgs e) {
            TimetableViewer.GetCurrentTimetable().IsLiked = false;
        }


        private void SelectSubjectPanel_OnSlotSelectionChanged(object sender, EventArgs e) {
            _inputSlots.SelectedSubjectNames = SelectSubjectPanel.GetNamesOfCheckedSubject().ToList();
            UpdateGUI(_inputSlots.GetSlotsOf(SelectSubjectPanel.UIDofSelectedSlots));
        }

        private void ShowSummaryButton_OnClick(object sender, RoutedEventArgs e) {
            SummaryWindow.GetSingletonInstance(_outputTimetables.GetCurrentState()).ShowWindow();
        }

        private void AddToGoogleCalendarButton_OnClick(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(new Page_AddToGoogleCalendar(TimetableViewer.GetCurrentTimetable(),
                Global.TimetableStartDate));
        }

        private void ViewSelector_OnSelectionChanged(object sender, SelectionChangedEventArgs e) {
            if (ViewSelector.SelectedIndex == 0) {
                var allTimetables = _outputTimetables.GetPreviousState();
                _outputTimetables?.SetState(allTimetables);
                TimetableViewer?.Initialize(new CyclicIndex(allTimetables.Count - 1));
                Global.Snackbar.MessageQueue.Enqueue("Showing ALL timetables");
            }
            else {
                var likedTimetable = _outputTimetables.GetLikedTimetableOnly();
                _outputTimetables?.SetState(likedTimetable);
                TimetableViewer?.Initialize(new CyclicIndex(likedTimetable.Count - 1));
                Global.Snackbar.MessageQueue.Enqueue("Showing FAVORITE timetables");
            }
        }
    }
}