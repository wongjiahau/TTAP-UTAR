using System;
using System.Windows.Input;

namespace Time_Table_Arranging_Program.Class.Helper {
    //Refer https://stackoverflow.com/questions/3480966/display-hourglass-when-application-is-busy
    /// <summary>
    /// Purpose of this class is to let the cursor display waiting while the app is busy
    /// Refer StackOverflow on how to use this class
    /// </summary>
    public class WaitCursor : IDisposable {
        private readonly Cursor _previousCursor;

        public WaitCursor() {
            _previousCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
        }

        #region IDisposable Members

        public void Dispose() {
            Mouse.OverrideCursor = _previousCursor;
        }

        #endregion
    }
}