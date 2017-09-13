using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Interfaces {
    public interface ITaskRunnerWithProgressFeedback {
        void RunTask(Action action);

    }

    public class MockTaskRunner : ITaskRunnerWithProgressFeedback {
        private readonly ManualResetEvent _event = new ManualResetEvent(false);
        public void RunTask(Action action) {
            _event.Reset();
            Console.WriteLine("Loading . . .");
            RunAsync(action);
            Console.WriteLine("Load completed.");
            _event.WaitOne();
        }

        private async void RunAsync(Action action) {
            await Task.Run(() => action.Invoke());
            _event.Set();
        }


    }
}
