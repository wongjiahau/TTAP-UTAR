using Time_Table_Arranging_Program.Class;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Model {
    public interface ISlotListModel : IMutableObservable<SlotList> {
        void SelectSlot(int uid, bool isSelected);
        void SelectSubject(string subjectName, bool isSelected);
    }

    public class SlotListModel : ISlotListModel {
        private readonly MutableObservable<SlotList> _observable;
        private readonly SlotList _slotList;

        public SlotListModel(SlotList slotList) {
            _slotList = slotList;
            _observable = new MutableObservable<SlotList>(_slotList);
        }

        public void SelectSlot(int uid, bool isSelected) {
            foreach (var slot in _slotList) {
                if (slot.UID != uid) continue;
                slot.IsSelected = isSelected;
                return;
            }
        }

        public void SelectSubject(string subjectName, bool isSelected) {
            foreach (var slot in _slotList) {
                if (slot.SubjectName == subjectName) {
                    slot.IsSelected = isSelected;
                }
            }
        }

        public void RegisterObserver(IObserver o) {
            _observable.RegisterObserver(o);
        }

        public void RemoveObserver(IObserver o) {
            _observable.RemoveObserver(o);
        }

        public void NotifyObserver() {
            _observable.NotifyObserver();
        }

        public SlotList GetCurrentState() {
            return _observable.GetCurrentState();
        }

        public void SetState(SlotList newState) {
            _observable.SetState(newState);
        }
    }
}