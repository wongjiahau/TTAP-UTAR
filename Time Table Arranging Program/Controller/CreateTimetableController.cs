using Time_Table_Arranging_Program.Model;

namespace Time_Table_Arranging_Program.Controller {
    public interface ICreateTimetableController {
        void SelectSlot(int uid);
        void DeselectSlot(int uid);
        void SelectSubject(string subjectName);
        void DeselectSubject(string subjectName);
    }

    public class CreateTimetableController : ICreateTimetableController {
        private readonly SlotListModel _slotListModel;

        public CreateTimetableController(SlotListModel slotListModel) {
            _slotListModel = slotListModel;
        }

        public void SelectSlot(int uid) {
            _slotListModel.SelectSlot(uid, true);
        }

        public void DeselectSlot(int uid) {
            _slotListModel.SelectSlot(uid, false);
        }

        public void SelectSubject(string subjectName) {
            _slotListModel.SelectSubject(subjectName, true);
        }

        public void DeselectSubject(string subjectName) {
            _slotListModel.SelectSubject(subjectName, false);
        }
    }
}