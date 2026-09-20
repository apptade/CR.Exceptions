using Microsoft.Extensions.Logging;

namespace Cr.Exceptions.AspNetCore;

public static class LogLevelMapBuilderExtensions
{
    extension(LogLevelMapBuilder builder)
    {
        public LogLevelMapBuilder AddDefaultMappings()
        {
            return builder
                .Map<ValidationException>(LogLevel.Debug)
                .Map<UnauthorizedException>(LogLevel.Debug)
                .Map<ForbiddenException>(LogLevel.Debug)
                .Map<NotFoundException>(LogLevel.Debug)
                .Map<ConflictException>(LogLevel.Debug)
                .Map<UnprocessableException>(LogLevel.Debug)
                .Map<InternalException>(LogLevel.Error);
        }
    }
}