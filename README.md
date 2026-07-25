# Intro

A lightweight library for defining application exceptions and automatically converting them into HTTP responses in ASP.NET Core.

## Installation

Register the exception handler during application startup.

```csharp
builder.Services.AddCrExceptionHandler();
```

Or configure custom exception mappings.

```csharp
builder.Services.AddCrExceptionHandler(options =>
{
    options.AddDefaultMappings();
    options.Map<MyCustomException>(499);
});
```

Enable exception handling middleware.

```csharp
app.UseExceptionHandler();
```

## Creating a Custom Exception

Create your own exception by inheriting from one of the provided exception categories.

Example:

```csharp
public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(Guid userId)
        : base(
        [
            new CrError(
                "UserNotFound",
                $"User '{userId}' was not found.")
        ],
        "User was not found.")
    {
    }
}
```

Each `CrException` contains one or more `CrError` objects.

```csharp
public sealed record class CrError(string Code, string Message);
```
Each error provides:

- `Code` — unique identifier used by clients.
- `Message` — human-readable error description.

## Throwing an Exception

Throw your custom exception normally.

```csharp
throw new UserNotFoundException(userId);
```

The exception handler automatically converts it into an RFC 7807 `ProblemDetails` response.

Example:

```json
{
  "type": "about:blank",
  "title": "Not Found",
  "status": 404,
  "detail": "User was not found.",
  "instance": "/api/users/123",
  "traceId": "0HNNAF6ABMHQO",
  "errors": [
    {
      "code": "UserNotFound",
      "message": "User '123' was not found."
    }
  ]
}
```

## Default Exception Mappings

| Exception Category | HTTP Status |
|--------------------|------------:|
| `ValidationException` | 400 Bad Request |
| `UnauthorizedException` | 401 Unauthorized |
| `ForbiddenException` | 403 Forbidden |
| `NotFoundException` | 404 Not Found |
| `ConflictException` | 409 Conflict |
| `UnprocessableException` | 422 Unprocessable Entity |

## Internal Errors

Unexpected exceptions are automatically converted into a generic internal error response.

Example:

```json
{
  "type": "about:blank",
  "title": "Internal Server Error",
  "status": 500,
  "detail": "An unexpected error occurred.",
  "instance": "",
  "traceId": "0HNNAF6ABMHQO",
  "errors": [
    {
      "code": "InternalError",
      "message": "An unexpected internal error occurred."
    }
  ]
}
```
