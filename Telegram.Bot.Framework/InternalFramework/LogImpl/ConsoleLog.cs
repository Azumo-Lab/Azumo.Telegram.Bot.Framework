//  <Telegram.Bot.Framework>
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework.FrameworkHelper;

namespace Telegram.Bot.Framework.InternalFramework.LogImpl
{
    /// <summary>
    /// 
    /// </summary>
    internal class ConsoleLog
    {
        internal static void ConsoleWriteln(params ConsoleContext[] contexts)
        {
            List<ConsoleContext> Contexts = new List<ConsoleContext>
            {
                new ConsoleContext(){ Color = ConsoleColor.White, Message = "[" },
                new ConsoleContext(){ Color = ConsoleColor.Green, Message = Helper.GetNowTime() },
                new ConsoleContext(){ Color = ConsoleColor.White, Message = "]" },
                new ConsoleContext(){ Color = ConsoleColor.White, Message = "[" },
                new ConsoleContext(){ Color = ConsoleColor.Green, Message = Helper.GetTaskID() },
                new ConsoleContext(){ Color = ConsoleColor.White, Message = "]" },
            };
            Contexts.AddRange(contexts);
            foreach (ConsoleContext item in Contexts)
            {
                Console.ForegroundColor = item.Color;
                Console.Write(item.Message);
                Console.ResetColor();
            }
            Console.Write(Environment.NewLine);
        }
    }

    internal class ConsoleContext
    {
        public ConsoleColor Color { get; set; }

        public string Message { get; set; }
    }
}
