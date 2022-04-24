using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace WellKnownJwtTests.TestHelpers;

public class XUnitAspNetLogger : ILogger, ILoggerProvider
{
    private readonly ITestOutputHelper _testOutputHelper;

    public XUnitAspNetLogger(ITestOutputHelper testOutputHelper)
    {
        _testOutputHelper = testOutputHelper;
    }

    public IDisposable BeginScope<TState>(TState state)
    {
        return Disposable.Create();
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        _testOutputHelper.WriteLine(formatter(state, exception));
    }

    public void Dispose()
    {
    }

    public ILogger CreateLogger(string categoryName)
    {
        return this;
    }
}