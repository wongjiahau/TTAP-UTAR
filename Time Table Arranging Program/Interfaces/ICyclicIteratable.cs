using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Time_Table_Arranging_Program.Interfaces {
    public interface ICountable {
        int Counts { get; }
    }

    public interface ICyclicIteratable : ICountable {
        void GoToNext();
        void GoToPrevious();
        void GoToRandom();
        void GoTo(int index);
        int MaxIndex();
        int CurrentIndex();
    }


    public interface ICyclicIteratable<out T> : ICyclicIteratable {
        T GetCurrent();
    }


    public class CyclicIterator : ICyclicIteratable<int> {
        private int _iterator;
        private readonly int _maxIndex;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="maxIndex">Zero-based index</param>
        public CyclicIterator(int maxIndex) {
            _maxIndex = maxIndex;
        }

        public void GoToNext() {
            _iterator ++;
            if (_iterator > _maxIndex) _iterator = 0;
        }

        public void GoToPrevious() {
            _iterator--;
            if (_iterator < 0) _iterator = _maxIndex;
        }

        public void GoToRandom() {
            var rand = new Random();
            _iterator = rand.Next(0, _maxIndex);
        }

        public void GoTo(int index) {
            _iterator = index;
        }

        public int MaxIndex() {
            return _maxIndex;
        }

        public int CurrentIndex() {
            return _iterator;
        }

        public int GetCurrent() {
            return _iterator;
        }

        public int Counts { get; }
    }

    //[Obsolete]
    //public class InfiniteIteratable<TParent, TChild> : IInfiniteIteratable where TParent : ICollection<TChild>, IIndexable<TChild> {
    //    private TParent _content;
    //    private int _iterator = 0;

    //    public InfiniteIteratable(TParent content) {
    //        _content = content;
    //    }

    //    public void GoToNext() {
    //        _iterator++;
    //        if (_iterator == _content.Count) {
    //            _iterator = 0;
    //        }
    //    }

    //    public void GoToPrevious() {
    //        _iterator--;
    //        if (_iterator == -1) {
    //            _iterator = _content.Count - 1;
    //        }
    //    }

    //    public void GoToRandom() {
    //        var rand = new Random();
    //        _iterator = rand.Next(0 , _content.Count - 1);
    //    }

    //}
}