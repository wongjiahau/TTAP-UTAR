using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Time_Table_Arranging_Program.Interfaces {
    public interface IObserver {
        void Update();
    }

    /// <summary>
    /// Pure observer cannot mutate the state of subject
    /// </summary> 
    public interface IPureObserver<T> : IObserver {
        void SetObservedThings(IImmutableObservable<T> x);
    }

    /// <summary>
    /// Dirty observer CAN possibly mutate the state of subject
    /// </summary> 
    public interface IDirtyObserver<T> : IObserver {
        void SetObservedThings(MutableObservable<T> x);
    }

    public interface IImmutableObservable<T> {
        void RegisterObserver(IObserver o);
        void RemoveObserver(IObserver o);
        void NotifyObserver();
        T GetCurrentState();
    }


    public interface IMutableObservable<T> : IImmutableObservable<T> {
        void SetState(T newState);
    }

    public class ImmutableObservable<T> : IImmutableObservable<T> {
        private readonly List<IObserver> _observers = new List<IObserver>();
        protected T _currentState;
        protected T _previousState;

        protected ImmutableObservable(T initialState) {
            Assert.IsTrue(initialState != null);
            _currentState = initialState;
            _previousState = initialState;
        }

        public void RegisterObserver(IObserver o) {
            _observers.Add(o);
        }

        public void RemoveObserver(IObserver o) {
            _observers.Remove(o);
        }

        public void NotifyObserver() {
            foreach (var o in _observers) {
                o.Update();
            }
        }

        public T GetCurrentState() {
            return _currentState;
        }

        public T GetPreviousState() {
            return _previousState;
        }
    }

    public class MutableObservable<T> : ImmutableObservable<T>, IMutableObservable<T> {
        public MutableObservable(T initialState) : base(initialState) { }

        public void SetState(T newState) {
            _previousState = _currentState;
            _currentState = newState;
            NotifyObserver();
        }
    }
}