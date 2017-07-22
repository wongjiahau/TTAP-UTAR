using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Time_Table_Arranging_Program.Windows_Control {
    /// <summary>
    /// Interaction logic for Windows_Settings.xaml
    /// </summary>
    public partial class Windows_Settings : Window {
        private Windows_Settings() {
            InitializeComponent();
            InitializeSettings();
            ItemsControl_Settings.ItemsSource = Settings;
        }

        private void InitializeSettings() {
            Settings = new List<Setting>
            {
                Global.Settings.SearchByConsideringWeekNumber,
                Global.Settings.GeneralizeSlot
            };
        }

        private static Windows_Settings _singleton;

        public static Windows_Settings GetInstance() {
            if (_singleton == null) _singleton = new Windows_Settings();
            _singleton.ApplyClicked = false;
            return _singleton;
        }
        public List<Setting> Settings { private set; get; }
        public bool ApplyClicked { get; private set; } = false;

        private void ApplyButton_OnClick(object sender , RoutedEventArgs e) {
            ApplyClicked = true;
            this.Hide();
        }

        private void CancelButton_OnClick(object sender , RoutedEventArgs e) {
            ApplyClicked = false;
            this.Hide();
        }

        private void Windows_Settings_OnClosing(object sender , CancelEventArgs e) {
            e.Cancel = true;
            this.Hide();
        }


    }

    public class Setting {
        public enum SettingDescription {
            SearchByConsideringWeekNumber,
            GeneralizedSlot
        }

        public Setting() {

        }
        public Setting(SettingDescription desc , string label, string explanation, bool isChecked) {
            Description = desc;
            Label = label;
            Explanation = explanation;
            IsChecked = isChecked;
        }
        public bool IsChecked { get; set; }
        public string Label { get; private set; }
        public string Explanation { get; private set; }
        public SettingDescription Description { get; private set; }
    }


}
