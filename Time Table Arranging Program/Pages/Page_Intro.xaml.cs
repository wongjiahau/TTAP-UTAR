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

namespace Time_Table_Arranging_Program.Pages {
    /// <summary>
    /// Interaction logic for Page_First.xaml
    /// </summary>
    public partial class Page_Intro : Page {
        private const string LoginPageUrl = "https://unitreg.utar.edu.my/portal/courseRegStu/login.jsp";

        private const string LoginFailedUrl =
            "https://unitreg.utar.edu.my/portal/courseRegStu/login.jsp?message=invalidSecurity";

        private const string CourseTimetablePreviewUrl =
            "https://unitreg.utar.edu.my/portal/courseRegStu/schedule/masterSchedule.jsp";

        private const string EndUrl = "https://www.google.com/";

        private int _currentPage = 1;
        private const int NavigationCountUpperLimit = 3;
        private int _navigationCount = 0;
        public Page_Intro() {
            InitializeComponent();
        }

        private void Page_First_OnLoaded(object sender , RoutedEventArgs e) {
            GotItButton_OnClick(null , null);
            Browser.Navigate(LoginPageUrl);
        }

        private bool _browsingToCourseTimetablePreview = false;
        private void Browser_OnLoadCompleted(object sender , NavigationEventArgs e) {            
            Browser .InvokeScript("execScript", "document.documentElement.style.overflow ='hidden'", "JavaScript");
            RefreshButton.IsEnabled = true;
            string currentUrl = Browser.Source.ToString();
            //if (currentUrl == LoginPageUrl) return;
            //if(currentUrl == LoginSuccessUrl) 
            //if(currentUrl == LoginFailedUrl) Browser.Navigate(LoginPageUrl);                        
            if (currentUrl == LoginPageUrl || currentUrl == LoginFailedUrl || currentUrl == EndUrl) {
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
            Browser.Visibility = Visibility.Hidden;
            string plainText = GetPlainTextOfHtml(Browser);
            var bg = CustomBackgroundWorker<string , List<Slot>>.RunAndShowLoadingScreen(
                new SlotParser().Parse , plainText , "Loading slots . . .");
            TryGetStartDateAndEndDate(plainText);
            Global.InputSlotList.AddRange(bg.GetResult());
            if (CanGoToPage(_currentPage + 1)) {
                Browser.InvokeScript("changePage" , _currentPage + 1);
                _currentPage++;
            }
            else {
                Browser.Navigate(EndUrl);
                NavigationService.Navigate(
                                Global.Factory
                                    .Generate_Page_CreateTimetable_with_GeneralizedSlots
                                    (Global.InputSlotList));
            }
        }

        private void TryGetStartDateAndEndDate(string input) {
            try {
                var parser = new StartDateEndDateParser(input);
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

        private void LoadTestDataButton_OnClick(object sender, RoutedEventArgs e) {
            Global.InputSlotList.AddRange(TestData.TestSlots); 
            NavigationService.Navigate(
                Global.Factory
                    .Generate_Page_CreateTimetable_with_GeneralizedSlots
                    (Global.InputSlotList));

        }
    }
}