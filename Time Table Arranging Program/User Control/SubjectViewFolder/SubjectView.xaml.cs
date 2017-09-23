using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using MaterialDesignThemes.Wpf;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Model;
using Time_Table_Arranging_Program.UserInterface;

namespace Time_Table_Arranging_Program.User_Control.SubjectViewFolder {
    public partial class SubjectView : UserControl, INeedDataContext<SubjectModel> {
        private double _listviewOriginalHeight;
        private SubjectModel _subjectModel;

        public SubjectView() {
            InitializeComponent();
            UIDofDeselectedSlots = new HashSet<int>();
            UIDofSelectedSlots = new HashSet<int>();
            InitializeDraggablePopup();
        }

        public void SetDataContext(SubjectModel subjectModel) {
            _subjectModel = subjectModel;
            _subjectModel.Focused += _subjectModel_Focused;
            DataContext = subjectModel;
            foreach (var item in subjectModel.Slots) {
                item.IsSelected = true;
                UIDofSelectedSlots.Add(item.UID);
            }
        }

        private void _subjectModel_Focused(object sender , EventArgs e) {
            Focused?.Invoke(this , null);
        }

        public string NameOfClashingCounterpart { get; set; }
        public event RoutedEventHandler ListViewCheckBox_Checked;
        public event EventHandler Focused;

        public string SubjectName {
            get => SubjectNameHighlightTextBlock.Text;
            set => SubjectNameHighlightTextBlock.Text = value;
        }

        public HashSet<int> UIDofDeselectedSlots { get; set; }
        public HashSet<int> UIDofSelectedSlots { get; set; }

        #region EventHandlers

        private void SubjectView_OnMouseEnter(object sender , MouseEventArgs e) {
            _subjectModel.FocusMe();
        }

        private void SubjectView_OnMouseLeave(object sender , MouseEventArgs e) {
            _subjectModel.IsFocused = false;
        }

        private void Border_OnMouseDown(object sender , MouseButtonEventArgs e) {
            if (e.ChangedButton == MouseButton.Left)
                Checkbox.IsChecked = !Checkbox.IsChecked;
        }

        private void ViewSlotsContextMenuItem_OnClick(object sender , RoutedEventArgs e) {
            ListViewPopup.IsOpen = true;
        }

        #endregion

        #region ListDownMenu

        private void InitializeDraggablePopup() {
            var thumb = new Thumb {
                Width = 0 ,
                Height = 0 ,
                Cursor = Cursors.SizeAll
            };
            PopupDp.Children.Add(thumb);
            MouseDown += (sender , e) => {
                if (ListViewPopup.IsOpen) {
                    thumb.RaiseEvent(e);
                }
            };

            thumb.DragDelta += (sender , e) => {
                ListViewPopup.HorizontalOffset += e.HorizontalChange;
                ListViewPopup.VerticalOffset += e.VerticalChange;
            };
        }

        private void ChooseSlotButton_Click(object sender , RoutedEventArgs e) {
            ListViewPopup.IsOpen = !ListViewPopup.IsOpen;
        }

        private void CloseButton_OnClick(object sender , RoutedEventArgs e) {
            ListViewPopup.IsOpen = false;
        }

        private void ListViewPopup_OnClosed(object sender , EventArgs e) {
            ListViewPopup.VerticalOffset = 0;
            ListViewPopup.HorizontalOffset = 0;
        }

        private void HideButton_Click(object sender , RoutedEventArgs e) {
            _listviewOriginalHeight = ListView.ActualHeight;
            var da1 = CustomAnimation.GetLeavingScreenAnimation(_listviewOriginalHeight , 0);
            da1.Completed += (o , args) => { ListView.Visibility = Visibility.Collapsed; };

            if (ListView.Visibility == Visibility.Visible) {
                InstructionLabel.Visibility = Visibility.Collapsed;
                ListView.BeginAnimation(HeightProperty , da1);
                var maximizeIcon = new PackIcon();
                maximizeIcon.Kind = PackIconKind.WindowMaximize;
                HidePopupButton.Content = maximizeIcon;
            }
            else {
                ListView.Visibility = Visibility.Visible;
                var minimizeIcon = new PackIcon();
                minimizeIcon.Kind = PackIconKind.WindowMinimize;
                HidePopupButton.Content = minimizeIcon;
            }
        }

        #endregion
    }
}