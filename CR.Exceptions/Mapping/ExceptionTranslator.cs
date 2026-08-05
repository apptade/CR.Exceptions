using System.Collections.Frozen;
using System.Diagnostics.CodeAnalysis;

namespace CR.Exceptions;

public class ExceptionTranslator : TypeMap<Func<Exception, CrException>>
{
    internal ExceptionTranslator(FrozenDictionary<Type, Func<Exception, CrException>> dictionary) : base(dictionary) { }

    public CrException Translate(Exception exception)
        => ExecuteTranslator(exception, GetByHierarchy(exception.GetType()));

    public bool TryTranslate(Exception exception, [MaybeNullWhen(false)] out CrException translated)
        => (translated = TryGetByHierarchy(exception.GetType(), out var translator) ? ExecuteTranslator(exception, translator) : null) != null;

    private static CrException ExecuteTranslator(Exception innerException, Func<Exception, CrException> translator)
    {
        return
            translator(innerException) ??
            throw new InvalidOperationException($"{nameof(translator)} '{translator.Method.Name}' returned null.");
    }
}