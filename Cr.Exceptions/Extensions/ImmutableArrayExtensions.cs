using System.Runtime.CompilerServices;

namespace Cr.Exceptions;

public static class ImmutableArrayExtensions
{
    extension<TSource>(ImmutableArray<TSource> source)
    {
        public void ThrowIfDefault([CallerArgumentExpression(nameof(source))] string? paramName = null)
        {
            if (source.IsDefault)
            {
                throw new ArgumentException("The array cannot be default.", paramName);
            }
        }

        public void ThrowIfContainsNull([CallerArgumentExpression(nameof(source))] string? paramName = null)
        {
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