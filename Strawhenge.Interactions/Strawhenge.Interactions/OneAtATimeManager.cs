using System;

namespace Strawhenge.Interactions
{
    public class OneAtATimeManager
    {
        OneAtATime _current;
        OneAtATime _next;

        public void Start(OneAtATime oneAtATime, Action callback = null)
        {
            oneAtATime.Callback = callback;

            if (_next != null)
            {
                _next.Callback?.Invoke();
                _next = oneAtATime;
                return;
            }

            if (_current != null)
            {
                _next = oneAtATime;
                _current.Stop(OnCurrentStopped);
                return;
            }

            _current = oneAtATime;
            _current.Start();
        }

        void OnCurrentStopped()
        {
            _current = _next;
            _current?.Start();
        }

        public void Stop()
        {
            _current?.Stop(OnCurrentStopped);
            _current = null;
        }
    }
}