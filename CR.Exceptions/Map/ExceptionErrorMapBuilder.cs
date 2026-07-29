using System.Collections.Frozen;

namespace CR.Exceptions.Map;

public sealed class ExceptionErrorMapBuilder
{
    private readonly List<ExceptionErrorDescriptor> _descriptors = [];

    public ExceptionErrorMapBuilder Add(ExceptionErrorDescriptor descriptor)
    {
        ArgumentNullException.ThrowIfNull(descriptor);
        _descriptors.Add(descriptor);

        return this;
    }

    public ExceptionErrorMapBuilder AddParams(params ExceptionErrorDescriptor[] descriptors)
    {
        return AddRange(descriptors);
    }

    public ExceptionErrorMapBuilder AddRange(IEnumerable<ExceptionErrorDescriptor> descriptors)
    {
        ArgumentNullException.ThrowIfNull(descriptors);
        _descriptors.AddRange(descriptors);

        return this;
    }

    public ExceptionErrorMap Build()
    {
        return new ExceptionErrorMap(_descriptors.ToFrozenDictionary(keySelector: k => k.Code, comparer: StringComparer.Ordinal));
    }
}