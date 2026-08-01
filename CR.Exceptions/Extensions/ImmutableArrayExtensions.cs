using System.Collections.Immutable;

namespace CR.Exceptions.Extensions;

public static class ImmutableArrayExtensions
{
    extension<TSource>(ImmutableArray<TSource> source) where TSource : class?
    {
        public void ThrowIfEmptyOrContainsNull()
        {
            if (source.IsDefaultOrEmpty)
            {
                throw new ArgumentException("The array cannot be empty.", nameof(source));
            }

            for (var i = 0; i < source.Length; i++)
            {
                if (source[i] is null)
                {
                    throw new ArgumentNullException(nameof(source), $"The array element[{i}] is null.");
                }
            }
        }
    }
}