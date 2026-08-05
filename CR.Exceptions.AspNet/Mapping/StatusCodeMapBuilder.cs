using System.Net;

namespace CR.Exceptions.AspNet;

public class StatusCodeMapBuilder : MapBuilder<Type, int>
{
    public StatusCodeMapBuilder Map<TException>(int code) where TException : CrException
    {
        ThrowIfInvalidCode(code);
        AddPair(typeof(TException), code);

        return this;
    }

    public StatusCodeMap Build()
        => new(BuildFrozenDictionary());

    private static void ThrowIfInvalidCode(int code)
    {
        if (!Enum.IsDefined(typeof(HttpStatusCode), code))
        {
            throw new ArgumentOutOfRangeException(
                nameof(code), $"'{code}' is not a standard HTTP status code.");
        }
    }
}