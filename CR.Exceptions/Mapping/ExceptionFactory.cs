using CR.Exceptions.Extensions;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions.Mapping;

public class ExceptionFactory : Map<string, Func<CrException>>
{
    internal ExceptionFactory(FrozenDictionary<string, Func<CrException>> dictionary) : base(dictionary) { }

    public CrException Create(string code)
        => GetValue(code).ToResult();

    public bool TryCreate(string code, [MaybeNullWhen(false)] out CrException exception)
        => (exception = TryGetValue(code, out var factory) ? factory.ToResult() : null) != null;
}