namespace CR.Exceptions.Mapping;

public class ExceptionTranslatorBuilder : MapBuilder<Type, Func<CrException>>
{
    public ExceptionTranslatorBuilder Map<TException>(Func<CrException> translator) where TException : CrException
    {
        AddPair(typeof(TException), translator);
        return this;
    }

    public ExceptionTranslator Build()
        => new(BuildFrozenDictionary());
}