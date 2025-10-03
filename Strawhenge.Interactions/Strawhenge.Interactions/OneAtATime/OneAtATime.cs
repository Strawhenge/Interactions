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
            OnStopRequested();
        }

        protected abstract void OnStart();

        protected abstract void OnStopRequested();

        protected void InvokeStopped() => Stopped?.Invoke();
    }
}