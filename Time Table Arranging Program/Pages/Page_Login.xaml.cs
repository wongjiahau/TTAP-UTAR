using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HtmlAgilityPack;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Helper;
using Time_Table_Arranging_Program.Class.SlotGeneralizer;
using Time_Table_Arranging_Program.Class.TokenParser;
using Time_Table_Arranging_Program.User_Control;
using Time_Table_Arranging_Program.Windows_Control;

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

        private const string TestServerUrl = "http://localhost/ttap_testdata/";

        private int _currentPage = 1;
        private const int NavigationCountUpperLimit = 3;
        private int _navigationCount = 0;
        public Page_Login() {
            InitializeComponent();
        }

        private bool _loadDataFromTestServer;
        public Page_Login(bool loadDataFromTestServer) :this(){
            _loadDataFromTestServer = loadDataFromTestServer;
        }

        private void Page_First_OnLoaded(object sender , RoutedEventArgs e) {
            GotItButton_OnClick(null , null);
            if (_loadDataFromTestServer) {
                Browser.Navigate(TestServerUrl);
                return;
            }
            Browser.Navigate(LoginPageUrl);
        }

        private bool _browsingToCourseTimetablePreview = false;

        private void Browser_OnLoadCompleted(object sender , NavigationEventArgs e) {
            Browser.InvokeScript("execScript" , "document.documentElement.style.overflow ='hidden'" , "JavaScript");
            RefreshButton.IsEnabled = true;
            string currentUrl = Browser.Source.ToString();
            if (currentUrl.Contains(TestServerUrl)) goto here;
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

        public static string GetPlainTextOfHtml(WebBrowser b) {
            dynamic doc = b.Document;
            var htmlText = doc.documentElement.InnerHtml;
            return htmlText.RemoveTags();
        }


        private void AddSlotManuallyButton_OnClick(object sender , RoutedEventArgs e) {
            NavigationService.Navigate(new Page_AddSlot());
        }

        private void RefreshButton_OnClick(object sender , RoutedEventArgs e) {
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

        private void LoginButton_OnClick(object sender , RoutedEventArgs e) {
            MessageBox.Show("Not implemented yet");
        }


    }
}