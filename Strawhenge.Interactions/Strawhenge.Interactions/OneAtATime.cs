using System;

namespace Strawhenge.Interactions
{
    public abstract class OneAtATime
    {
        bool _calledStart;
        bool _calledStop;

        internal Action Callback { get; set; }

        internal void Start()
        {
            if (_calledStart) return;
            _calledStart = true;

            OnStart();
        }

        internal void Stop(Action onStopped = null)
        {
            if (_calledStop) return;
            _calledStop = true;

            OnStop(() =>
            {
                onStopped?.Invoke();
                Callback?.Invoke();
            });
        }

        protected abstract void OnStart();

        protected abstract void OnStop(Action onStopped);

        protected void InvokeStop() => Stop();
    }
}