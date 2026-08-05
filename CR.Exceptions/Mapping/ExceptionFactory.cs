using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions;

public class ExceptionFactory : Map<string, Func<CrException>>
{
    internal ExceptionFactory(FrozenDictionary<string, Func<CrException>> dictionary) : base(dictionary) { }

    public CrException Create(string code)
        => ExecuteFactory(GetValue(code));

    public bool TryCreate(string code, [MaybeNullWhen(false)] out CrException exception)
        => (exception = TryGetValue(code, out var factory) ? ExecuteFactory(factory) : null) != null;

    private static CrException ExecuteFactory(Func<CrException> factory)
    {
        return factory() ?? throw new InvalidOperationException($"{nameof(factory)} '{factory.Method.Name}' returned null.");
    }
}