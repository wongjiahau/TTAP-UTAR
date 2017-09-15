using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Helper;
using Time_Table_Arranging_Program.Class.TokenParser;
using Time_Table_Arranging_Program.Pages.Login;
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
        private readonly UrlProvider _urlProvider;
        private string _studentIdInput;
        private string _passwordInput;
        private string _captchaInput;
        private int _currentPage = 1;
        private const int NavigationCountUpperLimit = 3;
        private int _navigationCount = 0;

        public Page_Login() {
            _urlProvider = new UrlProvider();
            InitializeComponent();
            bool gotInternet = CheckForInternetConnection();
            if (gotInternet) Loaded += Page_Login_Loaded;
        }

        private void Page_Login_Loaded(object sender , RoutedEventArgs e) {
            if (_loadDataFromTestServer) {
                Browser.Navigate(_urlProvider.TestServerUrl);
                return;
            }
            if (Global.InputSlotList.Count == 0) Browser.Navigate(_urlProvider.LoginPageUrl);
            else {
                DialogBox.Show("Login again?" , "WARNING : If you login again your previous data will be overwrited." ,
                    "CANCEL" , "LOGIN AGAIN");
                switch (DialogBox.Result) {
                    case DialogBox.ResultEnum.LeftButtonClicked:
                        NavigationService.GoForward();
                        break;
                    case DialogBox.ResultEnum.RightButtonClicked:
                        Global.InputSlotList.Clear();
                        //Need to reset two times to properly load the login page again
                        ResetButton_OnClick(null , null);
                        ResetButton_OnClick(null , null);
                        break;
                }
            }
        }

        private readonly bool _loadDataFromTestServer;

        public Page_Login(bool loadDataFromTestServer) : this() {
            _loadDataFromTestServer = loadDataFromTestServer;
        }

        private readonly bool _browsingToCourseTimetablePreview = false;
        private async void Browser_OnLoadCompleted(object sender , NavigationEventArgs e) {
            KapchaBrowser.Navigate(_urlProvider.KaptchaUrl);
            ResetButton.IsEnabled = true;
            string currentUrl = Browser.Source.ToString();
            if (currentUrl == _urlProvider.EndUrl) return;
            if (currentUrl.Contains(_urlProvider.TestServerUrl)) ExtractData();
            else if (_urlProvider.IsLoginFailed(currentUrl)) DisplayLoginFailedMessage();
            else if (_urlProvider.IsAtLoginPage(currentUrl)) AssertLoginPageIsLoadedProperly();
            else if (!currentUrl.Contains(_urlProvider.CourseTimetablePreviewUrl)) NavigateToCourseTimeTablePreview();
            else if (currentUrl.Contains(_urlProvider.CourseTimetablePreviewUrl)) ExtractData();
            #region NestedFunctions
            void AssertLoginPageIsLoadedProperly()
            {
                string html = GetHtml(Browser);
                if (!html.Contains("Course Registration System"))
                    Browser.Navigate(_urlProvider.LoginPageUrl);
            }
            void DisplayLoginFailedMessage()
            {
                Global.Snackbar.MessageQueue.Enqueue("Login failed. Please make sure you entered the correct information.");
                NavigationService.Refresh();
                _navigationCount = 0;
            }
            void NavigateToCourseTimeTablePreview()
            {
                _currentPage = 1;
                if (_browsingToCourseTimetablePreview) return;
                if (_navigationCount < NavigationCountUpperLimit) {
                    Browser.Navigate(_urlProvider.CourseTimetablePreviewUrl);
                    _navigationCount++;
                }
                else {
                    Browser.Navigate(_urlProvider.LoginPageUrl);
                    Global.Snackbar.MessageQueue.Enqueue($"No record found.");
                    ResetButton_OnClick(null , null);
                }
            }
            async void ExtractData()
            {
                string html = GetHtml(Browser);
                DisplayLoadingBar(true);
                await Task.Run(() => {
                    if (Global.Toggles.SaveLoadedHtmlToggle.IsToggledOn) SaveToFile(html);
                    Global.InputSlotList.AddRange(new HtmlSlotParser().Parse(html));
                    Global.LoadedHtml += html;
                    TryGetStartDateAndEndDate(html);
                });
                if (CanGoToPage(_currentPage + 1)) {
                    Browser.InvokeScript("changePage" , _currentPage + 1);
                    _currentPage++;
                }
                else {
                    DisplayLoadingBar(false);
                    if (Global.InputSlotList.Count == 0) {
                        DialogBox.Show("No data available." , "" , "OK");
                        ResetButton_OnClick(null , null);
                        return;
                    }
                    Browser.Navigate(_urlProvider.EndUrl);
                    NavigationService.Navigate(
                        Page_CreateTimetable.GetInstance(Global.Settings.SearchByConsideringWeekNumber ,
                            Global.Settings.GeneralizeSlot));
                }
                #region NestedFunctions

                bool CanGoToPage(int pageNumber)
                {
                    dynamic doc = Browser.Document;
                    string htmlText = doc.documentElement.InnerHtml;
                    return htmlText.Contains($"javascript:changePage(\'{pageNumber}\')");
                }
                void TryGetStartDateAndEndDate(string input)
                {
                    try {
                        var parser = new StartDateEndDateFinder(input);
                        Global.TimetableStartDate = parser.GetStartDate();
                        Global.TimetableEndDate = parser.GetEndDate();
                    }
                    catch { }
                }
                #endregion

            }
            #endregion
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

        private bool CheckForInternetConnection() {
            if (Helper.CanConnectToWebsite(_urlProvider.LoginPageUrl)) {
                DrawerHost.IsBottomDrawerOpen = false;
                return true;
            }
            else {
                DrawerHost.IsBottomDrawerOpen = true;
                return false;
            }
        }

        private string GetHtml(WebBrowser b) {
            dynamic doc = b.Document;
            var htmlText = doc.documentElement.InnerHtml;
            return htmlText;
        }
        #region EventHandlers

        private void ResetButton_OnClick(object sender , RoutedEventArgs e) {
            UserNameBox.Text = "";
            PasswordBox.Password = "";
            CaptchaBox.Text = "";
            Browser.Navigate(_urlProvider.LoginPageUrl);
        }


        private void GotItButton_OnClick(object sender , RoutedEventArgs e) {
            DialogHost.IsOpen = false;
            Browser.Navigate(_urlProvider.LoginPageUrl);
            Browser.Visibility = Visibility.Visible;
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
                Browser.InvokeScript("execScript" ,
                    "document.getElementsByName('reqFregkey')[0].value='" + _studentIdInput + "'" , "JavaScript");
                Browser.InvokeScript("execScript" ,
                    "document.getElementsByName('reqPassword')[0].value='" + _passwordInput + "'" , "JavaScript");
                Browser.InvokeScript("execScript" ,
                    "document.getElementsByName('kaptchafield')[0].value='" + _captchaInput + "'" , "JavaScript");
                Browser.InvokeScript("execScript" ,
                    "document.getElementsByName('_submit')[0].click()" , "JavaScript");
            }
            catch (Exception ex) {
                //If the flow went here, it may be due to the following steps : 
                // 1. User login into account, and successfully loaded slots
                // 2. User clicked back
                // 3. User login again
                ResetButton_OnClick(null , null);
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

        private void DisplayLoadingBar(bool displayIt) {
            KapchaBrowser.Visibility = displayIt ? Visibility.Hidden : Visibility.Visible;
            DrawerHost.IsTopDrawerOpen = displayIt;
            DrawerHost.IsEnabled = !displayIt;
        }
    }
}