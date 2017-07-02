using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.UserInterface;

namespace Time_Table_Arranging_Program {
    /// <summary>
    ///     Interaction logic for CheckBoxWithListDownMenu.xaml
    /// </summary>
    public interface ICheckBoxWithListDownMenu {
        bool IsChecked { get; set; }
        List<Slot> SlotList { get; set; }
        FontWeight FontWeight { set; }
        string SubjectName { get; set; }
        string SubjectCode { get; set; }
        string HighlightText { get; set; }


        HashSet<int> UIDofDeselectedSlots { get; set; }
        HashSet<int> UIDofSelectedSlots { get; set; }
        event RoutedEventHandler Checked;
        event RoutedEventHandler ListViewCheckBox_Checked;
    }

    public partial class CheckBoxWithListDownMenu : UserControl, ICheckBoxWithListDownMenu {
        private double _listviewOriginalHeight;

        private Brush _originalColor;

        public CheckBoxWithListDownMenu() {
            InitializeComponent();
            UIDofDeselectedSlots = new HashSet<int>();
            UIDofSelectedSlots = new HashSet<int>();
            InitializeDraggablePopup();
        }

        public event RoutedEventHandler Checked;
        public event RoutedEventHandler ListViewCheckBox_Checked;

        public bool IsChecked {
            get { return Checkbox.IsChecked == true; }
            set
            {
                Checkbox.IsChecked = value;
                if (value) {
                    Border.Background = ColorDictionary.CheckedColor;
                }
                else {
                    Border.Background = null;
                    Border.Background = ColorDictionary.UncheckedColor;
                }
            }
        }

        public List<Slot> SlotList {
            get { return (List<Slot>) ListView.ItemsSource; }
            set
            {
                SubjectNameLabel.Content = "\u2629" + value[0].SubjectName;
                SubjectCode = value[0].Code;
                ListView.ItemsSource = value;
                foreach (var item in ListView.ItemsSource) {
                    ((Slot) item).IsSelected = true;
                    UIDofSelectedSlots.Add(((Slot) item).UID);
                }
            }
        }

        public new FontWeight FontWeight {
            set { Checkbox.FontWeight = value; }
        }

        public string SubjectName {
            get { return SubjectNameHighlightTextBlock.Text; }
            set { SubjectNameHighlightTextBlock.Text = value; }
        }

        public string SubjectCode {
            get { return SubjectCodeHighlightTextBlock.Text; }
            set { SubjectCodeHighlightTextBlock.Text = value; }
        }


        public HashSet<int> UIDofDeselectedSlots { get; set; }
        public HashSet<int> UIDofSelectedSlots { get; set; }

        public string HighlightText {
            get { return SubjectNameHighlightTextBlock.HighlightedText; }

            set
            {
                SubjectNameHighlightTextBlock.HighlightedText = value;
                SubjectCodeHighlightTextBlock.HighlightedText = value;
            }
        }

        private void InitializeDraggablePopup() {
            var thumb = new Thumb
            {
                Width = 0,
                Height = 0,
                Cursor = Cursors.SizeAll
            };
            PopupDp.Children.Add(thumb);
            MouseDown += (sender, e) =>
            {
                if (ListViewPopup.IsOpen) {
                    thumb.RaiseEvent(e);
                }
            };

            thumb.DragDelta += (sender, e) =>
            {
                ListViewPopup.HorizontalOffset += e.HorizontalChange;
                ListViewPopup.VerticalOffset += e.VerticalChange;
            };
        }

        private void Checkbox_Checked(object sender, RoutedEventArgs e) {
            Checked?.Invoke(this, e);
            if (Checkbox.IsChecked.Value) {
                ChooseSlotButton.Visibility = Visibility.Visible;
                //SubjectNameHighlightTextBlock.FontWeight = FontWeights.Bold;
                Border.Background = ColorDictionary.CheckedColor;
            }
            else {
                ChooseSlotButton.Visibility = Visibility.Hidden;
                //SubjectNameHighlightTextBlock.FontWeight = FontWeights.DemiBold;
                Border.Background = _originalColor; // ColorDictionary.UncheckedColor;
            }
        }

        private void ChooseSlotButton_Click(object sender, RoutedEventArgs e) {
            ListViewPopup.IsOpen = true;
        }

        private void CloseButton_OnClick(object sender, RoutedEventArgs e) {
            ListViewPopup.IsOpen = false;
        }

        private void HideButton_Click(object sender, RoutedEventArgs e) {
            _listviewOriginalHeight = ListView.ActualHeight;
            var da1 = CustomAnimation.GetLeavingScreenAnimation(_listviewOriginalHeight, 0);
            da1.Completed += (o, args) => { ListView.Visibility = Visibility.Collapsed; };

            if (ListView.Visibility == Visibility.Visible) {
                ToggleCheckButton.Visibility = Visibility.Collapsed;
                InstructionLabel.Visibility = Visibility.Collapsed;
                ListView.BeginAnimation(HeightProperty, da1);
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


        private void ListViewPopup_OnClosed(object sender, EventArgs e) {
            ListViewPopup.VerticalOffset = 0;
            ListViewPopup.HorizontalOffset = 0;
        }

        private void CheckBoxWithListDownMenu_OnMouseEnter(object sender, MouseEventArgs e) {
            if (_originalColor == null) _originalColor = Border.Background;
            Border.Background = ColorDictionary.MouseOverColor;
        }

        private void CheckBoxWithListDownMenu_OnMouseLeave(object sender, MouseEventArgs e) {
            Border.Background =
                Checkbox.IsChecked.Value
                    ? ColorDictionary.CheckedColor
                    : _originalColor;
            //: ColorDictionary.UncheckedColor;
        }


        private void Border_OnMouseDown(object sender, MouseButtonEventArgs e) {
            Checkbox.IsChecked = !Checkbox.IsChecked.Value;
        }

        private void ListViewItemCheckBox_Checked(object sender, RoutedEventArgs e) {
            var c = sender as CheckBox;
            var selectedSlot = (Slot) (c.Tag as ListViewItem).Content;
            selectedSlot.IsSelected = c.IsChecked.Value;
            CascadeEffectToSimilarSlot(selectedSlot);
        }

        private void ToggleCheckButton_OnClick(object sender, RoutedEventArgs e) {
            var temp = ListView.ItemsSource;
            ListView.ItemsSource = null;
            ListView.ItemsSource = temp;
            if (ToggleCheckButton.Content.ToString() == "Untick all slots") {
                ToggleCheckButton.Content = "Tick all slots";
                InstructionLabel.Content = ". . . Tick the slots that you want";
                foreach (var item in ListView.ItemsSource) {
                    ((Slot) item).IsSelected = false;
                    UIDofDeselectedSlots.Add(((Slot) item).UID);
                    UIDofSelectedSlots.Clear();
                }
            }
            else {
                ToggleCheckButton.Content = "Untick all slots";
                InstructionLabel.Content = ". . . Untick the slots that you don't want";
                foreach (var item in ListView.ItemsSource) {
                    ((Slot) item).IsSelected = true;
                    UIDofSelectedSlots.Add(((Slot) item).UID);
                    UIDofDeselectedSlots.Clear();
                }
            }
            ListViewCheckBox_Checked?.Invoke(this, null);
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e) {
            var item = sender as ListViewItem;

            if (item != null) {
                ((Slot) item.Content).IsSelected = !((Slot) item.Content).IsSelected;
            }
            CascadeEffectToSimilarSlot((Slot) item.Content);
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
            ListViewCheckBox_Checked?.Invoke(this, null);
        }
    }
}