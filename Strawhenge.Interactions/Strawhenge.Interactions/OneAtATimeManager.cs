using System;
using System.Collections.Generic;
using System.Linq;

namespace Strawhenge.Interactions
{
    public class OneAtATimeManager
    {
        readonly Queue<Action> _skippedCallbacks = new Queue<Action>();

        OneAtATime _current;
        Action _currentCallback;

        OneAtATime _next;
        Action _nextCallback;

        public void Start(OneAtATime oneAtATime, Action callback = null)
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

        void SetCurrent(OneAtATime oneAtATime, Action callback)
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

            while (_skippedCallbacks.Any())
                _skippedCallbacks.Dequeue().Invoke();

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