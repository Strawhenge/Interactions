using Strawhenge.Interactions.Furniture;
using Xunit.Abstractions;

namespace Strawhenge.Interactions.Tests;

public class FurnitureTests
{
    readonly Chair _chair;
    readonly UserContext _userContext;
    readonly FurnitureUser<UserContext> _user;

    readonly FurnitureUser<UserContext> _otherUser;
    readonly Chair _otherChair;

    public FurnitureTests(ITestOutputHelper testOutputHelper)
    {
        var logger = new TestOutputLogger(testOutputHelper);

        _chair = new();
        _userContext = new();
        _user = new(_userContext, logger);

        _otherChair = new();
        _otherUser = new(new UserContext(), logger);
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
        _chair.VerifyOnUseInvokedOnce();
    }

    [Fact]
    public void Furniture_should_invoke_on_end_use_when_user_ends_use()
    {
        _user.Use(_chair);
        _user.EndUse();

        _chair.VerifyOnEndUseInvokedOnce();
    }

    [Fact]
    public void Furniture_should_not_invoke_on_end_use_multiple_times()
    {
        _user.Use(_chair);
        _user.EndUse();
        _user.EndUse();

        _chair.VerifyOnEndUseInvokedOnce();
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
    public void Furniture_user_should_not_be_using_furniture_when_furniture_invokes_ended()
    {
        _user.Use(_chair);

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

    [Fact]
    public void Furniture_should_not_have_user_assigned_when_furniture_invokes_ended()
    {
        _user.Use(_chair);

        _chair.InvokeEnded();
        _chair.CurrentUser.VerifyIsNone();
    }

    [Fact]
    public void Use_callback_should_invoke_when_use_ended()
    {
        var callback = new VerifiableCallback();
        _user.Use(_chair, callback);

        _user.EndUse();
        _chair.InvokeEnded();

        callback.VerifyInvokedOnce();
    }

    [Fact]
    public void End_use_callback_should_invoke_when_use_ended()
    {
        _user.Use(_chair);

        var callback = new VerifiableCallback();
        _user.EndUse(callback);
        _chair.InvokeEnded();

        callback.VerifyInvokedOnce();
    }

    [Fact]
    public void Multiple_end_use_callbacks_should_invoke_when_use_ended()
    {
        _user.Use(_chair);

        var callback1 = new VerifiableCallback();
        _user.EndUse(callback1);

        var callback2 = new VerifiableCallback();
        _user.EndUse(callback2);

        _chair.InvokeEnded();

        callback1.VerifyInvokedOnce();
        callback2.VerifyInvokedOnce();
    }

    [Fact]
    public void Furniture_user_should_not_be_using_furniture_when_another_user_is_using_it()
    {
        _user.Use(_chair);
        _otherUser.Use(_chair);

        _otherUser.CurrentFurniture.VerifyIsNone();
        _user.CurrentFurniture.VerifyIsSome(_chair);
    }

    [Fact]
    public void Furniture_should_not_be_assigned_user_when_another_user_is_using_it()
    {
        _user.Use(_chair);
        _otherUser.Use(_chair);

        _chair.CurrentUser.VerifyIsSome(_user);
    }

    [Fact]
    public void Use_callback_should_invoke_when_furniture_is_used_by_another_user()
    {
        _user.Use(_chair);

        var callback = new VerifiableCallback();
        _otherUser.Use(_chair, callback);

        callback.VerifyInvokedOnce();
    }

    [Fact]
    public void Furniture_user_should_not_be_using_furniture_already_using_another_furniture()
    {
        _user.Use(_chair);
        _user.Use(_otherChair);

        _user.CurrentFurniture.VerifyIsSome(_chair);
    }

    [Fact]
    public void Furniture_should_not_be_assigned_user_when_already_using_another_furniture()
    {
        _user.Use(_chair);
        _user.Use(_otherChair);

        _otherChair.CurrentUser.VerifyIsNone();
        _chair.CurrentUser.VerifyIsSome(_user);
    }

    [Fact]
    public void Use_callback_should_invoke_when_already_using_another_furniture()
    {
        _user.Use(_chair);

        var callback = new VerifiableCallback();
        _user.Use(_otherChair, callback);

        callback.VerifyInvokedOnce();
    }

    [Fact]
    public void End_use_callback_should_invoke_when_not_using_furniture()
    {
        var callback = new VerifiableCallback();
        _user.EndUse(callback);

        callback.VerifyInvokedOnce();
    }

    [Fact]
    public void User_should_not_use_furniture_when_deactivated()
    {
        _chair.Deactivate();
        _user.Use(_chair);

        _user.CurrentFurniture.VerifyIsNone();
    }

    [Fact]
    public void Furniture_should_not_have_user_assigned_when_deactivated()
    {
        _chair.Deactivate();
        _user.Use(_chair);

        _chair.CurrentUser.VerifyIsNone();
    }

    [Fact]
    public void Furniture_should_not_invoke_on_use_when_deactivated()
    {
        _chair.Deactivate();
        _user.Use(_chair);

        _chair.VerifyOnUseInvokedNever();
    }

    [Fact]
    public void Furniture_should_invoke_on_end_use_when_deactivated()
    {
        _user.Use(_chair);
        _chair.Deactivate();

        _chair.VerifyOnEndUseInvokedOnce();
    }

    [Fact]
    public void Furniture_should_invoke_event_when_deactivated()
    {
        var callback = new VerifiableCallback();
        _chair.DeactivatedStateChanged += callback;

        _chair.Deactivate();
        callback.VerifyInvokedOnce();
    }
    
    [Fact]
    public void Furniture_should_invoke_event_when_reactivated()
    {
        _chair.Deactivate();
        
        var callback = new VerifiableCallback();
        _chair.DeactivatedStateChanged += callback;

        _chair.Activate();
        callback.VerifyInvokedOnce();
    }
}