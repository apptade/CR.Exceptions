namespace CR.Exceptions.Extensions;

internal static class EnumerableExtensions
{
    extension<TSource>(IEnumerable<TSource>? source) where TSource : class?
    {
        public void ThrowIfEmptyOrContainsNull()
        {
            ArgumentNullException.ThrowIfNull(source);

            var index = 0;
            var isEmpty = true;

            foreach (var item in source)
            {
                isEmpty = false;

                if (item is null)
                {
                    throw new ArgumentNullException(nameof(source), $"The collection element[{index}] is null.");
                }

                index++;
            }

            if (isEmpty)
            {
                throw new ArgumentException("The collection cannot be empty.", nameof(source));
            }
        }
    }
}