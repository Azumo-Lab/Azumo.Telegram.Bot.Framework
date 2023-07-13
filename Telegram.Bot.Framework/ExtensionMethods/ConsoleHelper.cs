//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using System;
using System.Text;
using System.Threading;

namespace Telegram.Bot.Framework.ExtensionMethods
{
    /// <summary>
    /// 加强版控制台，添加了一些比较方便的功能
    /// </summary>
    public class ConsoleHelper
    {
        /// <summary>
        /// 静态的初始化，用于设定控制台输出的编码
        /// </summary>
        static ConsoleHelper()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        /// <summary>
        /// 输出一行指定颜色的文本
        /// </summary>
        /// <param name="str">指定输出的文本</param>
        /// <param name="consoleColor">指定的颜色</param>
        public static void WriteLine(string str, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(str);
            Console.ResetColor();
        }

        /// <summary>
        /// 清除所有文本，包括清除设定的颜色
        /// </summary>
        public static void Clear()
        {
            Console.ResetColor();
            Console.Clear();
        }

        /// <summary>
        /// 输出一行文本，如果有颜色的话，清除颜色的设置
        /// </summary>
        /// <param name="str">要输出的文本</param>
        public static void WriteLine(string str)
        {
            Console.ResetColor();
            Console.WriteLine(str);
        }

        /// <summary>
        /// 输出文本
        /// </summary>
        /// <param name="str">要输出的文本</param>
        /// <param name="consoleColor">指定的颜色</param>
        public static void Write(string str, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.Write(str);
            Console.ResetColor();
        }

        /// <summary>
        /// 输出无颜色的文本
        /// </summary>
        /// <param name="str">想要输出的文本</param>
        public static void Write(string str)
        {
            Console.ResetColor();
            Console.Write(str);
        }

        /// <summary>
        /// 输出类似Log的文本
        /// </summary>
        /// <param name="str">想要输出的文本</param>
        public static void Info(string str)
        {
            Log(ConsoleSetting.GetConsoleSetting(str, nameof(Info), ConsoleColor.Green));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        public static void Error(string str)
        {
            Log(ConsoleSetting.GetConsoleSetting(str, nameof(Error), ConsoleColor.Red, ConsoleColor.Red));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        public static void Warn(string str)
        {
            Log(ConsoleSetting.GetConsoleSetting(str, nameof(Warn), ConsoleColor.DarkYellow));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="consoleSetting"></param>
        private static void Log(ConsoleSetting consoleSetting)
        {
            ThreadPool.QueueUserWorkItem((setting) =>
            {
                lock (__LockObj)
                {
                    Write("[");
                    Write($"{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}", ConsoleColor.Green);
                    Write("]");
                    Write("[");
                    Write($"Thread:", ConsoleColor.DarkMagenta);
                    Write($"{Environment.CurrentManagedThreadId}", ConsoleColor.Green);
                    Write("]");
                    Write("[");
                    if ((int)setting.TextContentColor == 99)
                        Write(setting.Str);
                    else
                        Write(setting.Str, setting.ConsoleColor);
                    Write("]");
                    Console.WriteLine();
                }
            }, consoleSetting, true);
        }

        /// <summary>
        /// 
        /// </summary>
        private readonly static object __LockObj = new object();

        /// <summary>
        /// 
        /// </summary>
        private struct ConsoleSetting
        {
            /// <summary>
            /// 
            /// </summary>
            public string Str { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public ConsoleColor ConsoleColor { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public ConsoleColor TextContentColor { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="str"></param>
            /// <param name="name"></param>
            /// <param name="consoleColor"></param>
            /// <param name="textContentColor"></param>
            /// <returns></returns>
            public static ConsoleSetting GetConsoleSetting(string str, string name, ConsoleColor consoleColor, ConsoleColor textContentColor = (ConsoleColor)99)
            {
                return new ConsoleSetting
                {
                    Str = str,
                    Name = name,
                    ConsoleColor = consoleColor,
                    TextContentColor = textContentColor,
                };
            }
        }
    }
}
