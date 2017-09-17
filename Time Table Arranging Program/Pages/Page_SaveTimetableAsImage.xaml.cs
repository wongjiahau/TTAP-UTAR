using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using Microsoft.Win32;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.Helper;

namespace Time_Table_Arranging_Program.Pages {
    /// <summary>
    /// Interaction logic for Page_SaveTimetableAsImage.xaml
    /// </summary>
    public partial class Page_SaveTimetableAsImage : Page {
        public Page_SaveTimetableAsImage(ITimetable input) {
            InitializeComponent();
            TimeTableGui.GenerateGUI(input);
        }

        private void SaveImageButton_OnClick(object sender, RoutedEventArgs e) {
            var saveDialog = new SaveFileDialog {Filter = "Image file (*.png)|*.png", FileName = "MyTimetable"};
            if (saveDialog.ShowDialog() == false) return;
            try {
                using (new WaitCursor()) {
                    var bitmap = Helper.GetImage(TimeTableGui);
                    Helper.SaveAsPng(bitmap, saveDialog.FileName);
                }
                Global.Snackbar.MessageQueue.Enqueue("File saved at " + saveDialog.FileName, "OPEN",
                    () => { Process.Start(saveDialog.FileName); });
            }

            catch (Exception ex) {
                Global.Snackbar.MessageQueue.Enqueue("Failed to save file.", "SHOW DETAILS",
                    () => { MessageBox.Show(ex.Message); });
            }
        }

        private void Page_SaveTimetableAsImage_OnLoaded(object sender, RoutedEventArgs e) {
            SaveImageButton_OnClick(null, null);
        }
    }
}