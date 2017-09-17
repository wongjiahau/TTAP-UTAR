using System;
using System.ComponentModel;
using Time_Table_Arranging_Program.Windows_Control;

namespace Time_Table_Arranging_Program.Class {
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1">Type of input parameter</typeparam>
    /// <typeparam name="T2">Type of returned result OR parameter2</typeparam>
    public interface ICustomBackgroundWorker<T1, T2> {
        TimeSpan GetTimeElapsed { get; }

        T2 GetResult();
    }

    public class CustomBackgroundWorker<T1, T2> : ICustomBackgroundWorker<T1, T2> {
        private readonly Action<T1, T2> _action;
        private readonly Func<T1, T2> _func;
        private readonly T1 _parameter1;
        private readonly T2 _parameter2;
        private readonly TimeSpan _timeElapsed;

        private readonly AbortableBackgroundWorker _worker = new AbortableBackgroundWorker
        {
            WorkerSupportsCancellation = true
        };

        private T2 _result;

        private CustomBackgroundWorker(Func<T1, T2> func, T1 parameter1, string message) {
            _func = func;
            _parameter1 = parameter1;
            _worker.DoWork += _worker_DoWork;
            _timeElapsed = ProgressWindow.ShowLoadingScreen(_worker, message);
        }

        private CustomBackgroundWorker(Action<T1, T2> action, T1 parameter1, T2 parameter2, string message) {
            _action = action;
            _parameter1 = parameter1;
            _parameter2 = parameter2;
            _worker.DoWork += _worker_DoWork;
            _timeElapsed = ProgressWindow.ShowLoadingScreen(_worker, message);
        }

        public T2 GetResult() {
            return _result;
        }

        public TimeSpan GetTimeElapsed => _timeElapsed;

        public static CustomBackgroundWorker<T1, T2> RunAndShowLoadingScreen(Func<T1, T2> func, T1 parameter,
                                                                             string message) {
            return new CustomBackgroundWorker<T1, T2>(func, parameter, message);
        }

        public static CustomBackgroundWorker<T1, T2> RunAndShowLoadingScreen(Action<T1, T2> action, T1 parameter1,
                                                                             T2 parameter2, string message) {
            return new CustomBackgroundWorker<T1, T2>(action, parameter1, parameter2, message);
        }

        private void _worker_DoWork(object sender, DoWorkEventArgs e) {
            if (_func != null)
                _result = _func(_parameter1);

            if (_action != null)
                _action.Invoke(_parameter1, _parameter2);
        }
    }
}