using Strawhenge.Interactions.Furniture;

namespace Strawhenge.Interactions.Tests;

public class FurnitureTests
{
    readonly UserContext _userContext;
    readonly FurnitureUser<UserContext> _user;
    readonly Chair _chair;

    public FurnitureTests()
    {
        _userContext = new();
        _user = new(_userContext);
        _chair = new();
    }

    [Fact]
    public void Furniture_user_should_be_using_furniture()
    {
        _user.Use(_chair);
        _user.CurrentFurniture.VerifyIsSome(_chair);
    }

    [Fact]
    public void Furniture_should_have_user_assigned()
    {
        _user.Use(_chair);
        _chair.CurrentUser.VerifyIsSome(_user);
    }

    [Fact]
    public void Furniture_should_receive_user_context()
    {
        _user.Use(_chair);
        _chair.VerifyUserContextReceived(_userContext);
    }

    [Fact]
    public void Furniture_should_invoke_on_use_when_used()
    {
        _user.Use(_chair);
        _chair.VerifyOnUseInvoked();
    }

    [Fact]
    public void Furniture_should_invoke_on_end_use_when_user_ends_use()
    {
        _user.Use(_chair);
        _user.EndUse();

        _chair.VerifyOnEndUseInvoked();
    }

    [Fact]
    public void Furniture_user_should_not_be_using_furniture_when_user_ends_use_and_furniture_invokes_ended()
    {
        _user.Use(_chair);
        _user.EndUse();
        _user.CurrentFurniture.VerifyIsSome(_chair);

        _chair.InvokeEnded();
        _user.CurrentFurniture.VerifyIsNone();
    }

    [Fact]
    public void Furniture_should_not_have_user_assigned_when_user_ends_use_and_furniture_invokes_ended()
    {
        _user.Use(_chair);
        _user.EndUse();
        _chair.CurrentUser.VerifyIsSome(_user);

        _chair.InvokeEnded();
        _chair.CurrentUser.VerifyIsNone();
    }
}

class Chair : Furniture<UserContext>
{
    bool _onUseInvoked;
    bool _onEndUseInvoked;

    protected override void OnUse() => _onUseInvoked = true;

    protected override void OnEndUse() => _onEndUseInvoked = true;

    public void VerifyUserContextReceived(UserContext expectedUserContext) =>
        UserContext.VerifyIsSome(expectedUserContext);

    public void VerifyOnUseInvoked() => Assert.True(_onUseInvoked);

    public void VerifyOnEndUseInvoked() => Assert.True(_onEndUseInvoked);

    public void InvokeEnded() => Ended();
}

class UserContext
{
}