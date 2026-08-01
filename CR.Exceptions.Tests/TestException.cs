using System.Collections.Immutable;

namespace CR.Exceptions.Tests;

internal sealed class TestException : CrException
{
    public TestException(ImmutableArray<CrError> errors) : base(errors, "Test exception message")
    {
    }
}