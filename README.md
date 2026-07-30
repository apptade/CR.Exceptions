# Intro

A lightweight framework for defining application errors, creating typed exceptions, and exposing consistent error responses across application boundaries.

CR.Exceptions separates:

- error definition (`CrError`);
- application exceptions (`CrException`);
- external error mapping (`ErrorMap`);
- exception creation (`ExceptionFactory`);
- HTTP response representation (ASP.NET Core integration).

The goal is to keep external service errors (for example Keycloak, GitHub, payment providers) isolated from application logic while providing a consistent error contract for clients.

---

## Installation

Register the exception handler during application startup:

```csharp
builder.Services.AddCrExceptionHandler();

app.UseExceptionHandler();
````

Custom mappings can be configured:

```csharp
builder.Services.AddCrExceptionHandler(options =>
{
    options.StatusCodes.AddDefaultMappings();
    options.StatusCodes.Map<MyCustomException>(499);
});
```

---

# Error Model

Every application error is represented by `CrError`.

Each error provides:

* `Code` — stable identifier used by clients.
* `Message` — human-readable error description.

---

# Application Exceptions

Application exceptions are created by inheriting from one of the provided exception categories.

Available categories:

| Exception                | HTTP Status |
| ------------------------ | ----------: |
| `ValidationException`    |         400 |
| `UnauthorizedException`  |         401 |
| `ForbiddenException`     |         403 |
| `NotFoundException`      |         404 |
| `ConflictException`      |         409 |
| `UnprocessableException` |         422 |

Example:

```csharp
public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(Guid userId) : base(
        [new CrError("Identity.UserNotFound", $"User '{userId}' was not found.")],
        "User was not found.")
    {
    }
}
```

Throw exceptions normally:

```csharp
throw new UserNotFoundException(userId);
```

---

# External Error Mapping

External systems usually expose their own error codes.

For example, an external API may return:

```
user_not_found
````

These codes can be registered in `ErrorMap` and converted into application-level errors:

```csharp
ErrorMap errorMap = _errorMapBuilder.Add(new ErrorRegistration(
    "user_not_found",
    [new CrError("IdentityUserNotFound", "User was not found.")])).Build();
````

After registration, the application can resolve errors by external code:

```csharp
if (errorMap.TryGet("user_not_found", out var errors))
{
    throw new UserNotFoundException(errors);
}
```

This allows application services to decide how external errors should be handled.

For example, API clients may map external HTTP responses to application exceptions:

```csharp
switch (response.StatusCode)
{
    case 404:
        throw new ApiNotFoundException(errors);

    case 403:
        throw new ApiForbiddenException(errors);

    default:
        throw new ApiException(errors);
}
```

---

# ExceptionFactory

`ExceptionFactory` creates typed exceptions from registered error codes.

Example registration:

```csharp
var registration = new ErrorRegistration(
    "invalid_grant",
    [new CrError("IdentityInvalidCredentials", "Invalid username or password.")]);

ExceptionFactory factory = _exceptionFactoryBuilder.Add(
    new ExceptionRegistration(
        registration,
        errors => new InvalidCredentialsException(errors))).Build();
```

After registration:

```csharp
var exception = factory.Create("invalid_grant");
throw exception;
```

The factory only handles registered error codes.
If a code is not registered, the application should decide how to handle this case.

---

# ASP.NET Core Integration

CR.Exceptions automatically converts `CrException` instances into RFC 7807 `ProblemDetails` responses.

Example response:

```json
{
  "type": "https://tools.ietf.org/html/rfc9110#section-15.5.5",
  "title": "Not Found",
  "status": 404,
  "detail": "The requested resource was not found.",
  "instance": "/api/test",
  "errors": [
    {
      "code": "TestNotFound",
      "message": "Test Entity not found"
    }
  ],
  "traceId": "5a1192a06ca5cd006057ca7e6b84e231"
}
```

Clients should use the `errors[].code` value as the stable identifier.

---

# Internal Errors

Unexpected exceptions are converted into a generic internal error response.

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
  "traceId": "3e071f69b5f9e695a32a699369b57651"
}
```