using CR.Exceptions.Extensions;
using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions.Mapping;

public class ExceptionTranslator : TypeMap<Func<CrException>>
{
    internal ExceptionTranslator(FrozenDictionary<Type, Func<CrException>> dictionary) : base(dictionary) { }

    public CrException Translate(CrException exception)
        => SearchValue(exception.GetType()).ToResult();

    public bool TryTranslate(CrException exception, [MaybeNullWhen(false)] out CrException translated)
        => (translated = TrySearchValue(exception.GetType(), out var translator) ? translator.ToResult() : null) != null;
}