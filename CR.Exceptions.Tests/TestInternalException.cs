using System.Collections.Immutable;

namespace CR.Exceptions.Tests;

public sealed class TestInternalException : InternalException
{
    private static readonly ImmutableArray<CrError> _errors = [new("TestInternalCode", "TestInternalMessage")];

    public TestInternalException() : base(_errors) { }
}