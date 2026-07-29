using System.Collections.Immutable;

namespace CR.Exceptions;

public abstract class CrException : Exception
{
    public ImmutableArray<CrError> Errors { get; }

    protected CrException(ImmutableArray<CrError> errors, string message, Exception? innerException = null) : base(message, innerException)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        if (errors.IsDefaultOrEmpty)
        {
            throw new ArgumentException("At least one error must be provided.", nameof(errors));
        }

        for (var i = 0; i < errors.Length; i++)
        {
            var error = errors[i];

            ArgumentNullException.ThrowIfNull(error);

            if (string.IsNullOrWhiteSpace(error.Code))
                throw new ArgumentException($"errors[{i}].Code cannot be null or whitespace.", nameof(errors));

            if (string.IsNullOrWhiteSpace(error.Message))
                throw new ArgumentException($"errors[{i}].Message cannot be null or whitespace.", nameof(errors));
        }

        Errors = errors;
    }
}