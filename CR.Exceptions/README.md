# Intro

A lightweight library for defining application errors, creating typed exceptions, and mapping external error codes into domain-specific exceptions.

This package contains only the core exception model and does not depend on ASP.NET Core.

## Features

- Typed application exceptions
- Standard exception categories
- Structured application errors (`CrError`)
- External error mapping (`ErrorMap`)
- Exception factory (`ExceptionFactory`)
- No ASP.NET Core dependencies

---

# Installation

```bash
dotnet add package CrCore.Exceptions
```

---

# Error Model

Every application error is represented by `CrError`.

```csharp
var error = new CrError("IdentityUserNotFound", "User was not found.");
```

Each error contains:

- `Code` — stable identifier for clients.
- `Message` — human-readable description.

---

# Exception Categories

Applications should inherit from one of the predefined exception categories.

Available categories:

| Exception | Purpose |
|-----------|---------|
| `ValidationException` | Validation failures |
| `UnauthorizedException` | Authentication required |
| `ForbiddenException` | Access denied |
| `NotFoundException` | Resource not found |
| `ConflictException` | Resource conflict |
| `UnprocessableException` | Business rule violation |
| `InternalException` | Internal server error |

Example:

```csharp
public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(Guid userId) : base(
        [new CrError("IdentityUserNotFound", $"User '{userId}' was not found.")],
        "User was not found.")
    {
    }
}
```

Usage:

```csharp
throw new UserNotFoundException(userId);
```

---

# ErrorMap

External systems usually expose their own error codes.

For example:

```text
user_not_found
```

Those codes can be mapped into application errors.

```csharp
ErrorMap errorMap = builder
    .Add(new ErrorRegistration(
        "user_not_found",
        [new CrError("IdentityUserNotFound", "User was not found.")]))
    .Build();
```

Resolving an external error:

```csharp
if (errorMap.TryGet("user_not_found", out var errors))
{
    throw new UserNotFoundException(errors);
}
```

This keeps external service contracts isolated from the application domain.

---

# ExceptionFactory

`ExceptionFactory` creates typed exceptions from registered external error codes.

Registration:

```csharp
ExceptionFactory factory = builder
    .Add(new ExceptionRegistration(
        new ErrorRegistration(
            "invalid_grant",
            [new CrError("IdentityInvalidCredentials", "Invalid username or password.")]),
        errors => new InvalidCredentialsException(errors)))
    .Build();
```

Usage:

```csharp
throw factory.Create("invalid_grant");
```

The factory only creates exceptions for registered error codes.