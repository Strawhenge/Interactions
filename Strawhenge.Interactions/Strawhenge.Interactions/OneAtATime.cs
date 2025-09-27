using System;

namespace Strawhenge.Interactions
{
    public abstract class OneAtATime
    {
        internal event Action Stopped;

        internal OneAtATimeState State { get; private set; } = OneAtATimeState.New;

        internal void Start()
        {
            State = OneAtATimeState.Started;
            OnStart();
        }

        internal void Stop()
        {
            State = OneAtATimeState.Stopping;
            OnStop(OnStopped);
        }

        protected abstract void OnStart();

        protected abstract void OnStop(Action onStopped);

        protected void InvokeStopped() => OnStopped();

        void OnStopped()
        {
            State = OneAtATimeState.Stopped;
            Stopped?.Invoke();
        }
    }
}