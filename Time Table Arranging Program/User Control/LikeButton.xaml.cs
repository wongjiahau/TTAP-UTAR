using System;
using System.Windows;
using System.Windows.Controls;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.User_Control {
    /// <summary>
    /// Interaction logic for LikeButton.xaml
    /// </summary>
    public partial class LikeButton : UserControl, IPureObserver<ITimetableList>, IPureObserver<ITimetable> {
        private readonly string CheckedTooltip = "Remove this timetable from favorites";
        private readonly string UncheckedTooltip = "Add this timetable to favorites";
        private IImmutableObservable<ITimetable> _observableTimetable;

        private IImmutableObservable<ITimetableList> _observableTimetableList;

        public LikeButton() {
            InitializeComponent();
            DataContext = this;
        }

        public string UncheckedMessage { private get; set; }
        public string CheckedMessage { private get; set; }

        public void SetObservedThings(IImmutableObservable<ITimetable> x) {
            _observableTimetable = x;
            _observableTimetable.RegisterObserver(this);
        }

        public void Update() {
            if (_observableTimetableList.GetCurrentState() == TimetableList.NoPossibleCombination ||
                _observableTimetableList.GetCurrentState() == TimetableList.NoSlotsIsChosen) {
                IsEnabled = false;
            }
            else {
                IsEnabled = true;
            }

            ToggleButton.IsChecked = _observableTimetable.GetCurrentState().IsLiked;
            ToggleButton.ToolTip = ToggleButton.IsChecked.Value ? CheckedTooltip : UncheckedTooltip;
        }

        public void SetObservedThings(IImmutableObservable<ITimetableList> x) {
            _observableTimetableList = x;
            _observableTimetableList.RegisterObserver(this);
        }


        public event RoutedEventHandler Checked;
        public event RoutedEventHandler Unchecked;

        private void ToggleButton_OnClick(object sender, RoutedEventArgs e) {
            if (ToggleButton.IsChecked.Value) {
                AutoCloseNotificationBar.Show(CheckedMessage);
                Checked?.Invoke(this, null);
                ToggleButton.ToolTip = CheckedTooltip;
            }
            else {
                AutoCloseNotificationBar.Show(UncheckedMessage);
                Unchecked?.Invoke(this, null);
                ToggleButton.ToolTip = UncheckedTooltip;
            }
        }

        #region TextProperty

        public string Text {
            get { return (string) GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(LikeButton),
                new PropertyMetadata("this is a like button"));

        #endregion
    }
}