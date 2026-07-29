using System.Collections.Frozen;

namespace CR.Exceptions.Catalog;

public sealed class ExceptionErrorCatalog
{
    private readonly FrozenDictionary<string, ExceptionErrorDefinition> _definitionMap;

    internal ExceptionErrorCatalog(FrozenDictionary<string, ExceptionErrorDefinition> definitionMap)
    {
        _definitionMap = definitionMap;
    }

    public IReadOnlyCollection<CrError> ResolveErrors(string code)
    {
        return GetDefinition(code).Errors;
    }

    public CrException ResolveException(string code)
    {
        var definition = GetDefinition(code);

        if (definition.ExceptionFactory is null)
        {
            throw new InvalidOperationException(
                $"Error with code '{code}' does not have exception factory.");
        }

        return definition.ExceptionFactory(definition.Errors);
    }

    private ExceptionErrorDefinition GetDefinition(string code)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(code);

        if (!_definitionMap.TryGetValue(code, out var definition))
        {
            throw new InvalidOperationException(
                $"Error with code '{code}' is not added.");
        }

        return definition;
    }
}