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
using Telegram.Bot.Framework.InternalFramework.InterFaces;

namespace Telegram.Bot.Framework.InternalFramework.Mangers
{
    /// <summary>
    /// 
    /// </summary>
    internal class BotNameManger : IBotNameManger
    {
        private readonly Dictionary<string, HashSet<string>> Command_BotNames;
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="serviceProvider"></param>
        public BotNameManger(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 机器人名称
        /// </summary>
        public string BotName { get; set; } = null;

        /// <summary>
        /// 判断
        /// </summary>
        /// <param name="CommandName"></param>
        /// <returns></returns>
        public bool Contains(string CommandName)
        {
            return BotName != null && GetBotNames(CommandName).Contains(BotName);
        }

        /// <summary>
        /// 获取Bot名称列表
        /// </summary>
        /// <param name="CommandName">指令名称</param>
        /// <returns></returns>
        public HashSet<string> GetBotNames(string CommandName)
        {
            if (Command_BotNames.ContainsKey(CommandName))
                return Command_BotNames[CommandName];
            return new HashSet<string>();
        }
    }
}
