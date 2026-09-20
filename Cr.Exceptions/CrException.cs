namespace Cr.Exceptions;

public class CrException : Exception
{
    public ImmutableArray<CrError> Errors { get; }

    public CrException(string? message = null, Exception? innerException = null) : base(message, innerException)
    {
        Errors = [];
    }

    public CrException(ImmutableArray<CrError> errors, string? message = null, Exception? innerException = null) : base(message, innerException)
    {
        errors.ThrowIfEmptyOrContainsNull();

        Errors = errors;
    }
}