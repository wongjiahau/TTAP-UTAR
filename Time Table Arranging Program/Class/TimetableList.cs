using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class {
    public interface ITimetableList : ConvertibleToListOf<ITimetable>, ICollection<ITimetable>, IIndexable<ITimetable>,
        ICyclicIteratable<ITimetable> {
        string Message { get; }
        void AddUniqueRange(List<ITimetable> newRange);
        bool IsEmpty();
    }

    public class TimetableList : ITimetableList {
        public static readonly TimetableList NoPossibleCombination = new TimetableList("No possible combination :(");
        public static readonly TimetableList NoSlotsIsChosen = new TimetableList("⟸ Search and select your subjects");

        public static readonly TimetableList NoLikedTimetable =
            new TimetableList("👀 No Favorites Yet ! ");

        private readonly CyclicIterator _cyclicIterator = new CyclicIterator(0);
        private List<ITimetable> _timetables = new List<ITimetable>();

        private TimetableList(string message) {
            Message = message;
        }
        
        public TimetableList(List<List<Slot>> inputList) {            
            if (inputList == null) throw new NullReferenceException("inputList should not be null");
            _timetables = new List<ITimetable>();
            foreach (List<Slot> x in inputList) {
                _timetables.Add(new Timetable(new HashedList<Slot>(x)));
            }
            _cyclicIterator = new CyclicIterator(inputList.Count - 1);
        }

        public TimetableList(List<ITimetable> inputList) {
            if (inputList == null) throw new NullReferenceException("inputList should not be null");
            _timetables = inputList;
        }

        public string Message { get; } = "No message";

        public bool IsEmpty() {
            return
                this == NoPossibleCombination ||
                this == NoLikedTimetable ||
                this == NoSlotsIsChosen
                ;
        }

        /// <summary>
        /// Duplicates will not be added
        /// </summary>
        /// <param name="newRange"></param>
        public void AddUniqueRange(List<ITimetable> newRange) {
            foreach (ITimetable t in newRange) {
                if (_timetables.All(x => x.GetUid() != t.GetUid())) {
                    _timetables.Add(t);
                }
            }
        }

        public void Add(ITimetable timetable) {
            _timetables.Add(timetable);
        }

        public void Clear() {
            _timetables.Clear();
        }

        public bool Contains(ITimetable item) {
            return _timetables.Contains(item);
        }

        public void CopyTo(ITimetable[] array, int arrayIndex) {
            throw new NotImplementedException();
        }

        bool ICollection<ITimetable>.Remove(ITimetable item) {
            return _timetables.Remove(item);
        }

        public int Counts => _timetables.Count;
        public int Count => _timetables.Count;

        public bool IsReadOnly { get; }

        public List<ITimetable> ToList() {
            return _timetables;
        }


        public IEnumerator<ITimetable> GetEnumerator() {
            return _timetables.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public ITimetable this[int index] {
            get { return _timetables[index]; }
            set { _timetables[index] = value; }
        }


        public void GoToNext() {
            _cyclicIterator.GoToNext();
        }

        public void GoToPrevious() {
            _cyclicIterator.GoToPrevious();
        }

        public void GoToRandom() {
            _cyclicIterator.GoToRandom();
        }

        public void GoTo(int index) {
            _cyclicIterator.GoTo(index);
        }

        public int MaxIndex() {
            return _cyclicIterator.MaxIndex();
        }

        public int CurrentIndex() {
            return _cyclicIterator.CurrentIndex();
        }

        public ITimetable GetCurrent() {
            return _timetables[_cyclicIterator.GetCurrent()];
        }
      
    }
}