using System.Collections.Frozen;
using System.Collections.Immutable;

namespace CR.Exceptions.Map;

public sealed class ExceptionErrorMap
{
    private readonly FrozenDictionary<string, ExceptionErrorDescriptor> _descriptorMap;

    public ExceptionErrorMap(FrozenDictionary<string, ExceptionErrorDescriptor> descriptorMap)
    {
        _descriptorMap = descriptorMap;
    }

    public ImmutableArray<CrError> GetErrors(string code)
    {
        return GetDescriptor(code).Errors;
    }

    public CrException CreateException(string code)
    {
        var definition = GetDescriptor(code);

        if (definition.ExceptionFactory is null)
        {
            throw new InvalidOperationException(
                $"Error with code '{code}' does not have exception factory.");
        }

        return definition.ExceptionFactory(definition.Errors);
    }

    public bool TryGetErrors(string code, out ImmutableArray<CrError> errors)
    {
        if (_descriptorMap.TryGetValue(code, out var descriptor))
        {
            errors = descriptor.Errors;
            return true;
        }

        errors = default;
        return false;
    }

    public bool TryCreateException(string code, out CrException? exception)
    {
        if (_descriptorMap.TryGetValue(code, out var descriptor))
        {
            if (descriptor.ExceptionFactory is not null)
            {
                exception = descriptor.ExceptionFactory(descriptor.Errors);
                return true;
            }
        }

        exception = null;
        return false;
    }

    private ExceptionErrorDescriptor GetDescriptor(string code)
    {
        if (_descriptorMap.TryGetValue(code, out var descriptor))
        {
            return descriptor;
        }

        throw new KeyNotFoundException($"Error with code '{code}' is not added.");
    }
}