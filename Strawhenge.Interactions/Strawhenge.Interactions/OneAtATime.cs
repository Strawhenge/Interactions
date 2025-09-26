using System;

namespace Strawhenge.Interactions
{
    public abstract class OneAtATime
    {
        internal Action Callback { get; set; }

        internal void Start()
        {
            OnStart();
        }

        internal void Stop(Action onStopped = null)
        {
            OnStop(() =>
            {
                onStopped?.Invoke();
                Callback?.Invoke();
            });
        }

        protected abstract void OnStart();

        protected abstract void OnStop(Action onStopped);
    }
}