using System.Collections.Immutable;

namespace CR.Exceptions;

internal static class CrErrorValidator
{
    public static void ThrowExceptionIfInvalid(ImmutableArray<CrError> errors)
    {
        if (errors.IsDefaultOrEmpty)
        {
            throw new ArgumentException("At least one error must be provided.", nameof(errors));
        }

        for (var i = 0; i < errors.Length; i++)
        {
            var error = errors[i] ?? throw new ArgumentNullException(nameof(errors), $"errors[{i}] is null.");

            if (string.IsNullOrWhiteSpace(error.Code))
                throw new ArgumentException($"errors[{i}].Code cannot be null or whitespace.", nameof(errors));

            if (string.IsNullOrWhiteSpace(error.Message))
                throw new ArgumentException($"errors[{i}].Message cannot be null or whitespace.", nameof(errors));
        }
    }
}