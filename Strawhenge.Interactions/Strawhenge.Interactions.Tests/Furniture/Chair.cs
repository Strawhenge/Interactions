using Strawhenge.Interactions.Furniture;

namespace Strawhenge.Interactions.Tests;

class Chair : Furniture<UserContext>
{
    bool _onUseInvoked;
    bool _onEndUseInvoked;

    public override string Name => nameof(Chair);

    protected override void OnUse() => _onUseInvoked = true;

    protected override void OnEndUse() => _onEndUseInvoked = true;

    public void VerifyUserContextReceived(UserContext expectedUserContext) =>
        UserContext.VerifyIsSome(expectedUserContext);

    public void VerifyOnUseInvoked() => Assert.True(_onUseInvoked);

    public void VerifyOnEndUseInvoked() => Assert.True(_onEndUseInvoked);

    public void InvokeEnded() => Ended();
}