using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Helper;
using Time_Table_Arranging_Program.Class.TokenParser;
using Time_Table_Arranging_Program.Windows_Control;
using KeyEventArgs = System.Windows.Input.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using WebBrowser = System.Windows.Controls.WebBrowser;

namespace Time_Table_Arranging_Program.Pages {
    /// <summary>
    /// Interaction logic for Page_First.xaml
    /// </summary>
    public partial class Page_Login : Page {
        private const string LoginPageUrl = "https://unitreg.utar.edu.my/portal/courseRegStu/login.jsp";

        private const string LoginFailedUrl =
            "https://unitreg.utar.edu.my/portal/courseRegStu/login.jsp?message=invalidSecurity";

        private const string CourseTimetablePreviewUrl =
            "https://unitreg.utar.edu.my/portal/courseRegStu/schedule/masterSchedule.jsp";

        private string _studentIdInput;
        private string _passwordInput;
        private string _captchaInput;
        private const string TestServerUrl = "http://localhost/ttap_testdata/";

        private int _currentPage = 1;
        private const int NavigationCountUpperLimit = 3;
        private int _navigationCount = 0;

        public Page_Login() {
            InitializeComponent();
        }


        private bool _loadDataFromTestServer;
        public Page_Login(bool loadDataFromTestServer) : this() {
            _loadDataFromTestServer = loadDataFromTestServer;
        }

        private void Page_Login_OnLoaded(object sender , RoutedEventArgs e) {
            GotItButton_OnClick(null , null);
            if (_loadDataFromTestServer) {
                Browser.Navigate(TestServerUrl);
                return;
            }
            Browser.Navigate(LoginPageUrl);
        }

        private bool _browsingToCourseTimetablePreview = false;

        private void Browser_OnLoadCompleted(object sender , NavigationEventArgs e) {
            KapchaBrowser.Navigate("https://unitreg.utar.edu.my/portal/Kaptcha.jpg");
            ResetButton.IsEnabled = true;
            string currentUrl = Browser.Source.ToString();
            if (currentUrl.Contains(TestServerUrl)) goto here;
            if (currentUrl == LoginFailedUrl ||
                (currentUrl == LoginPageUrl && PasswordBox.Password.Length > 0)
                ) {
                //display error and ert a newline after current cursor without entering INSERT efresh page when error
                MessageBox.Show("Please make sure information are valid!");
                this.NavigationService.Refresh();
            }
            if (currentUrl == LoginPageUrl || currentUrl == LoginFailedUrl) {
                _navigationCount = 0;
                return;
            }
            if (currentUrl.Contains(CourseTimetablePreviewUrl) == false) {
                _currentPage = 1;
                if (_browsingToCourseTimetablePreview == false) {
                    if (_navigationCount < NavigationCountUpperLimit) {
                        Browser.Navigate(CourseTimetablePreviewUrl);
                        _navigationCount++;
                    }
                    else {
                        Browser.Navigate(LoginPageUrl);
                        Global.Snackbar.MessageQueue.Enqueue("No record found, please try again.");
                    }
                }
                return;
            }

            here:
            string html = GetHtml(Browser);
            if (Global.Toggles.SaveLoadedHtmlToggle.IsToggledOn) SaveToFile(html);
            ProgressBar.Visibility = Visibility.Visible;
            var bg = CustomBackgroundWorker<string , List<Slot>>.RunAndShowLoadingScreen(
               new HtmlSlotParser().Parse , html , "Loading slots . . .");
            //    TryGetStartDateAndEndDate(plainText);
            Global.InputSlotList.AddRange(bg.GetResult());
            if (CanGoToPage(_currentPage + 1)) {
                Browser.InvokeScript("changePage" , _currentPage + 1);
                _currentPage++;
            }
            else {
                if (Global.InputSlotList.Count == 0) {
                    DialogBox.Show("No data available." , "" , "OK");
                    if (DialogBox.Result == DialogBox.ResultEnum.RightButtonClicked) {
                        LoadTestDataButton_OnClick(null , null);
                    }
                    return;
                }

                NavigationService.Navigate(
                    Page_CreateTimetable.GetInstance(Global.Settings.SearchByConsideringWeekNumber ,
                        Global.Settings.GeneralizeSlot));
            }
        }

        private void SaveToFile(string html) {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Text file (*.html)|*.html";
            saveFileDialog.FileName = "SampleHtmlData";
            if (saveFileDialog.ShowDialog() == true) {
                File.WriteAllText(saveFileDialog.FileName , html);
                Global.Snackbar.MessageQueue.Enqueue("File saved.");
            }
        }

        private string GetHtml(WebBrowser b) {
            dynamic doc = b.Document;
            var htmlText = doc.documentElement.InnerHtml;
            return htmlText;
        }

        private void TryGetStartDateAndEndDate(string input) {
            try {
                var parser = new StartDateEndDateFinder(input);
                Global.TimetableStartDate = parser.GetStartDate();
                Global.TimetableEndDate = parser.GetEndDate();
            }
            catch { }
        }

        private bool CanGoToPage(int pageNumber) {
            dynamic doc = Browser.Document;
            string htmlText = doc.documentElement.InnerHtml;
            return htmlText.Contains($"javascript:changePage(\'{pageNumber}\')");
        }

        private bool CheckForInternetConnection() {
            if (Helper.CanConnectToInternet()) {
                DrawerHost.IsBottomDrawerOpen = false;
                return true;
            }
            else {
                DrawerHost.IsBottomDrawerOpen = true;
                return false;
            }
        }
        #region EventHandlers
        private void ResetButton_OnClick(object sender , RoutedEventArgs e) {
            UserNameBox.Text = "";
            PasswordBox.Password = "";
            CaptchaBox.Text = "";
            Browser.Navigate(LoginPageUrl);
        }

        private void GotItButton_OnClick(object sender , RoutedEventArgs e) {
            DialogHost.IsOpen = false;
            Browser.Navigate(LoginPageUrl);
            Browser.Visibility = Visibility.Visible;
        }

        private void LoadTestDataButton_OnClick(object sender , RoutedEventArgs e) {
            Global.InputSlotList.AddRange(TestData.TestSlots);
            NavigationService.Navigate(
                Page_CreateTimetable.GetInstance(Global.Settings.SearchByConsideringWeekNumber ,
                    Global.Settings.GeneralizeSlot));

        }

        private void KapchaBrowser_OnLoadCompleted(object sender , NavigationEventArgs e) {
            KapchaBrowser.InvokeScript("execScript" , "document.body.style.overflow ='hidden'" , "JavaScript");
        }

        private void LoginButton_OnClick(object sender , RoutedEventArgs e) {
            if (!CheckForInternetConnection()) return;
            try {
                _studentIdInput = UserNameBox.Text;
                _passwordInput = PasswordBox.Password;
                _captchaInput = CaptchaBox.Text;
                Browser.InvokeScript("execScript",
                    "document.getElementsByName('reqFregkey')[0].value='" + _studentIdInput + "'", "JavaScript");
                Browser.InvokeScript("execScript",
                    "document.getElementsByName('reqPassword')[0].value='" + _passwordInput + "'", "JavaScript");
                Browser.InvokeScript("execScript",
                    "document.getElementsByName('kaptchafield')[0].value='" + _captchaInput + "'", "JavaScript");
                Browser.InvokeScript("execScript",
                    "document.getElementsByName('_submit')[0].click()", "JavaScript");
            }
            catch (Exception ex) {
                //If the flow went here, it may be due to the following steps : 
                // 1. User login into account, and successfully loaded slots
                // 2. User clicked back
                // 3. User login again

            }
        }

        private void CaptchaBox_OnKeyUp(object sender , KeyEventArgs e) {
            if (e.Key == Key.Enter)
                LoginButton_OnClick(null , null);
        }

        private void RetryButton_OnClicked(object sender , RoutedEventArgs e) {
            CheckForInternetConnection();
        }

        #endregion
    }
}