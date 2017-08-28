using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class.UndoFramework;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_UndoManager {
        private UndoManager _undoManager = new UndoManager();
        private static IntObject _intObject = new IntObject();
        private AddOneAction _addOneAction = new AddOneAction(_intObject);
        private AddTwoAction _addTwoAction = new AddTwoAction(_intObject);
        [Test]
        public void Test_UndoManager_Execute() {

            _undoManager.Execute(_addOneAction);
            Assert.AreEqual(1 , _intObject.Value);
        }


    }

    #region MockActions
    public class AddOneAction : UndoableAction {
        public override void Execute() {
            (Target as IntObject).Value += 1;
        }

        public override void Undo() {
            (Target as IntObject).Value -= 1;
        }

        public override string ToolTipMessage { get; }
        public AddOneAction(IntObject target) : base(target) { }
    }

    public class AddTwoAction : UndoableAction {
        public AddTwoAction(IntObject target) : base(target) { }
        public override void Execute() {
            (Target as IntObject).Value += 2;
        }

        public override void Undo() {
            (Target as IntObject).Value -= 2;
        }

        public override string ToolTipMessage { get; }
    }

    public class IntObject {
        public int Value;

        public void Reset() {
            Value = 0;
        }
    }
    #endregion

}
