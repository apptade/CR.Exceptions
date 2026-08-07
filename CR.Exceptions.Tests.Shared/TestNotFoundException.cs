using System.Collections.Immutable;

namespace CR.Exceptions.Tests.Shared;

public sealed class TestNotFoundException : NotFoundException
{
    private static readonly ImmutableArray<CrError> _errors = [new("TestNotFoundCode", "Test not found message")];

    public TestNotFoundException(Exception? innerException = null) : base(_errors, innerException) { }
}