namespace CR.Exceptions.Mapping;

public class ExceptionFactoryBuilder : MapBuilder<string, Func<CrException>>
{
    public ExceptionFactoryBuilder() : base() { }

    public ExceptionFactoryBuilder(int startCapacity) : base(startCapacity) { }

    public ExceptionFactoryBuilder Map(string code, Func<CrException> factory)
    {
        AddPair(code, factory);
        return this;
    }

    public ExceptionFactory Build()
        => new(BuildFrozenDictionary(comparer: StringComparer.Ordinal));
}