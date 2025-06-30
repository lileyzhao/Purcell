using System.Runtime.CompilerServices;

namespace SharpEdge.Extensions;

public static class NullableExtensions
{
    /// <summary>
    /// Returns <paramref name="defaultValue"/> if <paramref name="value"/> is <see langword="null"/>; otherwise, returns <paramref name="value"/>.
    /// 如果 <paramref name="value"/> 为 <see langword="null"/>，则返回 <paramref name="defaultValue"/>；否则返回 <paramref name="value"/>。
    /// </summary>
    /// <typeparam name="T">The type of the reference type parameter. 引用类型参数的类型。</typeparam>
    /// <param name="value">The nullable reference value to check. 要检查的可空引用值。</param>
    /// <param name="defaultValue">The default value to return if <paramref name="value"/> is <see langword="null"/>. 当 <paramref name="value"/> 为 <see langword="null"/> 时返回的默认值。</param>
    /// <returns><paramref name="value"/> if it is not <see langword="null"/>; otherwise, <paramref name="defaultValue"/>. 如果 <paramref name="value"/> 不为 <see langword="null"/> 则返回 <paramref name="value"/>；否则返回 <paramref name="defaultValue"/>。</returns>
    public static T DefaultIfNull<T>(this T? value, T defaultValue)
        where T : class
    {
        return value ?? defaultValue;
    }

    /// <summary>
    /// Returns the result of <paramref name="factory"/> if <paramref name="value"/> is <see langword="null"/>; otherwise, returns <paramref name="value"/>.
    /// 如果 <paramref name="value"/> 为 <see langword="null"/>，则返回 <paramref name="factory"/> 的结果；否则返回 <paramref name="value"/>。
    /// </summary>
    /// <typeparam name="T">The type of the reference type parameter. 引用类型参数的类型。</typeparam>
    /// <param name="value">The nullable reference value to check. 要检查的可空引用值。</param>
    /// <param name="factory">A factory function that produces the default value when <paramref name="value"/> is <see langword="null"/>. 当 <paramref name="value"/> 为 <see langword="null"/> 时生成默认值的工厂函数。</param>
    /// <returns><paramref name="value"/> if it is not <see langword="null"/>; otherwise, the result of <paramref name="factory"/>. 如果 <paramref name="value"/> 不为 <see langword="null"/> 则返回 <paramref name="value"/>；否则返回 <paramref name="factory"/> 的结果。</returns>
    public static T DefaultIfNull<T>(this T? value, Func<T> factory)
        where T : class
    {
        return value ?? factory();
    }

    /// <summary>
    /// Returns the result of <paramref name="factory"/> invoked with <paramref name="arg"/> if <paramref name="value"/> is <see langword="null"/>; otherwise, returns <paramref name="value"/>.
    /// 如果 <paramref name="value"/> 为 <see langword="null"/>，则返回使用 <paramref name="arg"/> 调用 <paramref name="factory"/> 的结果；否则返回 <paramref name="value"/>。
    /// </summary>
    /// <typeparam name="T">The type of the reference type parameter. 引用类型参数的类型。</typeparam>
    /// <typeparam name="TArg">The type of the argument to pass to the factory. 传递给工厂函数的参数类型。</typeparam>
    /// <param name="value">The nullable reference value to check. 要检查的可空引用值。</param>
    /// <param name="factory">A factory function that produces the default value when <paramref name="value"/> is <see langword="null"/>. 当 <paramref name="value"/> 为 <see langword="null"/> 时生成默认值的工厂函数。</param>
    /// <param name="arg">The argument to pass to the factory function. 传递给工厂函数的参数。</param>
    /// <returns><paramref name="value"/> if it is not <see langword="null"/>; otherwise, the result of <paramref name="factory"/> invoked with <paramref name="arg"/>. 如果 <paramref name="value"/> 不为 <see langword="null"/> 则返回 <paramref name="value"/>；否则返回使用 <paramref name="arg"/> 调用 <paramref name="factory"/> 的结果。</returns>
    public static T DefaultIfNull<T, TArg>(this T? value, Func<TArg, T> factory, TArg arg)
        where T : class
    {
        return value ?? factory(arg);
    }

    /// <summary>
    /// Returns the result of <paramref name="factory"/> invoked with <paramref name="arg1"/> and <paramref name="arg2"/> if <paramref name="value"/> is <see langword="null"/>; otherwise, returns <paramref name="value"/>.
    /// 如果 <paramref name="value"/> 为 <see langword="null"/>，则返回使用 <paramref name="arg1"/> 和 <paramref name="arg2"/> 调用 <paramref name="factory"/> 的结果；否则返回 <paramref name="value"/>。
    /// </summary>
    /// <typeparam name="T">The type of the reference type parameter. 引用类型参数的类型。</typeparam>
    /// <typeparam name="TArg1">The type of the first argument to pass to the factory. 传递给工厂函数的第一个参数类型。</typeparam>
    /// <typeparam name="TArg2">The type of the second argument to pass to the factory. 传递给工厂函数的第二个参数类型。</typeparam>
    /// <param name="value">The nullable reference value to check. 要检查的可空引用值。</param>
    /// <param name="factory">A factory function that produces the default value when <paramref name="value"/> is <see langword="null"/>. 当 <paramref name="value"/> 为 <see langword="null"/> 时生成默认值的工厂函数。</param>
    /// <param name="arg1">The first argument to pass to the factory function. 传递给工厂函数的第一个参数。</param>
    /// <param name="arg2">The second argument to pass to the factory function. 传递给工厂函数的第二个参数。</param>
    /// <returns><paramref name="value"/> if it is not <see langword="null"/>; otherwise, the result of <paramref name="factory"/> invoked with <paramref name="arg1"/> and <paramref name="arg2"/>. 如果 <paramref name="value"/> 不为 <see langword="null"/> 则返回 <paramref name="value"/>；否则返回使用 <paramref name="arg1"/> 和 <paramref name="arg2"/> 调用 <paramref name="factory"/> 的结果。</returns>
    public static T DefaultIfNull<T, TArg1, TArg2>(this T? value, Func<TArg1, TArg2, T> factory, TArg1 arg1, TArg2 arg2)
        where T : class
    {
        return value ?? factory(arg1, arg2);
    }

    /// <summary>
    /// Returns <paramref name="defaultValue"/> if <paramref name="value"/> is <see langword="null"/> and executes <paramref name="onNull"/>; otherwise, returns <paramref name="value"/>.
    /// 如果 <paramref name="value"/> 为 <see langword="null"/>，则执行 <paramref name="onNull"/> 并返回 <paramref name="defaultValue"/>；否则返回 <paramref name="value"/>。
    /// </summary>
    /// <typeparam name="T">The type of the reference type parameter. 引用类型参数的类型。</typeparam>
    /// <param name="value">The nullable reference value to check. 要检查的可空引用值。</param>
    /// <param name="defaultValue">The default value to return if <paramref name="value"/> is <see langword="null"/>. 当 <paramref name="value"/> 为 <see langword="null"/> 时返回的默认值。</param>
    /// <param name="onNull">An action to execute when <paramref name="value"/> is <see langword="null"/>. 当 <paramref name="value"/> 为 <see langword="null"/> 时要执行的操作。</param>
    /// <returns><paramref name="value"/> if it is not <see langword="null"/>; otherwise, <paramref name="defaultValue"/>. 如果 <paramref name="value"/> 不为 <see langword="null"/> 则返回 <paramref name="value"/>；否则返回 <paramref name="defaultValue"/>。</returns>
    public static T DefaultIfNull<T>(this T? value, T defaultValue, Action onNull)
        where T : class
    {
        if (value is null)
        {
            onNull();
            return defaultValue;
        }

        return value;
    }
    
    public static TResult ReturnIfNull<T, TResult>(this T? value, TResult defaultValue, Func<T, TResult> func)
        where T : class
    {
        return value is null ? defaultValue : func(value);
    }

    /// <summary>
    /// Executes the specified <paramref name="action"/> if <paramref name="value"/> is <see langword="null"/> and returns <paramref name="value"/>.
    /// 如果 <paramref name="value"/> 为 <see langword="null"/>，则执行指定的 <paramref name="action"/> 并返回 <paramref name="value"/>。
    /// </summary>
    /// <typeparam name="T">The type of the reference type parameter. 引用类型参数的类型。</typeparam>
    /// <param name="value">The nullable reference value to check. 要检查的可空引用值。</param>
    /// <param name="action">An action to execute when <paramref name="value"/> is <see langword="null"/>. 当 <paramref name="value"/> 为 <see langword="null"/> 时要执行的操作。</param>
    /// <returns>The original <paramref name="value"/>. 原始的 <paramref name="value"/>。</returns>
    public static T? DoIfNull<T>(this T? value, Action action)
        where T : class
    {
        if (value is null) action();
        return value;
    }

    /// <summary>
    /// Executes the specified <paramref name="action"/> with <paramref name="arg"/> if <paramref name="value"/> is <see langword="null"/> and returns <paramref name="value"/>.
    /// 如果 <paramref name="value"/> 为 <see langword="null"/>，则使用 <paramref name="arg"/> 执行指定的 <paramref name="action"/> 并返回 <paramref name="value"/>。
    /// </summary>
    /// <typeparam name="T">The type of the reference type parameter. 引用类型参数的类型。</typeparam>
    /// <typeparam name="TArg">The type of the argument to pass to the action. 传递给操作的参数类型。</typeparam>
    /// <param name="value">The nullable reference value to check. 要检查的可空引用值。</param>
    /// <param name="action">An action to execute when <paramref name="value"/> is <see langword="null"/>. 当 <paramref name="value"/> 为 <see langword="null"/> 时要执行的操作。</param>
    /// <param name="arg">The argument to pass to the action. 传递给操作的参数。</param>
    /// <returns>The original <paramref name="value"/>. 原始的 <paramref name="value"/>。</returns>
    public static T? DoIfNull<T, TArg>(this T? value, Action<TArg> action, TArg arg)
        where T : class
    {
        if (value is null) action(arg);
        return value;
    }

    /// <summary>
    /// Executes the specified <paramref name="action"/> with <paramref name="arg1"/> and <paramref name="arg2"/> if <paramref name="value"/> is <see langword="null"/> and returns <paramref name="value"/>.
    /// 如果 <paramref name="value"/> 为 <see langword="null"/>，则使用 <paramref name="arg1"/> 和 <paramref name="arg2"/> 执行指定的 <paramref name="action"/> 并返回 <paramref name="value"/>。
    /// </summary>
    /// <typeparam name="T">The type of the reference type parameter. 引用类型参数的类型。</typeparam>
    /// <typeparam name="TArg1">The type of the first argument to pass to the action. 传递给操作的第一个参数类型。</typeparam>
    /// <typeparam name="TArg2">The type of the second argument to pass to the action. 传递给操作的第二个参数类型。</typeparam>
    /// <param name="value">The nullable reference value to check. 要检查的可空引用值。</param>
    /// <param name="action">An action to execute when <paramref name="value"/> is <see langword="null"/>. 当 <paramref name="value"/> 为 <see langword="null"/> 时要执行的操作。</param>
    /// <param name="arg1">The first argument to pass to the action. 传递给操作的第一个参数。</param>
    /// <param name="arg2">The second argument to pass to the action. 传递给操作的第二个参数。</param>
    /// <returns>The original <paramref name="value"/>. 原始的 <paramref name="value"/>。</returns>
    public static T? DoIfNull<T, TArg1, TArg2>(this T? value, Action<TArg1, TArg2> action, TArg1 arg1, TArg2 arg2)
        where T : class
    {
        if (value is null) action(arg1, arg2);
        return value;
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is <see langword="null"/>.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/>，则抛出 <see cref="ArgumentNullException"/>。
    /// </summary>
    /// <typeparam name="T">The type of the reference type parameter. 引用类型参数的类型。</typeparam>
    /// <param name="argument">The reference type argument to validate as non-null. 要验证为非 null 的引用类型参数。</param>
    /// <param name="message">The message to include in the exception if <paramref name="argument"/> is null. 当 <paramref name="argument"/> 为 null 时要包含在异常中的消息。</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds. 与 <paramref name="argument"/> 对应的参数名称。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/>. 如果值不为 <see langword="null"/>，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/>. <paramref name="argument"/> 为 <see langword="null"/>。</exception>
    public static void ThrowIfNull<T>(this T? argument, string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        where T : class
    {
        if (argument is not null) return;

        if (string.IsNullOrWhiteSpace(message))
        {
            message = $"The argument '{paramName}' cannot be null. 参数 '{paramName}' 不能为 null。";
        }

        throw new ArgumentNullException(paramName, message);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is <see langword="null"/>.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/>，则抛出 <see cref="ArgumentNullException"/>。
    /// </summary>
    /// <typeparam name="T">The type of the reference type parameter. 引用类型参数的类型。</typeparam>
    /// <param name="argument">The reference type argument to validate as non-null. 要验证为非 null 的引用类型参数。</param>
    /// <param name="message">The message to include in the exception if <paramref name="argument"/> is null. 当 <paramref name="argument"/> 为 null 时要包含在异常中的消息。</param>
    /// <param name="innerException">The exception that is the cause of the current exception. 作为当前异常原因的异常。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/>. 如果值不为 <see langword="null"/>，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/>. <paramref name="argument"/> 为 <see langword="null"/>。</exception>
    public static void ThrowIfNull<T>(this T? argument, string? message, Exception? innerException)
        where T : class
    {
        if (argument is not null) return;

        if (string.IsNullOrWhiteSpace(message))
        {
            message = $"The argument cannot be null. 参数不能为 null。";
        }

        throw new ArgumentNullException(message, innerException);
    }

    /// <summary>
    /// Throws the specified exception if <paramref name="argument"/> is <see langword="null"/>.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/>，则抛出指定的异常。
    /// </summary>
    /// <typeparam name="T">The type of the reference type parameter. 引用类型参数的类型。</typeparam>
    /// <typeparam name="TException">The type of exception to throw. 要抛出的异常类型。</typeparam>
    /// <param name="argument">The reference type argument to validate as non-null. 要验证为非 null 的引用类型参数。</param>
    /// <param name="exception">The exception to throw if <paramref name="argument"/> is null. 当 <paramref name="argument"/> 为 null 时要抛出的异常。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/>. 如果值不为 <see langword="null"/>，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="TException"><paramref name="argument"/> is <see langword="null"/>. <paramref name="argument"/> 为 <see langword="null"/>。</exception>
    public static void ThrowIfNull<T, TException>(this T? argument, TException exception)
        where T : class
        where TException : Exception
    {
        if (argument is not null) return;
        throw exception;
    }

    /// <summary>
    /// Throws an exception created by the specified factory if <paramref name="argument"/> is <see langword="null"/>.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/>，则抛出由指定工厂创建的异常。
    /// </summary>
    /// <typeparam name="T">The type of the reference type parameter. 引用类型参数的类型。</typeparam>
    /// <typeparam name="TException">The type of exception to throw. 要抛出的异常类型。</typeparam>
    /// <param name="argument">The reference type argument to validate as non-null. 要验证为非 null 的引用类型参数。</param>
    /// <param name="exceptionFactory">A factory function that creates the exception to throw if <paramref name="argument"/> is null. 当 <paramref name="argument"/> 为 null 时创建要抛出异常的工厂函数。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/>. 如果值不为 <see langword="null"/>，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="TException"><paramref name="argument"/> is <see langword="null"/>. <paramref name="argument"/> 为 <see langword="null"/>。</exception>
    public static void ThrowIfNull<T, TException>(this T? argument, Func<TException> exceptionFactory)
        where T : class
        where TException : Exception
    {
        if (argument is not null) return;
        throw exceptionFactory();
    }

    /// <summary>
    /// Throws an exception created by the specified factory with an argument if <paramref name="argument"/> is <see langword="null"/>.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/>，则使用 <paramref name="arg"/> 调用 <paramref name="exceptionFactory"/> 创建的异常并抛出。
    /// </summary>
    /// <typeparam name="T">The type of the reference type parameter. 引用类型参数的类型。</typeparam>
    /// <typeparam name="TArg">The type of the argument to pass to the exception factory. 传递给异常工厂的参数类型。</typeparam>
    /// <typeparam name="TException">The type of exception to throw. 要抛出的异常类型。</typeparam>
    /// <param name="argument">The reference type argument to validate as non-null. 要验证为非 null 的引用类型参数。</param>
    /// <param name="exceptionFactory">A factory function that creates the exception to throw if <paramref name="argument"/> is null. 当 <paramref name="argument"/> 为 null 时创建要抛出异常的工厂函数。</param>
    /// <param name="arg">The argument to pass to the exception factory. 传递给异常工厂的参数。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/>. 如果值不为 <see langword="null"/>，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="TException"><paramref name="argument"/> is <see langword="null"/>. <paramref name="argument"/> 为 <see langword="null"/>。</exception>
    public static void ThrowIfNull<T, TArg, TException>(this T? argument, Func<TArg, TException> exceptionFactory,
        TArg arg)
        where T : class
        where TException : Exception
    {
        if (argument is not null) return;
        throw exceptionFactory(arg);
    }

    /// <summary>
    /// Throws an exception created by the specified factory with two arguments if <paramref name="argument"/> is <see langword="null"/>.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/>，则使用 <paramref name="arg1"/> 和 <paramref name="arg2"/> 调用 <paramref name="exceptionFactory"/> 创建的异常并抛出。
    /// </summary>
    /// <typeparam name="T">The type of the reference type parameter. 引用类型参数的类型。</typeparam>
    /// <typeparam name="TArg1">The type of the first argument to pass to the exception factory. 传递给异常工厂的第一个参数类型。</typeparam>
    /// <typeparam name="TArg2">The type of the second argument to pass to the exception factory. 传递给异常工厂的第二个参数类型。</typeparam>
    /// <typeparam name="TException">The type of exception to throw. 要抛出的异常类型。</typeparam>
    /// <param name="argument">The reference type argument to validate as non-null. 要验证为非 null 的引用类型参数。</param>
    /// <param name="exceptionFactory">A factory function that creates the exception to throw if <paramref name="argument"/> is null. 当 <paramref name="argument"/> 为 null 时创建要抛出异常的工厂函数。</param>
    /// <param name="arg1">The first argument to pass to the exception factory. 传递给异常工厂的第一个参数。</param>
    /// <param name="arg2">The second argument to pass to the exception factory. 传递给异常工厂的第二个参数。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/>. 如果值不为 <see langword="null"/>，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="TException"><paramref name="argument"/> is <see langword="null"/>. <paramref name="argument"/> 为 <see langword="null"/>。</exception>
    public static void ThrowIfNull<T, TArg1, TArg2, TException>(this T? argument,
        Func<TArg1, TArg2, TException> exceptionFactory, TArg1 arg1, TArg2 arg2)
        where T : class
        where TException : Exception
    {
        if (argument is not null) return;
        throw exceptionFactory(arg1, arg2);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is <see langword="null"/> or empty.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/> 或空字符串，则抛出 <see cref="ArgumentNullException"/>。
    /// </summary>
    /// <param name="argument">The string argument to validate as non-null and non-empty. 要验证为非 null 且非空的字符串参数。</param>
    /// <param name="message">The message to include in the exception if <paramref name="argument"/> is null or empty. 当 <paramref name="argument"/> 为 null 或空时要包含在异常中的消息。</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds. 与 <paramref name="argument"/> 对应的参数名称。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/> or empty. 如果值不为 <see langword="null"/> 或空，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/> or empty. <paramref name="argument"/> 为 <see langword="null"/> 或空字符串。</exception>
    public static void ThrowIfNullOrEmpty(this string? argument, string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (!string.IsNullOrEmpty(argument)) return;

        if (string.IsNullOrWhiteSpace(message))
        {
            message = $"The argument '{paramName}' cannot be null or empty. 参数 '{paramName}' 不能为 null 或空字符串。";
        }

        throw new ArgumentNullException(paramName, message);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is <see langword="null"/> or empty.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/> 或空字符串，则抛出 <see cref="ArgumentNullException"/>。
    /// </summary>
    /// <param name="argument">The string argument to validate as non-null and non-empty. 要验证为非 null 且非空的字符串参数。</param>
    /// <param name="message">The message to include in the exception if <paramref name="argument"/> is null or empty. 当 <paramref name="argument"/> 为 null 或空时要包含在异常中的消息。</param>
    /// <param name="innerException">The exception that is the cause of the current exception. 作为当前异常原因的异常。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/> or empty. 如果值不为 <see langword="null"/> 或空，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/> or empty. <paramref name="argument"/> 为 <see langword="null"/> 或空字符串。</exception>
    public static void ThrowIfNullOrEmpty(this string? argument, string? message,
        Exception? innerException)
    {
        if (!string.IsNullOrEmpty(argument)) return;

        if (string.IsNullOrWhiteSpace(message))
        {
            message = $"The argument cannot be null or empty. 参数不能为 null 或空字符串。";
        }

        throw new ArgumentNullException(message, innerException);
    }

    /// <summary>
    /// Throws the specified exception if <paramref name="argument"/> is <see langword="null"/> or empty.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/> 或空字符串，则抛出指定的异常。
    /// </summary>
    /// <typeparam name="TException">The type of exception to throw. 要抛出的异常类型。</typeparam>
    /// <param name="argument">The string argument to validate as non-null and non-empty. 要验证为非 null 且非空的字符串参数。</param>
    /// <param name="exception">The exception to throw if <paramref name="argument"/> is null or empty. 当 <paramref name="argument"/> 为 null 或空时要抛出的异常。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/> or empty. 如果值不为 <see langword="null"/> 或空，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="TException"><paramref name="argument"/> is <see langword="null"/> or empty. <paramref name="argument"/> 为 <see langword="null"/> 或空字符串。</exception>
    public static void ThrowIfNullOrEmpty<TException>(this string? argument, TException exception)
        where TException : Exception
    {
        if (!string.IsNullOrEmpty(argument)) return;
        throw exception;
    }

    /// <summary>
    /// Throws an exception created by the specified factory if <paramref name="argument"/> is <see langword="null"/> or empty.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/> 或空字符串，则抛出由指定工厂创建的异常。
    /// </summary>
    /// <typeparam name="TException">The type of exception to throw. 要抛出的异常类型。</typeparam>
    /// <param name="argument">The string argument to validate as non-null and non-empty. 要验证为非 null 且非空的字符串参数。</param>
    /// <param name="exceptionFactory">A factory function that creates the exception to throw if <paramref name="argument"/> is null or empty. 当 <paramref name="argument"/> 为 null 或空时创建要抛出异常的工厂函数。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/> or empty. 如果值不为 <see langword="null"/> 或空，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="TException"><paramref name="argument"/> is <see langword="null"/> or empty. <paramref name="argument"/> 为 <see langword="null"/> 或空字符串。</exception>
    public static void ThrowIfNullOrEmpty<TException>(this string? argument, Func<TException> exceptionFactory)
        where TException : Exception
    {
        if (!string.IsNullOrEmpty(argument)) return;
        throw exceptionFactory();
    }

    /// <summary>
    /// Throws an exception created by the specified factory with an argument if <paramref name="argument"/> is <see langword="null"/> or empty.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/> 或空字符串，则使用 <paramref name="arg"/> 调用 <paramref name="exceptionFactory"/> 创建的异常并抛出。
    /// </summary>
    /// <typeparam name="TArg">The type of the argument to pass to the exception factory. 传递给异常工厂的参数类型。</typeparam>
    /// <typeparam name="TException">The type of exception to throw. 要抛出的异常类型。</typeparam>
    /// <param name="argument">The string argument to validate as non-null and non-empty. 要验证为非 null 且非空的字符串参数。</param>
    /// <param name="exceptionFactory">A factory function that creates the exception to throw if <paramref name="argument"/> is null or empty. 当 <paramref name="argument"/> 为 null 或空时创建要抛出异常的工厂函数。</param>
    /// <param name="arg">The argument to pass to the exception factory. 传递给异常工厂的参数。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/> or empty. 如果值不为 <see langword="null"/> 或空，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="TException"><paramref name="argument"/> is <see langword="null"/> or empty. <paramref name="argument"/> 为 <see langword="null"/> 或空字符串。</exception>
    public static void ThrowIfNullOrEmpty<TArg, TException>(this string? argument,
        Func<TArg, TException> exceptionFactory,
        TArg arg)
        where TException : Exception
    {
        if (!string.IsNullOrEmpty(argument)) return;
        throw exceptionFactory(arg);
    }

    /// <summary>
    /// Throws an exception created by the specified factory with two arguments if <paramref name="argument"/> is <see langword="null"/> or empty.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/> 或空字符串，则使用 <paramref name="arg1"/> 和 <paramref name="arg2"/> 调用 <paramref name="exceptionFactory"/> 创建的异常并抛出。
    /// </summary>
    /// <typeparam name="TArg1">The type of the first argument to pass to the exception factory. 传递给异常工厂的第一个参数类型。</typeparam>
    /// <typeparam name="TArg2">The type of the second argument to pass to the exception factory. 传递给异常工厂的第二个参数类型。</typeparam>
    /// <typeparam name="TException">The type of exception to throw. 要抛出的异常类型。</typeparam>
    /// <param name="argument">The string argument to validate as non-null and non-empty. 要验证为非 null 且非空的字符串参数。</param>
    /// <param name="exceptionFactory">A factory function that creates the exception to throw if <paramref name="argument"/> is null or empty. 当 <paramref name="argument"/> 为 null 或空时创建要抛出异常的工厂函数。</param>
    /// <param name="arg1">The first argument to pass to the exception factory. 传递给异常工厂的第一个参数。</param>
    /// <param name="arg2">The second argument to pass to the exception factory. 传递给异常工厂的第二个参数。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/> or empty. 如果值不为 <see langword="null"/> 或空，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="TException"><paramref name="argument"/> is <see langword="null"/> or empty. <paramref name="argument"/> 为 <see langword="null"/> 或空字符串。</exception>
    public static void ThrowIfNullOrEmpty<TArg1, TArg2, TException>(this string? argument,
        Func<TArg1, TArg2, TException> exceptionFactory, TArg1 arg1, TArg2 arg2)
        where TException : Exception
    {
        if (!string.IsNullOrEmpty(argument)) return;
        throw exceptionFactory(arg1, arg2);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is <see langword="null"/>, empty, or consists only of white-space characters.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/>、空字符串或仅包含空白字符，则抛出 <see cref="ArgumentNullException"/>。
    /// </summary>
    /// <param name="argument">The string argument to validate as non-null, non-empty, and non-whitespace. 要验证为非 null、非空且非空白字符的字符串参数。</param>
    /// <param name="message">The message to include in the exception if <paramref name="argument"/> is null, empty, or whitespace. 当 <paramref name="argument"/> 为 null、空或空白字符时要包含在异常中的消息。</param>
    /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds. 与 <paramref name="argument"/> 对应的参数名称。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/>, empty, or whitespace. 如果值不为 <see langword="null"/>、空或空白字符，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/>, empty, or consists only of white-space characters. <paramref name="argument"/> 为 <see langword="null"/>、空字符串或仅包含空白字符。</exception>
    public static void ThrowIfNullOrWhiteSpace(this string? argument, string? message = null,
        [CallerArgumentExpression(nameof(argument))] string? paramName = null)
    {
        if (!string.IsNullOrWhiteSpace(argument)) return;

        if (string.IsNullOrWhiteSpace(message))
        {
            message =
                $"The argument '{paramName}' cannot be null, empty, or whitespace. 参数 '{paramName}' 不能为 null、空字符串或空白字符。";
        }

        throw new ArgumentNullException(paramName, message);
    }

    /// <summary>
    /// Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is <see langword="null"/>, empty, or consists only of white-space characters.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/>、空字符串或仅包含空白字符，则抛出 <see cref="ArgumentNullException"/>。
    /// </summary>
    /// <param name="argument">The string argument to validate as non-null, non-empty, and non-whitespace. 要验证为非 null、非空且非空白字符的字符串参数。</param>
    /// <param name="message">The message to include in the exception if <paramref name="argument"/> is null, empty, or whitespace. 当 <paramref name="argument"/> 为 null、空或空白字符时要包含在异常中的消息。</param>
    /// <param name="innerException">The exception that is the cause of the current exception. 作为当前异常原因的异常。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/>, empty, or whitespace. 如果值不为 <see langword="null"/>、空或空白字符，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="ArgumentNullException"><paramref name="argument"/> is <see langword="null"/>, empty, or consists only of white-space characters. <paramref name="argument"/> 为 <see langword="null"/>、空字符串或仅包含空白字符。</exception>
    public static void ThrowIfNullOrWhiteSpace(this string? argument, string? message,
        Exception? innerException)
    {
        if (!string.IsNullOrWhiteSpace(argument)) return;

        if (string.IsNullOrWhiteSpace(message))
        {
            message = $"The argument cannot be null, empty, or whitespace. 参数不能为 null、空字符串或空白字符。";
        }

        throw new ArgumentNullException(message, innerException);
    }

    /// <summary>
    /// Throws the specified exception if <paramref name="argument"/> is <see langword="null"/>, empty, or consists only of white-space characters.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/>、空字符串或仅包含空白字符，则抛出指定的异常。
    /// </summary>
    /// <typeparam name="TException">The type of exception to throw. 要抛出的异常类型。</typeparam>
    /// <param name="argument">The string argument to validate as non-null, non-empty, and non-whitespace. 要验证为非 null、非空且非空白字符的字符串参数。</param>
    /// <param name="exception">The exception to throw if <paramref name="argument"/> is null, empty, or whitespace. 当 <paramref name="argument"/> 为 null、空或空白字符时要抛出的异常。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/>, empty, or whitespace. 如果值不为 <see langword="null"/>、空或空白字符，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="TException"><paramref name="argument"/> is <see langword="null"/>, empty, or consists only of white-space characters. <paramref name="argument"/> 为 <see langword="null"/>、空字符串或仅包含空白字符。</exception>
    public static void ThrowIfNullOrWhiteSpace<TException>(this string? argument, TException exception)
        where TException : Exception
    {
        if (!string.IsNullOrWhiteSpace(argument)) return;
        throw exception;
    }

    /// <summary>
    /// Throws an exception created by the specified factory if <paramref name="argument"/> is <see langword="null"/>, empty, or consists only of white-space characters.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/>、空字符串或仅包含空白字符，则抛出由指定工厂创建的异常。
    /// </summary>
    /// <typeparam name="TException">The type of exception to throw. 要抛出的异常类型。</typeparam>
    /// <param name="argument">The string argument to validate as non-null, non-empty, and non-whitespace. 要验证为非 null、非空且非空白字符的字符串参数。</param>
    /// <param name="exceptionFactory">A factory function that creates the exception to throw if <paramref name="argument"/> is null, empty, or whitespace. 当 <paramref name="argument"/> 为 null、空或空白字符时创建要抛出异常的工厂函数。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/>, empty, or whitespace. 如果值不为 <see langword="null"/>、空或空白字符，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="TException"><paramref name="argument"/> is <see langword="null"/>, empty, or consists only of white-space characters. <paramref name="argument"/> 为 <see langword="null"/>、空字符串或仅包含空白字符。</exception>
    public static string ThrowIfNullOrWhiteSpace<TException>(this string? argument, Func<TException> exceptionFactory)
        where TException : Exception
    {
        if (string.IsNullOrWhiteSpace(argument))
        {
            throw exceptionFactory();
        }

        return argument;
    }

    /// <summary>
    /// Throws an exception created by the specified factory with an argument if <paramref name="argument"/> is <see langword="null"/>, empty, or consists only of white-space characters.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/>、空字符串或仅包含空白字符，则使用 <paramref name="arg"/> 调用 <paramref name="exceptionFactory"/> 创建的异常并抛出。
    /// </summary>
    /// <typeparam name="TArg">The type of the argument to pass to the exception factory. 传递给异常工厂的参数类型。</typeparam>
    /// <typeparam name="TException">The type of exception to throw. 要抛出的异常类型。</typeparam>
    /// <param name="argument">The string argument to validate as non-null, non-empty, and non-whitespace. 要验证为非 null、非空且非空白字符的字符串参数。</param>
    /// <param name="exceptionFactory">A factory function that creates the exception to throw if <paramref name="argument"/> is null, empty, or whitespace. 当 <paramref name="argument"/> 为 null、空或空白字符时创建要抛出异常的工厂函数。</param>
    /// <param name="arg">The argument to pass to the exception factory. 传递给异常工厂的参数。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/>, empty, or whitespace. 如果值不为 <see langword="null"/>、空或空白字符，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="TException"><paramref name="argument"/> is <see langword="null"/>, empty, or consists only of white-space characters. <paramref name="argument"/> 为 <see langword="null"/>、空字符串或仅包含空白字符。</exception>
    public static void ThrowIfNullOrWhiteSpace<TArg, TException>(this string? argument,
        Func<TArg, TException> exceptionFactory,
        TArg arg)
        where TException : Exception
    {
        if (!string.IsNullOrWhiteSpace(argument)) return;
        throw exceptionFactory(arg);
    }

    /// <summary>
    /// Throws an exception created by the specified factory with two arguments if <paramref name="argument"/> is <see langword="null"/>, empty, or consists only of white-space characters.
    /// 如果 <paramref name="argument"/> 为 <see langword="null"/>、空字符串或仅包含空白字符，则使用 <paramref name="arg1"/> 和 <paramref name="arg2"/> 调用 <paramref name="exceptionFactory"/> 创建的异常并抛出。
    /// </summary>
    /// <typeparam name="TArg1">The type of the first argument to pass to the exception factory. 传递给异常工厂的第一个参数类型。</typeparam>
    /// <typeparam name="TArg2">The type of the second argument to pass to the exception factory. 传递给异常工厂的第二个参数类型。</typeparam>
    /// <typeparam name="TException">The type of exception to throw. 要抛出的异常类型。</typeparam>
    /// <param name="argument">The string argument to validate as non-null, non-empty, and non-whitespace. 要验证为非 null、非空且非空白字符的字符串参数。</param>
    /// <param name="exceptionFactory">A factory function that creates the exception to throw if <paramref name="argument"/> is null, empty, or whitespace. 当 <paramref name="argument"/> 为 null、空或空白字符时创建要抛出异常的工厂函数。</param>
    /// <param name="arg1">The first argument to pass to the exception factory. 传递给异常工厂的第一个参数。</param>
    /// <param name="arg2">The second argument to pass to the exception factory. 传递给异常工厂的第二个参数。</param>
    /// <returns><paramref name="argument"/> if the value is not <see langword="null"/>, empty, or whitespace. 如果值不为 <see langword="null"/>、空或空白字符，则返回 <paramref name="argument"/>。</returns>
    /// <exception cref="TException"><paramref name="argument"/> is <see langword="null"/>, empty, or consists only of white-space characters. <paramref name="argument"/> 为 <see langword="null"/>、空字符串或仅包含空白字符。</exception>
    public static void ThrowIfNullOrWhiteSpace<TArg1, TArg2, TException>(this string? argument,
        Func<TArg1, TArg2, TException> exceptionFactory, TArg1 arg1, TArg2 arg2)
        where TException : Exception
    {
        if (!string.IsNullOrWhiteSpace(argument)) return;
        throw exceptionFactory(arg1, arg2);
    }
}