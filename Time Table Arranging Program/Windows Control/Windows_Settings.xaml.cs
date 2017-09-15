using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using Time_Table_Arranging_Program.Class.AbstractClass;
using Time_Table_Arranging_Program.MVVM_Framework;

namespace Time_Table_Arranging_Program.Windows_Control {
    /// <summary>
    /// Interaction logic for Windows_Settings.xaml
    /// </summary>
    public partial class Windows_Settings : Window {
        private static Windows_Settings _singleton;

        private Windows_Settings() {
            InitializeComponent();
            InitializeSettings();
            ItemsControl_Settings.ItemsSource = Settings;
        }

        public List<Setting> Settings { private set; get; }
        public bool ApplyClicked { get; private set; } = false;

        private void InitializeSettings() {
            Settings = new List<Setting>
            {
                Global.Settings.SearchByConsideringWeekNumber,
                Global.Settings.GeneralizeSlot
            };
        }

        public static Windows_Settings GetInstance() {
            if (_singleton == null) _singleton = new Windows_Settings();
            _singleton.ApplyClicked = false;
            return _singleton;
        }

        private void ApplyButton_OnClick(object sender, RoutedEventArgs e) {
            ApplyClicked = true;
            Hide();
        }

        private void CancelButton_OnClick(object sender, RoutedEventArgs e) {
            ApplyClicked = false;
            Hide();
        }

        private void Windows_Settings_OnClosing(object sender, CancelEventArgs e) {
            e.Cancel = true;
            Hide();
        }

        private void ToggleButton_OnChecked(object sender, RoutedEventArgs e) {
            if (Global.Settings.SearchByConsideringWeekNumber.IsChecked) {
                Global.Settings.GeneralizeSlot.IsChecked = false;
            }
        }
    }

    public class Setting : ObservableObject {
        public enum SettingDescription {
            SearchByConsideringWeekNumber,
            GeneralizedSlot
        }

        private bool _isChecked;

        public Setting() { }

        public Setting(SettingDescription desc, string label, string explanation, bool isChecked) {
            Description = desc;
            Label = label;
            Explanation = explanation;
            IsChecked = isChecked;
        }

        public bool IsChecked {
            get => _isChecked;
            set => SetProperty(ref _isChecked, value);
        }

        public string Label { get; private set; }
        public string Explanation { get; private set; }
        public SettingDescription Description { get; private set; }
    }
}