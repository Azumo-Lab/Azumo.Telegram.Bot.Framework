//  < Telegram.Bot.Framework >
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
//
//  This program is free software: you can redistribute it and/or modify
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
using System.Text;

namespace Telegram.Bot.Framework.TelegramAttributes
{
    /// <summary>
    /// 指令标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class CommandAttribute : Attribute
    {
        /// <summary>
        /// 命令名称
        /// </summary>
        public string CommandName { get; }

        /// <summary>
        /// 可以使用的Bot名称
        /// </summary>
        public IEnumerable<string> BotName { get; }

        /// <summary>
        /// 使用标签
        /// </summary>
        /// <param name="CommandName">指令名称</param>
        public CommandAttribute(string CommandName)
        {
            if (!CommandName.StartsWith('/'))
            {
                CommandName = $"/{CommandName}";
            }
            this.CommandName = CommandName;
        }

        /// <summary>
        /// 使用标签
        /// </summary>
        /// <param name="CommandName">指令名称</param>
        /// <param name="BotName">Bot名称(可以使用多个)</param>
        public CommandAttribute(string CommandName, params string[] BotName) : this(CommandName)
        {
            this.BotName = new List<string>(BotName);
        }
    }
}
