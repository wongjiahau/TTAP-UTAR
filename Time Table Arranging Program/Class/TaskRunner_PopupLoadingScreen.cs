using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Time_Table_Arranging_Program.Interfaces;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program.Class {
    public class PopupLoadingScreenTaskRunner : ITaskRunnerWithProgressFeedback {
        private readonly string _message;
        public PopupLoadingScreenTaskRunner(string message) {
            _message = message;
        }

        public async void RunTask(Action action) {
            var w = new BasicLoadingScreen() { Message = _message };
            w.Show();
            await Task.Run(() => { action.Invoke(); });
            w.Close();
        }
    }
}
