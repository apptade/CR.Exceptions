using System.Net;

namespace CR.Exceptions.AspNet.Mapping;

public sealed class StatusCodeMapBuilder : TypeMapBuilder<int>
{
    public StatusCodeMap Build()
    {
        return new(BuildFrozenDictionary());
    }

    protected override void ThrowIfInvalidValue(int value)
    {
        if (!Enum.IsDefined(typeof(HttpStatusCode), value))
        {
            throw new ArgumentOutOfRangeException(
                nameof(value), $"'{value}' is not a standard HTTP status code.");
        }
    }
}