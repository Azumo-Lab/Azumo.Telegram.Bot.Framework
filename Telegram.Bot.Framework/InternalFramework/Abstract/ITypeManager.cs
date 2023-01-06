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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.InternalFramework.Abstract
{
    /// <summary>
    /// 
    /// </summary>
    internal interface ITypeManager
    {
        /// <summary>
        /// 当前使用的Bot名称
        /// </summary>
        string BotName { get; }

        /// <summary>
        /// 获取控制器类型
        /// </summary>
        /// <param name="CommandName"></param>
        /// <returns></returns>
        Type GetControllerType(string CommandName);

        /// <summary>
        /// 获取所有的CommandInfos信息
        /// </summary>
        /// <returns></returns>
        List<CommandInfos> GetCommandInfos();

        /// <summary>
        /// 获取所有的CommandInfos信息
        /// </summary>
        /// <returns></returns>
        Dictionary<string, CommandInfos> GetCommandInfosDic();

        /// <summary>
        /// 获得一个方法
        /// </summary>
        /// <param name="CommandName"></param>
        /// <returns></returns>
        MethodInfo GetControllerMethod(string CommandName);

        /// <summary>
        /// 判断是否存在某个指令名
        /// </summary>
        /// <param name="CommandName"></param>
        /// <returns></returns>
        bool ContainsCommandName(string CommandName);

        /// <summary>
        /// 判断方法允许的BotName中是否有本Bot
        /// </summary>
        /// <param name="CommandName"></param>
        /// <returns></returns>
        bool ContainsBotName(string CommandName);

        /// <summary>
        /// 获取Bot名称列表
        /// </summary>
        /// <param name="CommandName">指令名称</param>
        /// <returns></returns>
        HashSet<string> GetCommandBotNames(string CommandName);

        /// <summary>
        /// 获取兜底的消息类型
        /// </summary>
        /// <param name="messageType"></param>
        /// <returns></returns>
        List<CommandInfos> GetMessageController(MessageType messageType);
    }
}
