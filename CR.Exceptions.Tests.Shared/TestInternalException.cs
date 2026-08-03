using System.Collections.Immutable;

namespace CR.Exceptions.Tests.Shared;

public sealed class TestInternalException : InternalException
{
    private static readonly ImmutableArray<CrError> _errors = [new("TestInternalCode", "TestInternalMessage")];

    public TestInternalException() : base(_errors) { }
}