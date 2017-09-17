using System;
using NUnit.Framework;
using Time_Table_Arranging_Program.Class.UndoFramework;

namespace NUnit.Tests2 {
    [TestFixture]
    public class Test_UndoManager {
        private readonly UndoManager _undoManager = new UndoManager();
        private static readonly IntObject _intObject = new IntObject();
        private readonly AddOneAction _addOneAction = new AddOneAction(_intObject);
        private readonly AddTwoAction _addTwoAction = new AddTwoAction(_intObject);
        [Test]
        public void Test_UndoManager_Execute() {
            _undoManager.ClearHistory();
            _undoManager.Execute(_addOneAction);
            _undoManager.Execute(_addTwoAction);
            Assert.AreEqual(3 , _intObject.Value);
        }

        [Test]
        public void Test_UndoManager_Undo_1() {
            _intObject.Reset();
            _undoManager.ClearHistory();
            _undoManager.Execute(_addOneAction);
            _undoManager.Execute(_addTwoAction);
            _undoManager.Undo();
            Assert.AreEqual(1 , _intObject.Value);
        }

        [Test]
        public void Test_UndoManager_Undo_2() {
            _intObject.Reset();
            _undoManager.ClearHistory();
            _undoManager.Execute(_addOneAction);
            _undoManager.Execute(_addTwoAction);
            _undoManager.Undo();
            _undoManager.Undo();
            Assert.AreEqual(0 , _intObject.Value);
        }

        [Test]
        public void Test_UndoManager_Redo() {
            _intObject.Reset();
            _undoManager.ClearHistory();
            _undoManager.Execute(_addOneAction);
            _undoManager.Execute(_addTwoAction);
            _undoManager.Undo();
            _undoManager.Redo();
            Assert.AreEqual(3 , _intObject.Value);
        }

        [Test]
        public void Test_UndoManager_UndoingARedo() {
            _intObject.Reset();
            _undoManager.ClearHistory();
            _undoManager.Execute(_addOneAction);
            _undoManager.Execute(_addTwoAction);
            _undoManager.Undo();
            _undoManager.Redo();
            _undoManager.Undo();
            Assert.AreEqual(1 , _intObject.Value);
        }

        [Test]
        public void Test_UndoManager_WhenUndoStackIsEmpty() {
            _undoManager.ClearHistory();
            try {
                _undoManager.Undo();
            }

            catch (Exception e) {
                Assert.Fail(e.Message);
            }
        }
        [Test]
        public void Test_UndoManager_WhenRedoStackIsEmpty() {
            _undoManager.ClearHistory();
            try {
                _undoManager.Redo();
            }

            catch (Exception e) {
                Assert.Fail(e.Message);
            }
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
