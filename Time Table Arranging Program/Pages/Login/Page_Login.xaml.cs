using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using ExtraTools;
using mshtml;
using Microsoft.Win32;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.ConfigFileManager;
using Time_Table_Arranging_Program.Class.Helper;
using Time_Table_Arranging_Program.Class.TokenParser;
using Time_Table_Arranging_Program.Pages.Login;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program.Pages {
    public partial class Page_Login : Page {
        //This two line is to prevent memory leak
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        private const int NavigationCountUpperLimit = 3;
        private readonly bool _browsingToCourseTimetablePreview = false;
        private readonly bool _loadDataFromTestServer;
        private readonly UrlProvider _urlProvider;
        private string _captchaInput;
        private int _currentPage = 1;
        private int _navigationCount;
        private string _passwordInput;
        private string _studentIdInput;

        public Page_Login() {
            _urlProvider = new UrlProvider();
            InitializeComponent();
            bool gotInternet = CheckForInternetConnection();
            if (gotInternet) Loaded += Page_Login_Loaded;
        }

        public Page_Login(bool loadDataFromTestServer) : this() {
            _loadDataFromTestServer = loadDataFromTestServer;
        }

        private void Page_Login_Loaded(object sender , RoutedEventArgs e) {
            if (_loadDataFromTestServer) {
                Browser.Navigate(_urlProvider.TestServerUrl);
                return;
            }
            if (Global.InputSlotList.Count == 0) {
                Browser.Navigate(_urlProvider.LoginPageUrl);
                InitializeUserIdBox();
            }
            else {
                DialogBox.Show("Login again?" , "WARNING : If you login again your previous data will be overwritten." ,
                    "CANCEL" , "LOGIN AGAIN");
                switch (DialogBox.Result) {
                    case DialogBox.Result_.LeftButtonClicked:
                        NavigationService.GoForward();
                        break;
                    case DialogBox.Result_.RightButtonClicked:
                        Global.InputSlotList.Clear();
                        //Need to reset two times to properly load the login page again
                        ResetButton_OnClick(null , null);
                        ResetButton_OnClick(null , null);
                        break;
                }
            }
        }

        private void InitializeUserIdBox() {
            var possibleStudentIds = new DataManager().GetStudentIds();
            foreach (string id in possibleStudentIds) {
                UserIdBox.Items.Add(id);
            }
        }

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
                Global.Snackbar.MessageQueue.Enqueue(
                    "Login failed. Please make sure you entered the correct information.");
                NavigationService.Refresh();
                CaptchaBox.Text = "";
                _navigationCount = 0;
            }

            void NavigateToCourseTimeTablePreview()
            {
                new DataManager().SaveData(new UserInfo(_studentIdInput , _passwordInput));
                _currentPage = 1;
                if (_browsingToCourseTimetablePreview) return;
                if (_navigationCount < NavigationCountUpperLimit) {
                    Browser.Navigate(_urlProvider.CourseTimetablePreviewUrl);
                    _navigationCount++;
                }
                else {
                    Browser.Navigate(_urlProvider.LoginPageUrl);
                    Global.Snackbar.MessageQueue.Enqueue($"No record found.");
                    CaptchaBox.Text = "";
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
            DrawerHost.IsBottomDrawerOpen = true;
            return false;
        }

        private string GetHtml(WebBrowser b) {
            dynamic doc = b.Document;
            var htmlText = doc.documentElement.InnerHtml;
            return htmlText;
        }

        private void DisplayLoadingBar(bool displayIt) {
            KapchaBrowser.Visibility = displayIt ? Visibility.Hidden : Visibility.Visible;
            DrawerHost.IsTopDrawerOpen = displayIt;
            DrawerHost.IsEnabled = !displayIt;
        }

        #region EventHandlers

        private void ResetButton_OnClick(object sender , RoutedEventArgs e) {
            Browser.Navigate(_urlProvider.LoginPageUrl);
        }

        private void KapchaBrowser_OnLoadCompleted(object sender , NavigationEventArgs e) {
            KapchaBrowser.InvokeScript("execScript" , "document.body.style.overflow ='hidden'" , "JavaScript");
            CopyBrowserImageToClipboard();
            LoadCopiedImageIntoImageControl();
            void CopyBrowserImageToClipboard()
            {
                //https://stackoverflow.com/questions/2566898/save-images-in-webbrowser-control-without-redownloading-them-from-the-internet
                var doc = (HTMLDocument)KapchaBrowser.Document;
                var imgRange = (IHTMLControlRange)((HTMLBody)doc.body).createControlRange();
                foreach (IHTMLImgElement img in doc.images) {
                    imgRange.add((IHTMLControlElement)img);
                    imgRange.execCommand("Copy" , false , null);
                }
            }

            void LoadCopiedImageIntoImageControl()
            {
                //https://stackoverflow.com/questions/25749843/wpf-image-source-clipboard-getimage-is-not-displayed
                if (!Clipboard.ContainsImage()) return;
                var clipboardData = System.Windows.Forms.Clipboard.GetDataObject();
                if (clipboardData == null) return;
                if (!clipboardData.GetDataPresent(System.Windows.Forms.DataFormats.Bitmap)) return;
                var bitmap = (Bitmap)clipboardData.GetData(System.Windows.Forms.DataFormats.Bitmap);
                var hBitmap = bitmap.GetHbitmap();
                try {
                    KapchaImage.Source = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hBitmap,
                        IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                }
                finally {
                    DeleteObject(hBitmap);
                }
            }
        }



        private void LoginButton_OnClick(object sender , RoutedEventArgs e) {
            if (!CheckForInternetConnection()) return;
            try {
                _studentIdInput = UserIdBox.Text;
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

        private void PasswordBox_OnGotKeyboardFocus(object sender , KeyboardFocusChangedEventArgs e) {
            var passwordToBeFilled = new DataManager().TryGetPassword(UserIdBox.Text);
            if (passwordToBeFilled != null) PasswordBox.Password = passwordToBeFilled;
        }
    }
}