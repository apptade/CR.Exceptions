using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions;

public class ExceptionTranslator : TypeMap<Func<CrException>>
{
    internal ExceptionTranslator(FrozenDictionary<Type, Func<CrException>> dictionary) : base(dictionary) { }

    public CrException Translate(CrException exception)
        => GetByHierarchy(exception.GetType()).ToResult();

    public bool TryTranslate(CrException exception, [MaybeNullWhen(false)] out CrException translated)
        => (translated = TryGetByHierarchy(exception.GetType(), out var translator) ? translator.ToResult() : null) != null;
}