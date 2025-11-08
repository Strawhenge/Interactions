using Strawhenge.Common.Logging;
using Strawhenge.Interactions.Furniture;

namespace Strawhenge.Interactions.Tests;

class Chair : Furniture.Furniture
{
    int _onUseInvoked;
    int _onEndUseInvoked;
    IFurnitureUserScope? _userScope;

    public Chair(ILogger logger) : base(logger)
    {
    }

    public override string Name => nameof(Chair);

    protected override void OnUse(IFurnitureUserScope userScope)
    {
        _onUseInvoked++;
        _userScope = userScope;
    }

    protected override void OnEndUse() => _onEndUseInvoked++;

    public void VerifyUserScopeReceived(IFurnitureUserScope expectedUserScope) =>
        Assert.Same(expectedUserScope, _userScope);

    public void VerifyOnUseInvokedOnce() => Assert.Equal(1, _onUseInvoked);

    public void VerifyOnUseInvokedNever() => Assert.Equal(0, _onUseInvoked);

    public void VerifyOnEndUseInvokedOnce() => Assert.Equal(1, _onEndUseInvoked);

    public void InvokeEnded() => Ended();

    public void Deactivate() => IsDeactivated = true;

    public void Activate() => IsDeactivated = false;
}