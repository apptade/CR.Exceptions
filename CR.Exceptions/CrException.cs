namespace CR.Exceptions;

public abstract class CrException : Exception
{
    public IReadOnlyCollection<Error> Errors { get; }

    protected CrException(IReadOnlyCollection<Error> errors, string message, Exception? innerException = null) : base(message, innerException)
    {
        ArgumentNullException.ThrowIfNull(errors);
        ArgumentException.ThrowIfNullOrWhiteSpace(message);

        if (errors.Count == 0)
        {
            throw new ArgumentException("At least one error must be provided.", nameof(errors));
        }

        foreach (var error in errors)
        {
            ArgumentNullException.ThrowIfNull(error, nameof(errors));
            ArgumentException.ThrowIfNullOrWhiteSpace(error.Code, nameof(errors));
            ArgumentException.ThrowIfNullOrWhiteSpace(error.Message, nameof(errors));
        }

        Errors = errors;
    }

    public sealed record class Error(string Code, string Message);
}