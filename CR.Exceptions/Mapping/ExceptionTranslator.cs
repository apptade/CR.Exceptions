using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions.Mapping;

public class ExceptionTranslator : TypeMap<Func<CrException>>
{
    internal ExceptionTranslator(FrozenDictionary<Type, Func<CrException>> dictionary) : base(dictionary) { }

    public CrException Translate(CrException exception)
        => TranslatorToException(SearchValue(exception.GetType()));

    public bool TryTranslate(CrException exception, [MaybeNullWhen(false)] out CrException translated)
        => (translated = TrySearchValue(exception.GetType(), out var factory) ? TranslatorToException(factory) : null) != null;

    private static CrException TranslatorToException(Func<CrException> translator)
    {
        return translator() ?? throw new NullReferenceException($"The registered {nameof(translator)} - return null exception");
    }
}