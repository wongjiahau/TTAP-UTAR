using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program.Interfaces {
    public interface ITaskRunnerWithProgressFeedback {
        void RunTask(Action action);
    }

    public abstract class TaskRunnerWithProgressFeedback : ITaskRunnerWithProgressFeedback {
        private readonly Stack<Action> _action = new Stack<Action>();
        private readonly BackgroundWorker _backgroundWorker = new BackgroundWorker();
        protected readonly AutoResetEvent _event = new AutoResetEvent(false);
        protected readonly IProgressIndicator _progressIndicator;

        public TaskRunnerWithProgressFeedback(IProgressIndicator progressIndicator) {
            _progressIndicator = progressIndicator;
            _backgroundWorker.DoWork += _backgroundWorker_DoWork;
            _backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
        }

        public void RunTask(Action action) {
            _action.Push(action);
            while (_backgroundWorker.IsBusy) ;
            _backgroundWorker.RunWorkerAsync();
            PauseAndShowProgressIndicator();
        }

        private void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e) {
            _action.Pop().Invoke();
            _event.Set();
        }

        private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            _progressIndicator.HideLoading();
        }

        protected abstract void PauseAndShowProgressIndicator();
    }

    public class TaskRunnerForMainUi : TaskRunnerWithProgressFeedback {
        public TaskRunnerForMainUi(string message) : base(new ProgressWindow(message)) { }

        protected override void PauseAndShowProgressIndicator() {
            _progressIndicator.ShowLoading();
            //_event.WaitOne() is not needed to be called here, because _progressIndicator.ShowLoading() will already pause the thread
        }
    }

    public class TaskRunnerForUnitTesting : TaskRunnerWithProgressFeedback {
        public TaskRunnerForUnitTesting() : base(new MockProgressIndicator("Loading . . .")) { }

        protected override void PauseAndShowProgressIndicator() {
            _progressIndicator.ShowLoading();
            _event.WaitOne();
        }
    }
}