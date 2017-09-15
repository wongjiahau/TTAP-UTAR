using System;

namespace Time_Table_Arranging_Program.Interfaces {
    public interface IProgressIndicator {
        void ShowLoading();
        void HideLoading();
    }

    public class MockProgressIndicator : IProgressIndicator {
        private readonly string _message;

        public MockProgressIndicator(string message) {
            _message = message;
        }

        public void ShowLoading() {
            Console.WriteLine(_message);
        }

        public void HideLoading() {
            Console.WriteLine("Finished.");
        }
    }
}