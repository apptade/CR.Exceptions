using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions.Mapping;

public sealed class ExceptionFactory : Map<string, ExceptionRegistration>
{
    internal ExceptionFactory(FrozenDictionary<string, ExceptionRegistration> dictionary) : base(dictionary) { }

    public CrException Create(string code)
        => TransformValueToResult(GetValue(code));

    public bool TryCreate(string code, [MaybeNullWhen(false)] out CrException exception)
        => (exception = TryGetValue(code, out var value) ? TransformValueToResult(value) : null) != null;

    private static CrException TransformValueToResult(ExceptionRegistration value)
    {
        return value.Factory(value.Definition.Errors)
            ?? throw new NullReferenceException("The registered factory return null exception");
    }
}