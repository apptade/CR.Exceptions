# Intro

A lightweight library for defining application errors, creating typed exceptions, and translating external exceptions into domain-specific exceptions.

This package contains only the core exception model and has no ASP.NET Core dependencies.

## Features

* Typed application exceptions
* Standard exception categories
* Structured application errors (`CrError`)
* Error code → exception mapping (`ExceptionFactory`)
* Exception → exception translation (`ExceptionTranslator`)
* No ASP.NET Core dependencies

---

# Installation

```bash
dotnet add package CrCore.Exceptions
```

---

# Error Model

Every application error is represented by `CrError`.

```csharp
var error = new CrError(
    "IdentityUserNotFound",
    "User was not found.");
```

Each error contains:

* `Code` — stable identifier intended for clients.
* `Message` — human-readable error description.

---

# Exception Categories

Applications should inherit from one of the predefined exception categories.

Available categories:

| Exception                | Purpose                 |
| ------------------------ | ----------------------- |
| `ValidationException`    | Validation failures     |
| `UnauthorizedException`  | Authentication required |
| `ForbiddenException`     | Access denied           |
| `NotFoundException`      | Resource not found      |
| `ConflictException`      | Resource conflict       |
| `UnprocessableException` | Business rule violation |
| `InternalException`      | Internal server error   |

Example:

```csharp
public sealed class UserNotFoundException : NotFoundException
{
    public UserNotFoundException() 
        : base([new CrError("IdentityUserNotFound","User was not found.")])
    {
    }
}
```

Usage:

```csharp
throw new UserNotFoundException();
```

---

# ExceptionFactory

External APIs often return string error codes.

For example:

```text
invalid_grant
```

`ExceptionFactory` maps those codes to typed exceptions.

Registration:

```csharp
ExceptionFactory factory = new ExceptionFactoryBuilder()
    .Map(
        "invalid_grant",
        static () => new InvalidCredentialsException())
    .Build();
```

Usage:

```csharp
throw factory.Create("invalid_grant");
```

This keeps external service contracts isolated from your application.

---

# ExceptionTranslator

Infrastructure exceptions are often not suitable for the application layer.

`ExceptionTranslator` converts one exception type into another.

Registration:

```csharp
ExceptionTranslator translator = new ExceptionTranslatorBuilder()
    .Map<KeycloakUserNotFoundException>(
        static innerException => new UserNotFoundException(innerException))
    .Build();
```

Usage:

```csharp
catch (Exception ex)
{
    throw translator.Translate(ex);
}
```

This allows infrastructure-specific exceptions to remain inside the infrastructure layer while exposing domain-specific exceptions to the rest of the application.