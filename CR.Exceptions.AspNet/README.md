# CrCore.Exceptions.AspNet

ASP.NET Core integration for **CrCore.Exceptions**.

This package converts `CrException` instances into RFC 7807 `ProblemDetails` responses and provides configurable exception-to-HTTP status code mapping.

## Features

- ASP.NET Core exception handler
- RFC 7807 `ProblemDetails` responses
- Configurable exception → HTTP status code mapping
- Automatic serialization of `CrException`
- Generic handling of unexpected exceptions

---

# Installation

```bash
dotnet add package CrCore.Exceptions.AspNet
```

Register the exception handler during application startup.

```csharp
builder.Services.AddCrExceptionHandler();

app.UseExceptionHandler();
```

---

# Default Status Code Mapping

The package provides default mappings for the built-in exception categories.

| Exception | HTTP Status |
|-----------|------------:|
| `ValidationException` | 400 |
| `UnauthorizedException` | 401 |
| `ForbiddenException` | 403 |
| `NotFoundException` | 404 |
| `ConflictException` | 409 |
| `UnprocessableException` | 422 |
| `CrException` | 500 |

---

# Custom Status Code Mapping

Mappings can be customized during registration.

```csharp
builder.Services.AddCrExceptionHandler(options =>
{
    options.StatusCodes.AddDefaultMappings();

    options.StatusCodes.Map<MyCustomException>(499);
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

---

# Dependency

This package depends on **CrCore.Exceptions** and is intended to be used in ASP.NET Core applications.

For defining application errors, exception categories, `ErrorMap`, and `ExceptionFactory`, see the **CrCore.Exceptions** package.