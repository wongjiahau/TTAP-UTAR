using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.SlotGeneralizer;
using Time_Table_Arranging_Program.Class.UndoFramework;
using Time_Table_Arranging_Program.Pages;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program {
    public static class Global {
        public static int MaxTime = 23;
        public static Window MainWindow;
        public static Frame MainFrame;
        public static Snackbar Snackbar;
        public static DateTime TimetableStartDate;
        public static DateTime TimetableEndDate;
        public static SlotList InputSlotList = new SlotList();
        public static string LoadedHtml = "";
        public static UndoManager UndoManager = new UndoManager();

        public static class Toggles {
            public static ToggableObject SaveLoadedHtmlToggle = new ToggableObject(false);
        }

        public static class Settings {
            public static Setting SearchByConsideringWeekNumber { private set; get; } =
                new Setting(Setting.SettingDescription.SearchByConsideringWeekNumber,
                    "Search for timetable by considering week number",
                    "Turning this on will allow you to have the chance of getting timetable that contains overlapping timeslots, however this may cause the program to run slower",
                    false);

            public static Setting GeneralizeSlot { private set; get; } =
                new Setting(Setting.SettingDescription.GeneralizedSlot,
                    "Generalize slots",
                    "It means to generalize slots that share the same Day, same Time, same Name and same Type as one slot",
                    true);
        }

        public static class State {
            public static bool FileIsSavedBefore = false;
            public static string LastSavedFileName = "";
        }

        public static class Constant {
            public const int MinTime = 7;
        }


        public static class Condition {
            public static bool NoSlotIsGeneralized = false;
        }
    }

    [Obsolete("", true)]
    public static class DialogBox_Old {
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