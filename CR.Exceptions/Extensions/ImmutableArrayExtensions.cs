using System.Collections.Immutable;
using System.Runtime.CompilerServices;

namespace CR.Exceptions.Extensions;

public static class ImmutableArrayExtensions
{
    extension<TSource>(ImmutableArray<TSource> source)
    {
        public void ThrowIfEmptyOrContainsNull([CallerArgumentExpression(nameof(source))] string? paramName = null)
        {
            if (source.IsDefaultOrEmpty)
            {
                throw new ArgumentException("The array cannot be empty.", paramName);
            }

            for (var i = 0; i < source.Length; i++)
            {
                if (source[i] is null)
                {
                    throw new ArgumentNullException(paramName, $"The array element[{i}] is null.");
                }
            }
        }
    }
}