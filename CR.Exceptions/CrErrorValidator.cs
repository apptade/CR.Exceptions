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
            if (errors[i] is null)
                throw new ArgumentNullException(nameof(errors), $"errors[{i}] is null.");
        }
    }
}