namespace Strawhenge.Interactions.Tests;

public partial class OneAtATimeTests
{
    readonly OneAtATimeManager _oneAtATimeManager = new();

    [Fact]
    public void Start_should_start_emote()
    {
        var emote = new Emote();

        _oneAtATimeManager.Start(emote);

        Assert.True(emote.HasCalledOnStart);
    }

    [Fact]
    public void Stop_should_stop_emote()
    {
        var emote = new Emote();

        _oneAtATimeManager.Start(emote);
        _oneAtATimeManager.Stop();

        Assert.True(emote.HasCalledOnStop);
    }

    [Fact]
    public void Callback_should_invoke_when_emote_stopped()
    {
        var emote = new Emote();
        var callbackInvoked = false;

        _oneAtATimeManager.Start(emote, () => callbackInvoked = true);
        Assert.False(callbackInvoked);

        _oneAtATimeManager.Stop();
        Assert.False(callbackInvoked);

        emote.InvokeOnStoppedCallback();
        Assert.True(callbackInvoked);
    }

    [Fact]
    public void Start_should_stop_previous_emote()
    {
        var previousEmote = new Emote();
        _oneAtATimeManager.Start(previousEmote);

        var emote = new Emote();
        _oneAtATimeManager.Start(emote);

        Assert.True(previousEmote.HasCalledOnStop);
    }

    [Fact]
    public void Start_should_start_emote_when_previous_emote_stopped()
    {
        var previousEmote = new Emote();
        _oneAtATimeManager.Start(previousEmote);

        var emote = new Emote();
        _oneAtATimeManager.Start(emote);
        Assert.False(emote.HasCalledOnStart);

        previousEmote.InvokeOnStoppedCallback();
        Assert.True(emote.HasCalledOnStart);
    }

    [Fact]
    public void Callback_should_invoke_when_previous_emote_has_not_stopped_and_new_emote_called_to_start()
    {
        var previousEmote = new Emote();
        _oneAtATimeManager.Start(previousEmote);

        var emote = new Emote();
        var callbackInvoked = false;
        _oneAtATimeManager.Start(emote, () => callbackInvoked = true);

        var nextEmote = new Emote();
        _oneAtATimeManager.Start(nextEmote);

        Assert.True(callbackInvoked);
    }
}