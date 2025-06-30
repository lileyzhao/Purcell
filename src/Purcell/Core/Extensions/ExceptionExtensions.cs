namespace PurcellLibs.Extensions;

/// <summary>
/// 提供 <see cref="Exception"/> 相关的扩展方法，用于参数验证和异常处理。
/// </summary>
public static class ExceptionExtensions
{
    /// <summary>
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/>，则抛出 <see cref="ArgumentNullException"/>。
    /// </summary>
    /// <typeparam name="T">参数的引用类型。</typeparam>
    /// <param name="argument">要验证为非 <see langword="null"/> 的引用类型参数。</param>
    /// <param name="message">描述错误的消息。</param>
    /// <param name="paramName">导致异常的参数名称，由编译器自动提供。</param>
    /// <returns>如果 <paramref name="argument"/> 不为 <see langword="null"/>，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="argument"/> 为 <see langword="null"/> 时抛出。</exception>
    public static T ThrowIfArgumentNull<T>([NotNull] this T? argument, string? message,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (argument == null)
        {
            throw new ArgumentNullException(paramName, message);
        }

        return argument;
    }

    /// <summary>
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/> 或空字符串，则抛出 <see cref="ArgumentNullException"/>。
    /// </summary>
    /// <param name="argument">要验证为非空的字符串参数。</param>
    /// <param name="message">描述错误的消息。</param>
    /// <param name="paramName">导致异常的参数名称，由编译器自动提供。</param>
    /// <returns>如果 <paramref name="argument"/> 不为空，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="ArgumentNullException">当 <paramref name="argument"/> 为 <see langword="null"/> 或空字符串时抛出。</exception>
    public static string ThrowIfStringEmpty([NotNull] this string? argument, string? message,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (string.IsNullOrEmpty(argument))
        {
            throw new ArgumentNullException(paramName, message);
        }

        return argument;
    }
}