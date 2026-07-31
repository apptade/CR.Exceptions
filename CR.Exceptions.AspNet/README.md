# Intro

ASP.NET Core integration for **CrCore.Exceptions**.

This package provides automatic handling of `CrException` instances, converts them into RFC 7807 `ProblemDetails` responses, and supports configurable exception-to-HTTP status code and log level mappings.

## Features

- ASP.NET Core exception handler
- RFC 7807 `ProblemDetails` responses
- Configurable exception → HTTP status code mapping
- Configurable exception → log level mapping
- Generic handling of unexpected exceptions

---

# Installation

```bash
dotnet add package CrCore.Exceptions.AspNet
```

Register the default exception handling during application startup.

```csharp
builder.Services.AddCrExceptions();

app.UseExceptionHandler();
```

---

# Default HTTP Status Code Mapping

| Exception | HTTP Status |
|-----------|------------:|
| `ValidationException` | 400 |
| `UnauthorizedException` | 401 |
| `ForbiddenException` | 403 |
| `NotFoundException` | 404 |
| `ConflictException` | 409 |
| `UnprocessableException` | 422 |
| `InternalException` | 500 |

# Custom HTTP Status Code Mapping

```csharp
builder.Services.AddCrStatusCodeMapping(builder =>
{
    builder.AddDefaultMappings();

    builder.Map<MyCustomException>(StatusCodes.Status499ClientClosedRequest);
});
```

---

# Default Log Level Mapping

| Exception | Log Level |
|-----------|------------:|
| `InternalException` | Error |

# Custom Log Level Mapping

```csharp
builder.Services.AddCrLogLevelMapping(builder =>
{
    builder.AddDefaultMappings();

    builder.Map<MyCustomException>(LogLevel.Warning);
});
```

---

# ProblemDetails Response

`CrException` instances are automatically converted into RFC 7807 `ProblemDetails`.

Example:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Not Found",
  "status": 404,
  "detail": "The requested resource was not found.",
  "instance": "/api/users/1",
  "errors": [
    {
      "code": "Identity.UserNotFound",
      "message": "User was not found."
    }
  ],
  "traceId": "..."
}
```

Clients should use `errors[].code` as the stable application error identifier.

---

# Unexpected Exceptions

Exceptions that do not inherit from `CrException` are converted into a generic internal server error response.

Example:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.6.1",
  "title": "An error occurred while processing your request.",
  "status": 500,
  "detail": "An unexpected error occurred.",
  "instance": "/api/users/1",
  "errors": [
    {
      "code": "InternalError",
      "message": "An unexpected internal error occurred."
    }
  ],
  "traceId": "..."
}
```