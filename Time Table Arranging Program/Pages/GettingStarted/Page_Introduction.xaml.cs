using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Time_Table_Arranging_Program.MVVM_Framework;
using Time_Table_Arranging_Program.MVVM_Framework.Models;
using Time_Table_Arranging_Program.MVVM_Framework.ViewModels;
using Time_Table_Arranging_Program.Pages.GettingStarted;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program.Pages.Page_GettingStarted {
    /// <summary>
    /// Interaction logic for Page_GettingStarted.xaml
    /// </summary>
    public partial class Page_Introduction : Page {
        private readonly CyclicIndex _cyclicIndex;
        private readonly List<Type> _pages = new List<Type>()
        {
            typeof(WelcomeToTTAP),
            typeof(DoYouWantToWatchTTAPTutorial),
            typeof(IfYouAreUsingComputerLabInUtar),
            typeof(LetsGetStarted)                       
        };
      
        public Page_Introduction() {
            InitializeComponent();
            _cyclicIndex = new CyclicIndex(_pages.Count - 1);
            _cyclicIndex.CurrentValueChanged += CyclicIndexOnCurrentValueChanged;
            var cyclicIndexVm = new BoundedIndexVM(_cyclicIndex);
            DataContext = cyclicIndexVm;
            
        }

        private void CyclicIndexOnCurrentValueChanged(object sender , EventArgs eventArgs) {            
            Transitioner.SelectedIndex = _cyclicIndex.CurrentValue;
        }

        private void SkipButton_OnClick(object sender , RoutedEventArgs e) {
            NavigationService.Navigate(new Page_Login());
        }
    }
}
