using System.Runtime.CompilerServices;

namespace CR.Exceptions;

internal static class FuncExtensions
{
    extension<TResult>(Func<TResult> func)
    {
        public TResult ToResult([CallerArgumentExpression(nameof(func))] string? paramName = null)
        {
            ArgumentNullException.ThrowIfNull(func, paramName);
            return func() ?? throw new NullReferenceException("delegate return null");
        }
    }
}