namespace PurcellLibs.Extensions;

/// <summary>
/// Exception 扩展方法类
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if argument is null.
    /// </summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="message">A message that describes the error.</param>
    /// <param name="paramName">The name of the parameter that caused the exception.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static T ThrowIfArgumentNull<T>(this T? argument, string? message,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument == null)
        {
            throw new ArgumentNullException(paramName, message);
        }

        return argument;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if argument is null.
    /// </summary>
    /// <param name="argument">The reference type argument to validate as non-null.</param>
    /// <param name="message">A message that describes the error.</param>
    /// <param name="paramName">The name of the parameter that caused the exception.</param>
    /// <exception cref="ArgumentNullException"></exception>
    public static string ThrowIfStringEmpty(this string? argument, string? message,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (string.IsNullOrEmpty(argument))
        {
            throw new ArgumentNullException(paramName, message);
        }

        return argument;
    }
}