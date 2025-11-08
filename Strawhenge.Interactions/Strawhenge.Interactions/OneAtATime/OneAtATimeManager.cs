using System;
using System.Collections.Generic;
using FunctionalUtilities;

namespace Strawhenge.Interactions
{
    public class OneAtATimeManager<T> where T : OneAtATime
    {
        readonly Queue<Action> _skippedCallbacks = new Queue<Action>();

        T _current;
        Action _currentCallback;

        T _next;
        Action _nextCallback;

        public Maybe<T> Current => Maybe.NotNull(_current);

        public void Start(T oneAtATime, Action callback = null)
        {
            if (_current == null)
                SetCurrent(oneAtATime, callback);
            else
            {
                if (_nextCallback != null)
                    _skippedCallbacks.Enqueue(_nextCallback);

                _next = oneAtATime;
                _nextCallback = callback;
                _current.Stop();
            }
        }

        public void Stop()
        {
            _current?.Stop();
        }

        void SetCurrent(T oneAtATime, Action callback)
        {
            _current = oneAtATime;
            _current.Stopped += OnCurrentStopped;
            _currentCallback = callback;

            _current.Start();
        }

        void OnCurrentStopped()
        {
            _current.Stopped -= OnCurrentStopped;
            _current = null;

            var callback = _currentCallback;
            _currentCallback = null;
            callback?.Invoke();

            foreach (var skippedCallback in _skippedCallbacks.DequeueAll())
                skippedCallback.Invoke();

            if (_next != null)
            {
                var next = _next;
                var nextCallback = _nextCallback;
                _next = null;
                _nextCallback = null;

                SetCurrent(next, nextCallback);
            }
        }
    }
}