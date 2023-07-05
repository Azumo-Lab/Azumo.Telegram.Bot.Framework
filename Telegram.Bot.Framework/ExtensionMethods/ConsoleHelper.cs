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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.ExtensionMethods
{
    /// <summary>
    /// 
    /// </summary>
    public class ConsoleHelper
    {
        static ConsoleHelper()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }

        public static void WriteLine(string str, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.WriteLine(str);
            Console.ResetColor();
        }

        public static void Clear()
        {
            Console.ResetColor();
            Console.Clear();
        }

        public static void WriteLine(string str)
        {
            Console.ResetColor();
            Console.WriteLine(str);
        }

        public static void Write(string str, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.Write(str);
            Console.ResetColor();
        }

        public static void Write(string str)
        {
            Console.ResetColor();
            Console.Write(str);
        }

        public static void Info(string str)
        {
            Log(str, nameof(Info), ConsoleColor.Green);
            Write($" {str}");
            Console.WriteLine();
        }

        public static void Error(string str)
        {
            Log(str, nameof(Error), ConsoleColor.Red);
            Write($" {str}", ConsoleColor.Red);
            Console.WriteLine();
        }

        public static void Warn(string str)
        {
            Log(str, nameof(Warn), ConsoleColor.DarkYellow);
            Write($" {str}");
            Console.WriteLine();
        }

        public static void Log(string str, string name, ConsoleColor consoleColor)
        {
            Write("[");
            Write($"{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}", ConsoleColor.Green);
            Write("]");
            Write("[");
            Write($"Thread:", ConsoleColor.DarkMagenta);
            Write($"{Environment.CurrentManagedThreadId}", ConsoleColor.Green);
            Write("]");
            Write("[");
            Write(name, consoleColor);
            Write("]");
        }
    }
}
