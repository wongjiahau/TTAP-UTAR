using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Class.SlotGeneralizer;
using Time_Table_Arranging_Program.Pages;
using Time_Table_Arranging_Program.Windows_Control;

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

        public static class Constant {
            public const int MinTime = 7;
        }

        public static class Factory {
            public static Page_CreateTimetable Generate_Page_CreateTimetable_with_GeneralizedSlots(SlotList slotList) {
                var generalized = new SlotList();
                generalized.AddRange(new SlotGeneralizer().GeneralizeAll(slotList));                
                return new Page_CreateTimetable(
                    generalized ,
                    Permutator.Run_v2_withoutConsideringWeekNumber
                );
            }

            public static Page_CreateTimetable Generate_Page_CreateTimetable_with_UngeneralizedSlots(SlotList slotList) {
               return new Page_CreateTimetable(
                    slotList , 
                    Permutator.Run_v2_WithConsideringWeekNumber
               );

            }
        }

        public static class Condition {
            public static bool NoSlotIsGeneralized  = false;
        }
    }

    public static class DialogBox {
        private static DialogHost _dialogHost;
        private static TextBlock _titleBlock;
        private static TextBlock _messageBlock;
        private static Button _okButton;

        public static void Initialize(DialogHost dialogHost , TextBlock titleBlock , TextBlock messageBlock ,
                                      Button OkButton) {
            _dialogHost = dialogHost;
            _titleBlock = titleBlock;
            _messageBlock = messageBlock;
            _okButton = OkButton;
        }

        public static void ShowDialog(string title , string message , string buttonMessage = "Got it!") {
            _titleBlock.Text = title;
            _messageBlock.Text = message;
            _okButton.Content = buttonMessage;
            _dialogHost.IsOpen = true;
        }
    }
}