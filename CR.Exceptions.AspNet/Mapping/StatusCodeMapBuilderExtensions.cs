using Microsoft.AspNetCore.Http;

namespace CR.Exceptions.AspNet.Mapping;

public static class StatusCodeMapBuilderExtensions
{
    extension(StatusCodeMapBuilder builder)
    {
        public StatusCodeMapBuilder AddDefaultMapping()
        {
            builder
                .Map<ValidationException>(StatusCodes.Status400BadRequest)
                .Map<UnauthorizedException>(StatusCodes.Status401Unauthorized)
                .Map<ForbiddenException>(StatusCodes.Status403Forbidden)
                .Map<NotFoundException>(StatusCodes.Status404NotFound)
                .Map<ConflictException>(StatusCodes.Status409Conflict)
                .Map<UnprocessableException>(StatusCodes.Status422UnprocessableEntity)
                .Map<InternalException>(StatusCodes.Status500InternalServerError);

            return builder;
        }
    }
}