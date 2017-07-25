using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Time_Table_Arranging_Program.Pages.GettingStarted;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program.Pages.Page_GettingStarted {
    /// <summary>
    /// Interaction logic for Page_GettingStarted.xaml
    /// </summary>
    public partial class Page_GettingStarted : Page {
        public Page_GettingStarted() {
            InitializeComponent();
            ScrollViewer.Content = new DoYouWantToWatchTTAPTutorial();
        }
             

        private void SkipButton_OnClick(object sender, RoutedEventArgs e) {
            NavigationService.Navigate(new Page_Intro());
        }

        private void NextButton_OnClick(object sender, RoutedEventArgs e) {
            this.ScrollViewer.Content = new IfYouAreUsingComputerLabInUtar();
        }
    }
}
