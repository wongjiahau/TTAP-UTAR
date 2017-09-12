using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Interfaces {
    public interface ITaskRunnerWithProgressFeedback {
        void RunTask(Action action);
    }

    public class MockTaskRunner : ITaskRunnerWithProgressFeedback {
        public async void RunTask(Action action) {
            Console.WriteLine("Loading . . .");
            await Task.Run(() => { action.Invoke(); });
            Console.WriteLine("Load completed.");
        }
    }
}
