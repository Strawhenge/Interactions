using Strawhenge.Common.Logging;
using Xunit.Abstractions;

namespace Strawhenge.Interactions.Tests;

public class TestOutputLogger : ILogger
{
    readonly ITestOutputHelper _testOutput;

    public TestOutputLogger(ITestOutputHelper testOutput)
    {
        _testOutput = testOutput;
    }

    public void LogInformation(string message) => _testOutput.WriteLine($"[Information] {message}");

    public void LogWarning(string message) => _testOutput.WriteLine($"[Warning] {message}");

    public void LogError(string message) => _testOutput.WriteLine($"[Error] {message}");

    public void LogException(Exception exception) => _testOutput.WriteLine($"[Exception] {exception}");
}