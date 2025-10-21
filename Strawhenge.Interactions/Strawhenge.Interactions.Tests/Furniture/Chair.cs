using Strawhenge.Interactions.Furniture;

namespace Strawhenge.Interactions.Tests;

class Chair : Furniture<UserContext>
{
    int _onUseInvoked;
    int _onEndUseInvoked;

    public override string Name => nameof(Chair);

    protected override void OnUse() => _onUseInvoked++;

    protected override void OnEndUse() => _onEndUseInvoked++;

    public void VerifyUserContextReceived(UserContext expectedUserContext) =>
        UserContext.VerifyIsSome(expectedUserContext);

    public void VerifyOnUseInvokedOnce() => Assert.Equal(1, _onUseInvoked);

    public void VerifyOnUseInvokedNever() => Assert.Equal(0, _onUseInvoked);

    public void VerifyOnEndUseInvokedOnce() => Assert.Equal(1, _onEndUseInvoked);

    public void InvokeEnded() => Ended();

    public void Deactivate() => IsDeactivated = true;
}