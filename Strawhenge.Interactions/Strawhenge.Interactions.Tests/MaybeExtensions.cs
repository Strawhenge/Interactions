using FunctionalUtilities;

namespace Strawhenge.Interactions.Tests;

static class MaybeExtensions
{
    public static void VerifyIsNone<T>(this Maybe<T> maybe)
    {
        Assert.NotNull(maybe);
        Assert.False(maybe.HasSome());
    }

    public static T VerifyIsSome<T>(this Maybe<T> maybe)
    {
        Assert.NotNull(maybe);
        Assert.True(maybe.HasSome(out var value));

        return value;
    }

    public static T VerifyIsSome<T>(this Maybe<T> maybe, T expectedValue)
    {
        var actualValue = maybe.VerifyIsSome();
        Assert.Equal(expectedValue, actualValue);
        return actualValue;
    }
}