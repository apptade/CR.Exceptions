using System.Runtime.CompilerServices;

namespace CR.Exceptions.Extensions;

internal static class FuncExtensions
{
    extension<TResult>(Func<TResult> func) where TResult : allows ref struct
    {
        public TResult ToResult([CallerArgumentExpression(nameof(func))] string? paramName = null)
        {
            ArgumentNullException.ThrowIfNull(func, paramName);

            return func() ?? throw new ArgumentNullException(paramName, "Func return null");
        }
    }
}