using System.Collections.Frozen;

namespace CR.Exceptions.Catalog;

public sealed class ExceptionErrorCatalogBuilder
{
    private readonly Dictionary<string, ExceptionErrorDefinition> _definitionMap;

    public ExceptionErrorCatalogBuilder()
    {
        _definitionMap = new(StringComparer.Ordinal);
    }

    public ExceptionErrorCatalogBuilder Add(string code, params CrError[] errors)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentNullException.ThrowIfNull(errors);

        _definitionMap.Add(code, new ExceptionErrorDefinition(errors));

        return this;
    }

    public ExceptionErrorCatalogBuilder Add(string code, Func<CrError[], CrException> factory, params CrError[] errors)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);
        ArgumentNullException.ThrowIfNull(factory);
        ArgumentNullException.ThrowIfNull(errors);

        _definitionMap.Add(code, new ExceptionErrorDefinition(errors, factory));

        return this;
    }

    internal ExceptionErrorCatalog Build()
    {
        return new ExceptionErrorCatalog(_definitionMap.ToFrozenDictionary(StringComparer.Ordinal));
    }
}