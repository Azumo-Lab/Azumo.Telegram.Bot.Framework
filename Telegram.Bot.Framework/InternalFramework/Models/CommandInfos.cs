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
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.InternalFramework.Models
{
    /// <summary>
    /// 指令信息
    /// </summary>
    internal class CommandInfos
    {
        /// <summary>
        /// 指令名称
        /// </summary>
        public string CommandName { get; set; }

        /// <summary>
        /// 该方法对应的消息类型
        /// </summary>
        public MessageType? MessageType { get; set; }

        /// <summary>
        /// 控制器类型
        /// </summary>
        public Type Controller { get; set; }

        /// <summary>
        /// 方法信息
        /// </summary>
        public MethodInfo CommandMethod { get; set; }

        /// <summary>
        /// 能够使用的Bot名称
        /// </summary>
        public HashSet<string> BotNames { get; set; }

        /// <summary>
        /// 方法参数信息
        /// </summary>
        public List<ParamInfos> ParamInfos { get; set; }

        /// <summary>
        /// 标记信息
        /// </summary>
        public CommandAttribute CommandAttribute { get; set; }

        public AuthenticationAttribute AuthenticationAttribute { get; set; }

        public override string ToString()
        {
            return $"{CommandName}{MessageType}{Controller}{CommandMethod}{CommandAttribute}";
        }
    }
}
