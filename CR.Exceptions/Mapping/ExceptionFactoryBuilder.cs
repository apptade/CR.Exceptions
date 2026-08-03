namespace CR.Exceptions.Mapping;

public class ExceptionFactoryBuilder : MapBuilder<string, Func<CrException>>
{
    public ExceptionFactoryBuilder Map(string code, Func<CrException> factory)
    {
        ThrowIfInvalidCode(code);
        AddPair(code, factory);

        return this;
    }

    public ExceptionFactory Build()
        => new(BuildFrozenDictionary(comparer: StringComparer.Ordinal));

    private static void ThrowIfInvalidCode(string code)
        => ArgumentException.ThrowIfNullOrEmpty(code);
}