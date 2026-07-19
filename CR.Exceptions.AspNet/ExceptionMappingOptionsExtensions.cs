using Microsoft.AspNetCore.Http;

namespace CR.Exceptions.AspNet;

public static class ExceptionMappingOptionsExtensions
{
    extension(ExceptionMappingOptions options)
    {
        public ExceptionMappingOptions AddDefaultMappings()
        {
            return options
                .Map<ValidationException>(StatusCodes.Status400BadRequest)
                .Map<UnauthorizedException>(StatusCodes.Status401Unauthorized)
                .Map<ForbiddenException>(StatusCodes.Status403Forbidden)
                .Map<NotFoundException>(StatusCodes.Status404NotFound)
                .Map<ConflictException>(StatusCodes.Status409Conflict)
                .Map<UnprocessableException>(StatusCodes.Status422UnprocessableEntity);
        }
    }
}