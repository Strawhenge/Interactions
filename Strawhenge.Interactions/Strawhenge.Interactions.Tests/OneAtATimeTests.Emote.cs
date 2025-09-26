namespace Strawhenge.Interactions.Tests;

public partial class OneAtATimeTests
{
    class Emote : OneAtATime
    {
        public bool HasCalledOnStart { get; private set; }

        public bool HasCalledOnStop { get; private set; }

        Action? _onStopped;

        protected override void OnStart()
        {
            HasCalledOnStart = true;
        }

        protected override void OnStop(Action onStopped)
        {
            HasCalledOnStop = true;
            _onStopped = onStopped;
        }

        public void InvokeOnStoppedCallback()
        {
            _onStopped?.Invoke();
        }
    }
}