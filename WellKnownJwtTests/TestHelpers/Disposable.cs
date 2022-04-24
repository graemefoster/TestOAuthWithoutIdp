using System;

namespace WellKnownJwtTests.TestHelpers;

public class Disposable : IDisposable
{
    public void Dispose()
    {
    }

    public static IDisposable Create() => new Disposable();
}