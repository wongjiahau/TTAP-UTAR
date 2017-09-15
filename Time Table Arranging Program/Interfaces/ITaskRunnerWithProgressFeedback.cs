using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Interfaces {
    public interface ITaskRunnerWithProgressFeedback {
        void RunTask(Action action);

    }

    public class TaskRunnerWithProgressFeedback : ITaskRunnerWithProgressFeedback {
        private readonly IProgressIndicator _progressIndicator;
        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();
        private AutoResetEvent _event = new AutoResetEvent(false);
        private Action _action;

        public TaskRunnerWithProgressFeedback(IProgressIndicator progressIndicator) {
            _progressIndicator = progressIndicator;
            _backgroundWorker.DoWork += _backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
        }

        private void _backgroundWorker_DoWork(object sender , DoWorkEventArgs e) {
            _action.Invoke();
            _event.Set();
        }

        private void _backgroundWorker_RunWorkerCompleted(object sender , RunWorkerCompletedEventArgs e) {
            _progressIndicator.Hide();
        }

        public void RunTask(Action action) {
            _action = action;
            _progressIndicator.Show();
            _backgroundWorker.RunWorkerAsync();
            _event.WaitOne();
        }
    }
}
