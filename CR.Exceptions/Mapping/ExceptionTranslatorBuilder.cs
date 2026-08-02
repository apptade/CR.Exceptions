namespace CR.Exceptions.Mapping;

public class ExceptionTranslatorBuilder : MapBuilder<Type, Func<CrException>>
{
    public ExceptionTranslatorBuilder() : base() { }

    public ExceptionTranslatorBuilder(int startCapacity) : base(startCapacity) { }

    public ExceptionTranslatorBuilder Map<TException>(Func<CrException> translator) where TException : CrException
    {
        AddPair(typeof(TException), translator);
        return this;
    }

    public ExceptionTranslator Build()
        => new(BuildFrozenDictionary());
}