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

        }

        public static Settings SearchByConsideringWeekNumber { private set; get; } = new Settings(Windows_Control.Settings.SettingDescription.SearchForCombinationByConsideringWeekNumber , false);
        private void InitializeSettings() {
            Settings = new List<Settings>();
            SearchByConsideringWeekNumber = (Settings)this.Resources["Setting1"];
            Settings.Add(SearchByConsideringWeekNumber);
        }

        private static Windows_Settings _singleton;

        public static Windows_Settings GetInstance() {
            if (_singleton == null) _singleton = new Windows_Settings();
            _singleton.ApplyClicked = false;
            return _singleton;
        }
        public List<Settings> Settings { private set; get; }
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

    public class Settings {
        public enum SettingDescription {
            SearchForCombinationByConsideringWeekNumber
        }

        public Settings() {

        }
        public Settings(SettingDescription desc , bool isChecked) {
            Description = desc;
            IsChecked = isChecked;
        }
        public bool IsChecked { get; set; }
        public SettingDescription Description { get; set; }
    }


}
