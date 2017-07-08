using System;
using System.Collections.Generic;
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
            if (currentUrl == LoginPageUrl) return;
            //if(currentUrl == LoginSuccessUrl) 
            if(currentUrl == LoginFailedUrl) Browser.Navigate(LoginPageUrl);
                        
            if (currentUrl == LoginPageUrl || currentUrl == LoginFailedUrl || currentUrl == EndUrl) {                
                return;
            }
            if (currentUrl.Contains(CourseTimetablePreviewUrl) == false) {
                _currentPage = 1;
                if (_browsingToCourseTimetablePreview == false) {
                    Browser.Navigate(CourseTimetablePreviewUrl);                    
                }
                return;
            }           
            Browser.Visibility = Visibility.Hidden;
            string plainText = LoadPlainText();
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

        private string LoadPlainText() {
            dynamic doc = Browser.Document;
            var htmlText = doc.documentElement.InnerHtml;
            return htmlText.RemoveTags();
        }
       

        private void AddSlotManuallyButton_OnClick(object sender , RoutedEventArgs e) {
            NavigationService.Navigate(new Page_AddSlot());
        }

        private void RefreshButton_OnClick(object sender , RoutedEventArgs e) {
            Browser.Refresh();
        }

        private void GotItButton_OnClick(object sender , RoutedEventArgs e) {
            DialogHost.IsOpen = false;
            Browser.Navigate(LoginPageUrl);
            Browser.Visibility = Visibility.Visible;
        }

        private void PrintButton_OnClick(object sender, RoutedEventArgs e) {
           
        }
    }
}