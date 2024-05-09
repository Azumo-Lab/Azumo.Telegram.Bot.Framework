using System;

namespace Telegram.Bot.Framework.Helpers
{
#if NET8_0_OR_GREATER
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0022:使用表达式主体来表示方法", Justification = "<挂起>")]
#endif
    internal static class ExceptionHelper
    {
        public static void ThrowIfNullOrEmpty(string? value, string paramName)
        {
#if NET8_0_OR_GREATER
            ArgumentException.ThrowIfNullOrEmpty(value, paramName);
#elif NETSTANDARD2_1_OR_GREATER
            if (string.IsNullOrEmpty(value))
                throw new ArgumentNullException(paramName);
#endif
        }

        public static void ThrowIfNull(object? value, string paramName)
        {
#if NET8_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(value, paramName);
#elif NETSTANDARD2_1_OR_GREATER
            if (value == null)
                throw new ArgumentNullException(paramName);
#endif
        }
    }
}
