using System.Collections;
using System.Collections.Generic;
using Time_Table_Arranging_Program.Class.AbstractClass;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class {
    public interface ITimetable : IHashedList<Slot> {
        bool IsLiked { get; set; }
        List<Slot> Slots { get; set; }
    }

    public class Timetable : ObservableObject, ITimetable {
        public static readonly ITimetable Empty = new Timetable(new HashedList<Slot>(new List<Slot>()));
        private IHashedList<Slot> _hashedSlotList;

        private bool _isLiked;


        public Timetable(HashedList<Slot> slotList) {
            _hashedSlotList = slotList;
            IsLiked = false;
        }

        public List<Slot> Slots {
            get { return _hashedSlotList.ToList(); }
            set { SetProperty(ref _hashedSlotList, new HashedList<Slot>(value)); }
        }

        public List<Slot> ToList() {
            return _hashedSlotList.ToList();
        }

        public int GetUid() {
            return _hashedSlotList.GetUid();
        }

        public IEnumerator<Slot> GetEnumerator() {
            return _hashedSlotList.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public void Add(Slot item) {
            _hashedSlotList.Add(item);
        }

        public void Clear() {
            _hashedSlotList.Clear();
        }

        public bool Contains(Slot item) {
            return _hashedSlotList.Contains(item);
        }

        public void CopyTo(Slot[] array, int arrayIndex) {
            _hashedSlotList.CopyTo(array, arrayIndex);
        }

        public bool Remove(Slot item) {
            return _hashedSlotList.Remove(item);
        }

        public int Count => _hashedSlotList.Count;
        bool ICollection<Slot>.IsReadOnly { get; }

        public bool IsLiked {
            get { return _isLiked; }
            set { SetProperty(ref _isLiked, value); }
        }
    }
}