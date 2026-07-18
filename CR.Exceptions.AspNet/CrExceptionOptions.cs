using System.Net;

namespace CR.Exceptions.AspNet;

public sealed class CrExceptionOptions
{
    private readonly Dictionary<Type, HttpStatusCode> _mappings = [];
    public IReadOnlyDictionary<Type, HttpStatusCode> Mappings => _mappings;

    public CrExceptionOptions AddDefaultMappings()
    {
        Map<ValidationException>(HttpStatusCode.BadRequest);
        Map<UnauthorizedException>(HttpStatusCode.Unauthorized);
        Map<ForbiddenException>(HttpStatusCode.Forbidden);
        Map<NotFoundException>(HttpStatusCode.NotFound);
        Map<ConflictException>(HttpStatusCode.Conflict);
        Map<UnprocessableException>(HttpStatusCode.UnprocessableEntity);

        return this;
    }

    public CrExceptionOptions Map<TType>(HttpStatusCode statusCode) where TType : CrException
    {
        if (!_mappings.TryAdd(typeof(TType), statusCode))
        {
            throw new InvalidOperationException(
                $"Mapping for {typeof(TType).Name} already exists");
        }

        return this;
    }

    public bool TryGetStatusCode(Exception exception, out HttpStatusCode statusCode)
    {
        foreach (var mapping in _mappings)
        {
            if (mapping.Key.IsAssignableFrom(exception.GetType()))
            {
                statusCode = mapping.Value;
                return true;
            }
        }

        statusCode = default;
        return false;
    }
}