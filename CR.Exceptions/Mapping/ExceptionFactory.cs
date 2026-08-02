using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions.Mapping;

public class ExceptionFactory : Map<string, Func<CrException>>
{
    internal ExceptionFactory(FrozenDictionary<string, Func<CrException>> dictionary) : base(dictionary) { }

    public CrException Create(string code)
        => FactoryToException(GetValue(code));

    public bool TryCreate(string code, [MaybeNullWhen(false)] out CrException exception)
        => (exception = TryGetValue(code, out var value) ? FactoryToException(value) : null) != null;

    private static CrException FactoryToException(Func<CrException> factory)
    {
        return factory() ?? throw new NullReferenceException($"The registered {nameof(factory)} - return null exception");
    }
}