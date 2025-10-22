using Strawhenge.Common.Logging;
using Strawhenge.Interactions.Furniture;

namespace Strawhenge.Interactions.Tests;

class Chair : Furniture<UserContext>
{
    int _onUseInvoked;
    int _onEndUseInvoked;
    UserContext? _userContext;

    public Chair(ILogger logger) : base(logger)
    {
    }

    public override string Name => nameof(Chair);

    protected override void OnUse(UserContext userContext)
    {
        _onUseInvoked++;
        _userContext = userContext;
    }

    protected override void OnEndUse() => _onEndUseInvoked++;

    public void VerifyUserContextReceived(UserContext expectedUserContext) =>
        Assert.Same(expectedUserContext, _userContext);

    public void VerifyOnUseInvokedOnce() => Assert.Equal(1, _onUseInvoked);

    public void VerifyOnUseInvokedNever() => Assert.Equal(0, _onUseInvoked);

    public void VerifyOnEndUseInvokedOnce() => Assert.Equal(1, _onEndUseInvoked);

    public void InvokeEnded() => Ended();

    public void Deactivate() => IsDeactivated = true;

    public void Activate() => IsDeactivated = false;
}