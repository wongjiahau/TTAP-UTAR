using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.MVVM_Framework.Models;
using Time_Table_Arranging_Program.MVVM_Framework.ViewModels;

namespace Time_Table_Arranging_Program.Windows_Control {
    /// <summary>
    /// Interaction logic for SummaryWindow.xaml
    /// </summary>
    public partial class SummaryWindow : Window {
        private static int Shown = 0;

        private static bool HintIsShownBefore = false;
        private readonly CyclicIndex _cylicIndex;
        private readonly ITimetableList _timetableList;

        public SummaryWindow(ITimetableList timetableList, CyclicIndex cyclicIndex) {
            InitializeComponent();
            _timetableList = timetableList;
            _cylicIndex = cyclicIndex;
            _cylicIndex.CurrentValueChanged += CylicIndex_CurrentValueChanged;
            DataContext = new CyclicIndexVM(cyclicIndex);
            if (HintIsShownBefore) HintPanel.Visibility = Visibility.Collapsed;
        }

        private void CylicIndex_CurrentValueChanged(object sender, EventArgs e) {
            DescriptionViewer.Update(_timetableList.ToList()[_cylicIndex.CurrentValue].ToList());
        }


        public void ShowWindow() {
            if (Shown + 1 > 1) return;
            Shown++;
            if (_timetableList.IsEmpty()) {
                return;
            }
            Show();
            CylicIndex_CurrentValueChanged(null, null);
        }

        private void BackButton_OnClick(object sender, RoutedEventArgs e) {
            Close();
        }

        private void SummaryWindow_OnClosing(object sender, CancelEventArgs e) {
            Shown--;
        }


        private void ToggleViewButton_OnClick(object sender, RoutedEventArgs e) {
            TipsLabel.Visibility = Visibility.Collapsed;
            if (MainBorder.Visibility == Visibility.Visible) {
                MainBorder.Visibility = Visibility.Collapsed;
                ToggleViewButton.Content = "Maximize";
            }
            else {
                MainBorder.Visibility = Visibility.Visible;
                ToggleViewButton.Content = "Minimize";
            }
        }

        private void OkButton_OnClick(object sender, RoutedEventArgs e) {
            HintPanel.Visibility = Visibility.Collapsed;
            HintIsShownBefore = true;
        }
    }
}