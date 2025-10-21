namespace Strawhenge.Interactions.Tests;

public partial class OneAtATimeTests
{
    class Emote : OneAtATime
    {
        public bool HasCalledOnStart { get; private set; }

        public bool HasCalledOnStopRequested { get; private set; }

        protected override void OnStart()
        {
            HasCalledOnStart = true;
        }

        protected override void OnStopRequested()
        {
            HasCalledOnStopRequested = true;
        }

        public new void InvokeStopped() => base.InvokeStopped();
    }
}