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

```csharp
builder.Services.AddCrExceptionsCore();

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
| `ValidationException` | Debug |
| `UnauthorizedException` | Debug |
| `ForbiddenException` | Debug |
| `NotFoundException` | Debug |
| `ConflictException` | Debug |
| `UnprocessableException` | Debug |
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
  "type": "https://tools.ietf.org/html/rfc9110#section-15.6.1",
  "title": "An error occurred while processing your request.",
  "status": 500,
  "detail": "An unexpected internal error occurred.",
  "instance": "/api/test",
  "errors": [
    {
      "code": "TestInternalCode",
      "message": "TestInternalMessage"
    }
  ],
  "traceId": "1ca274bed877413cefd8094fc63bd559"
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
  "instance": "/api/test",
  "errors": [
    {
      "code": "InternalError",
      "message": "An unexpected internal error occurred."
    }
  ],
  "traceId": "b217277ea131750f161bc6e8d8b33302"
}
```