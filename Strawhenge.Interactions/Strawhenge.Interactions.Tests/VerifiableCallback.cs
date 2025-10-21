namespace Strawhenge.Interactions.Tests;

class VerifiableCallback
{
    public static implicit operator Action(VerifiableCallback callback) => callback.OnInvoked;

    int _invokedCount;

    public void VerifyInvokedOnce() => Assert.Equal(1, _invokedCount);

    void OnInvoked() => _invokedCount++;
}