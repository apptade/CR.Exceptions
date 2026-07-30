using System.Collections.Immutable;

namespace CR.Exceptions.UnitTests;

internal sealed class TestException : CrException
{
    public TestException(ImmutableArray<CrError> errors) : base(errors, "Test exception message")
    {
    }
}