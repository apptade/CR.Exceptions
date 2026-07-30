using System.Collections.Frozen;

namespace CR.Exceptions.Extensions;

internal static class FrozenDictionaryExtensions
{
    extension<TSource>(IEnumerable<TSource> source)
    {
        public FrozenDictionary<TKey, TElement> ToUniqueFrozenDictionary<TKey, TElement>(
            Func<TSource, TKey> keySelector,
            Func<TSource, TElement> elementSelector,
            IEqualityComparer<TKey>? comparer = null) where TKey : notnull
        {
            ArgumentNullException.ThrowIfNull(source);
            ArgumentNullException.ThrowIfNull(keySelector);
            ArgumentNullException.ThrowIfNull(elementSelector);

            try
            {
                var validatedDictionary = source.ToDictionary(keySelector, elementSelector, comparer);
                return validatedDictionary.ToFrozenDictionary(comparer);
            }
            catch (ArgumentException ex)
            {
                throw new InvalidOperationException("Initialization failed: sequence contains duplicate keys.", ex);
            }
        }
    }
}