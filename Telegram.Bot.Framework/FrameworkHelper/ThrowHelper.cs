using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.FrameworkHelper
{
    internal static class ThrowHelper
    {
        public static void ThrowIfNullOrEmpty(string str)
        {
            ThrowIfNullOrEmpty(str, "文本不能为NULL，或空字符串");
        }

        public static void ThrowIfNullOrEmpty(string str, string ErrorInfo)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullException(ErrorInfo);
        }

        public static void ThrowIfZeroAndDown(int number, string ErrorInfo)
        {
            if (number <= 0)
                throw new ArgumentException(ErrorInfo);
        }

        public static void ThrowIfZeroAndDown(int number)
        {
            ThrowIfZeroAndDown(number, "不能小于或等于零");
        }
    }
}
