using MaterialDesignThemes.Wpf;
using System;
using System.Windows;
using System.Windows.Controls;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program {
    public static class Global {
        public static int MaxTime = 23;
        public static Window MainWindow;
        public static Snackbar Snackbar;
        public static DateTime TimetableStartDate;
        public static DateTime TimetableEndDate;
        public static SlotList InputSlotList = new SlotList();

        public static class State {
            public static bool FileIsSavedBefore = false;
            public static string LastSavedFileName = "";
        }
    }

    public static class DialogBox {
        private static DialogHost _dialogHost;
        private static TextBlock _titleBlock;
        private static TextBlock _messageBlock;
        private static Button _okButton;

        public static void Initialize(DialogHost dialogHost, TextBlock titleBlock, TextBlock messageBlock,
                                      Button OkButton) {
            _dialogHost = dialogHost;
            _titleBlock = titleBlock;
            _messageBlock = messageBlock;
            _okButton = OkButton;
        }

        public static void ShowDialog(string title, string message, string buttonMessage = "Got it!") {
            _titleBlock.Text = title;
            _messageBlock.Text = message;
            _okButton.Content = buttonMessage;
            _dialogHost.IsOpen = true;
        }
    }
}