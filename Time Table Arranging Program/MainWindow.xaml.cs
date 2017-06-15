using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Threading;
using Microsoft.Win32;
using NUnit.Tests2;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.GoogleCalendarApi;
using Time_Table_Arranging_Program.MVVM_Framework.Models;
using Time_Table_Arranging_Program.MVVM_Framework.ViewModels;
using Time_Table_Arranging_Program.Pages;
using Time_Table_Arranging_Program.Properties;
using Time_Table_Arranging_Program.UserInterface;
using Time_Table_Arranging_Program.User_Control;

namespace Time_Table_Arranging_Program {
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private const string FeedbackFontUrl = "https://goo.gl/forms/qKdc6EVGbxspoTaS2";
        private readonly List<string> _previousInputString = new List<string>();

        public MainWindow() {
            //The following two lines of code is to reset the PromptForFeedbackSettings to true
            //Please uncomment it, build it, before actual release
            // Properties.Settings.Default["PromptForFeedback"] = true;
            //  Properties.Settings.Default.Save();

            InitializeComponent();
            Global.MainWindow = this;
            Global.Snackbar = Snackbar;
            DialogBox.Initialize(DialogHost, Title, Message, DialogButton);
            MainFrame.Navigate(new Page_Intro());
        }

        private void MainFrame_OnNavigating(object sender, NavigatingCancelEventArgs e) {
            var ta = new ThicknessAnimation
            {
                Duration = CustomAnimation.FullScreenAnimationDuration,
                DecelerationRatio = CustomAnimation.DecelerationConstant,
                To = new Thickness(0, 0, 0, 0)
            };
            if (e.NavigationMode == NavigationMode.New || e.NavigationMode == NavigationMode.Forward) {
                ta.From = new Thickness(ActualWidth/3, 0, 0, 0);
            }
            else if (e.NavigationMode == NavigationMode.Back) {
                ta.From = new Thickness(0, 0, ActualWidth/3, 0);
            }
            (e.Content as Page)?.BeginAnimation(MarginProperty, ta);
        }

        private void MainWindow_OnClosing(object sender, CancelEventArgs e) {
            e.Cancel = true;
            if ((bool) Settings.Default["PromptForFeedback"]) {
                DialogHost.DialogContent = new PromptUserForFeedbackControl();
                DialogHost.IsOpen = true;
            }
            else {
                Application.Current.Shutdown();
            }
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e) {
            Process.Start(
                new ProcessStartInfo(
                    "https://docs.google.com/presentation/d/1XvdPWeCndWbrfmXBtGR5ZTHIUeXaI_Qx16EFVxdJ8JQ/edit?usp=sharing"));
            e.Handled = true;
        }

        private void AboutButton_Click(object sender, RoutedEventArgs e) {
            MainFrame.Navigate(new Page_About());
        }

        private void GoToCourseRegistrationWebsiteButton_OnClick(object sender, RoutedEventArgs e) {
            Process.Start(
                new ProcessStartInfo(
                    "https://unitreg.utar.edu.my/portal/courseRegStu/login.jsp"));
            e.Handled = true;
        }

        private void GoToAddSlotPage_OnClick(object sender, RoutedEventArgs e) {
            MainFrame.Navigate(new Page_AddSlot());
        }

        private void FeedbackButton_OnClick(object sender, RoutedEventArgs e) {
            Process.Start(new ProcessStartInfo(FeedbackFontUrl));
        }

        private void SaveSlot_OnClick(object sender, RoutedEventArgs e) {
            if (Global.State.FileIsSavedBefore) {
                SaveFile();
                return;
            }
            OpenSaveFileDialog();
        }

        private void SaveSlotAs_OnClick(object sender, RoutedEventArgs e) {
            OpenSaveFileDialog();
        }

        private void OpenSaveFileDialog() {
            var saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "TTAP file (*.ttap)|*.ttap";
            saveFileDialog.FileName = "MySlots";
            string fileName = "";
            if (saveFileDialog.ShowDialog() == true) {
                Global.State.FileIsSavedBefore = true;
                Global.State.LastSavedFileName = saveFileDialog.FileName;
                SaveFile();
            }
        }

        private void SaveFile() {
            var os = new ObjectSerializer();
            bool success = os.SerializeObject(Global.InputSlotList, Global.State.LastSavedFileName);
            if (success) {
                Snackbar.MessageQueue.Enqueue($"File saved at: " + $"{Global.State.LastSavedFileName}");
                //DialogBox.ShowDialog();
            }
        }


        private void LoadSlots_OnClick(object sender, RoutedEventArgs e) {
            var dialog = new OpenFileDialog();
            dialog.Filter = "TTAP file (*.ttap)|*.ttap";
            if (dialog.ShowDialog() == true) {
                var os = new ObjectSerializer();
                Global.InputSlotList = os.DeSerializeObject<SlotList>(dialog.FileName);
                MainFrame.Navigate(new Page_CreateTimetable(Global.InputSlotList));
            }
        }
    }
}