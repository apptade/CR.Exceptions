using CR.Exceptions.Extensions;
using System.Collections.Immutable;

namespace CR.Exceptions;

public abstract class CrException : Exception
{
    public ImmutableArray<CrError> Errors { get; }

    protected CrException(ImmutableArray<CrError> errors, string message, Exception? innerException = null) : base(message, innerException)
    {
        ArgumentException.ThrowIfNullOrEmpty(message);
        errors.ThrowIfEmptyOrContainsNull();

        Errors = errors;
    }
}