using System.Collections.Generic;
using static Time_Table_Arranging_Program.Permutator;

namespace Time_Table_Arranging_Program.Class {
    public interface IIndices {
        BoundedInt this[int index] { get; }
        int Count { get; }
        List<BoundedInt> GetCurrentIndices();
        bool Increment();
        void AddNewCrashedIndex(KeyValuePair[] k);
        bool CurrentIndicesContainCrashedIndex();
    }

    public class Indices : IIndices {
        private readonly List<BoundedInt> _boundedInts;
        private readonly List<KeyValuePair[]> _crashedIndexList = new List<KeyValuePair[]>();

        public Indices(params int[] upperLimits) {
            _boundedInts = new List<BoundedInt>();
            foreach (var upl in upperLimits) {
                _boundedInts.Add(new BoundedInt(upl, 0));
            }
        }

        public BoundedInt this[int index] => _boundedInts[index];
        public int Count => _boundedInts.Count;

        public void AddNewCrashedIndex(KeyValuePair[] k) {
            _crashedIndexList.Add(k);
        }

        public bool CurrentIndicesContainCrashedIndex() {
            for (var i = 0; i < _crashedIndexList.Count; i++) {
                if (_boundedInts[_crashedIndexList[i][0].Key].Value == _crashedIndexList[i][0].Value
                    &&
                    _boundedInts[_crashedIndexList[i][1].Key].Value == _crashedIndexList[i][1].Value
                    ) return true;
            }

            return false;
        }

        public List<BoundedInt> GetCurrentIndices() {
            return _boundedInts;
        }

        public bool Increment() {
            var pointer = _boundedInts.Count - 1;
            var x = _boundedInts;
            while (true) {
                x[pointer].Value++;
                if (x[pointer].Value > x[pointer].UpperLimit) {
                    x[pointer].Value = 0;
                    pointer--;
                    if (pointer < 0) return false;
                }
                else return true;
            }
        }
    }
}