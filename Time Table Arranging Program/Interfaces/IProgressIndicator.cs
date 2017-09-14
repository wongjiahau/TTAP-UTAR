using System;

namespace Time_Table_Arranging_Program.Interfaces {
    public interface IProgressIndicator {
        void Show();
        void Hide();
    }

    public class MockProgressIndicator : IProgressIndicator {
        private readonly string _message;
        public MockProgressIndicator(string message) {
            _message = message;
        }
        public void Show() {
            Console.WriteLine(_message);
        }

        public void Hide() {
            Console.WriteLine("Finished.");
        }
    }
}