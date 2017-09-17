using System.Collections;
using System.Collections.Generic;
using Time_Table_Arranging_Program.Interfaces;

namespace Time_Table_Arranging_Program.Class {
    public class HashedList<T> : IHashedList<T> {
        private static int _nextUid;
        private readonly List<T> _list;
        private readonly int _uid;

        public HashedList(List<T> list) {
            _list = list;
            _uid = _nextUid++;
        }


        public void Clear() {
            _list.Clear();
        }

        public bool Contains(T item) {
            return _list.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex) {
            _list.CopyTo(array, arrayIndex);
        }

        public bool Remove(T item) {
            return _list.Remove(item);
        }

        public int Count => _list.Count;
        bool ICollection<T>.IsReadOnly { get; }

        public IEnumerator<T> GetEnumerator() {
            return _list.GetEnumerator();
        }


        public void Add(T x) {
            _list.Add(x);
        }

        public int GetUid() {
            return _uid;
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        public List<T> ToList() {
            return _list;
        }
    }
}