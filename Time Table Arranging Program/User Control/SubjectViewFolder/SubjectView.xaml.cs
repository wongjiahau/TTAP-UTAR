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
    /// <summary>
    ///     Interaction logic for CheckBoxWithListDownMenu.xaml
    /// </summary>
    public interface ISubjectView {
        string SubjectName { get; set; }
        HashSet<int> UIDofDeselectedSlots { get; set; }
        HashSet<int> UIDofSelectedSlots { get; set; }
        string NameOfClashingCounterpart { get; set; }
        event RoutedEventHandler ListViewCheckBox_Checked;
    }

    public partial class SubjectView : UserControl, ISubjectView, INeedDataContext<SubjectModel> {
        private double _listviewOriginalHeight;
        private SubjectModel _subjectModel;
        public SubjectView() {
            InitializeComponent();
            UIDofDeselectedSlots = new HashSet<int>();
            UIDofSelectedSlots = new HashSet<int>();
            InitializeDraggablePopup();
        }
        public void SetDataContext(SubjectModel subjectModels) {
            _subjectModel = subjectModels;
            this.DataContext = subjectModels;
            foreach (var item in subjectModels.Slots) {
                item.IsSelected = true;
                UIDofSelectedSlots.Add(item.UID);
            }
        }

        public string NameOfClashingCounterpart { get; set; }
        public event RoutedEventHandler ListViewCheckBox_Checked;

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

        private void ListViewItemCheckBox_Checked(object sender , RoutedEventArgs e) {
            var c = sender as CheckBox;
            var selectedSlot = (Slot)(c.Tag as ListViewItem).Content;
            selectedSlot.IsSelected = c.IsChecked.Value;
            CascadeEffectToSimilarSlot(selectedSlot);
        }

        private void ToggleCheckButton_OnClick(object sender , RoutedEventArgs e) {
            var temp = ListView.ItemsSource;
            ListView.ItemsSource = null;
            ListView.ItemsSource = temp;
            if (ToggleCheckButton.Content.ToString() == "Untick all slots") {
                ToggleCheckButton.Content = "Tick all slots";
                InstructionLabel.Content = ". . . Tick the slots that you want";
                foreach (var item in ListView.ItemsSource) {
                    ((Slot)item).IsSelected = false;
                    UIDofDeselectedSlots.Add(((Slot)item).UID);
                    UIDofSelectedSlots.Clear();
                }
            }
            else {
                ToggleCheckButton.Content = "Untick all slots";
                InstructionLabel.Content = ". . . Untick the slots that you don't want";
                foreach (var item in ListView.ItemsSource) {
                    ((Slot)item).IsSelected = true;
                    UIDofSelectedSlots.Add(((Slot)item).UID);
                    UIDofDeselectedSlots.Clear();
                }
            }
            ListViewCheckBox_Checked?.Invoke(this , null);
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender , MouseButtonEventArgs e) {
            return;
            //This feature is disabld at the moment
            var item = sender as ListViewItem;

            if (item != null) {
                ((Slot)item.Content).IsSelected = !((Slot)item.Content).IsSelected;
            }
            CascadeEffectToSimilarSlot((Slot)item.Content);
        }

        private void CascadeEffectToSimilarSlot(Slot selectedSlot) {
            if (selectedSlot.IsSelected == false) {
                foreach (Slot s in ListView.Items) {
                    if (s.Code == selectedSlot.Code && s.Type == selectedSlot.Type && s.Number == selectedSlot.Number) {
                        s.IsSelected = false;
                        UIDofDeselectedSlots.Add(s.UID);
                        UIDofSelectedSlots.Remove(s.UID);
                    }
                }
            }
            else {
                foreach (Slot s in ListView.Items) {
                    if (s.Code == selectedSlot.Code && s.Type == selectedSlot.Type && s.Number == selectedSlot.Number) {
                        s.IsSelected = true;
                        UIDofDeselectedSlots.Remove(s.UID);
                        UIDofSelectedSlots.Add(s.UID);
                    }
                }
            }
            var temp = ListView.ItemsSource;
            ListView.ItemsSource = null;
            ListView.ItemsSource = temp;
            ListViewCheckBox_Checked?.Invoke(this , null);
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
                ToggleCheckButton.Visibility = Visibility.Collapsed;
                InstructionLabel.Visibility = Visibility.Collapsed;
                ListView.BeginAnimation(HeightProperty , da1);
                var maximizeIcon = new PackIcon();
                maximizeIcon.Kind = PackIconKind.WindowMaximize;
                HidePopupButton.Content = maximizeIcon;
            }
            else {
                ListView.Visibility = Visibility.Visible;
                ToggleCheckButton.Visibility = Visibility.Visible;
                InstructionLabel.Visibility = Visibility.Visible;

                var minimizeIcon = new PackIcon();
                minimizeIcon.Kind = PackIconKind.WindowMinimize;
                HidePopupButton.Content = minimizeIcon;
            }
        }

        #endregion


    }
}