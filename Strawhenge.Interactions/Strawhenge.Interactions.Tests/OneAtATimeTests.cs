using Strawhenge.Interactions.OneAtATime;

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
    public void Callback_should_invoke_when_emote_has_called_stop_from_within()
    {
        var emote = new Emote();
        var callbackInvoked = false;
        _oneAtATimeManager.Start(emote, () => callbackInvoked = true);

        emote.InvokeStop();
        Assert.True(callbackInvoked);
    }

    [Fact]
    public void Only_most_recent_emote_should_start_when_previous_emote_stopped()
    {
        var previousEmote = new Emote();
        _oneAtATimeManager.Start(previousEmote);

        var emotes = Enumerable.Range(0, 5)
            .Select(_ => new Emote())
            .ToArray();

        foreach (var emote in emotes)
            _oneAtATimeManager.Start(emote);

        previousEmote.InvokeOnStoppedCallback();

        var mostRecentEmote = emotes.Last();
        Assert.True(mostRecentEmote.HasCalledOnStart);

        foreach (var skippedEmote in emotes.SkipLast(1))
            Assert.False(skippedEmote.HasCalledOnStart);
    }

    [Fact]
    public void Skipped_emotes_should_have_callback_invoked_when_previous_emote_stopped()
    {
        var previousEmote = new Emote();
        _oneAtATimeManager.Start(previousEmote);

        var emotes = Enumerable.Range(0, 5)
            .Select(_ => new Emote())
            .ToArray();

        int callbacksInvoked = 0;
        foreach (var emote in emotes)
            _oneAtATimeManager.Start(emote, () => callbacksInvoked++);

        previousEmote.InvokeOnStoppedCallback();

        var expectedCallbacksInvoked = emotes.Length - 1;
        Assert.Equal(expectedCallbacksInvoked, callbacksInvoked);
    }
}