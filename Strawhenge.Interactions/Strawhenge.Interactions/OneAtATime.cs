using System;

namespace Strawhenge.Interactions
{
    public abstract class OneAtATime
    {
        internal event Action Stopped;

        internal void Start()
        {
            OnStart();
        }

        internal void Stop()
        {
            OnStop(() => Stopped?.Invoke());
        }

        protected abstract void OnStart();

        protected abstract void OnStop(Action onStopped);

        protected void InvokeStopped() => Stopped?.Invoke();
    }
}