using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using Time_Table_Arranging_Program.Class;

namespace Time_Table_Arranging_Program.Windows_Control {
    /// <summary>
    ///     Interaction logic for ProgressWindow.xaml
    /// </summary>
    public class AbortableBackgroundWorker : BackgroundWorker {
        private Thread _workerThread;

        protected override void OnDoWork(DoWorkEventArgs e) {
            _workerThread = Thread.CurrentThread;
            try {
                base.OnDoWork(e);
            }
            catch (ThreadAbortException) {
                e.Cancel = true; //We must set Cancel property to true!
                Thread.ResetAbort(); //Prevents ThreadAbortException propagation
            }
        }


        public void Abort() {
            if (_workerThread != null) {
                _workerThread.Abort();
                _workerThread = null;
            }
        }
    }


    public partial class ProgressWindow : Window {
        private readonly AbortableBackgroundWorker _backgroundWorker;
        private readonly Stopwatch _timer;


        private ProgressWindow(AbortableBackgroundWorker worker, string message) {
            InitializeComponent();
            this.Left = SystemParameters.PrimaryScreenWidth - this.Width - 10;
            MessageLabel.Content = message;
            _backgroundWorker = worker;
            _backgroundWorker.RunWorkerCompleted += _backgroundWorker_RunWorkerCompleted;
            _timer = Stopwatch.StartNew();
            _backgroundWorker.RunWorkerAsync();
        }

        public static TimeSpan ShowLoadingScreen(AbortableBackgroundWorker bg, string message) {
            var p = new ProgressWindow(bg, message);
            p.ShowDialog();
            return p._timer.Elapsed;
        }

        private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e) {
            if (e.Error != null) throw e.Error;
            _timer.Stop();
            if (e.Cancelled) {
                //unsure what to do yet
            }
            var timespan = _timer.Elapsed;
            if (timespan.TotalSeconds > 1) {
                Hide();
                // MessageBox.Show($"Time elapsed : {timespan.TotalSeconds:00} seconds");
            }
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e) {
            _backgroundWorker.CancelAsync();
            _backgroundWorker.Abort();
        }
    }
}