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
using Telegram.Bot.Framework.Abstract.Params;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Attributes;

namespace Telegram.Bot.Framework.Abstract.Controller
{
    /// <summary>
    /// 用于保存一个指令的各类信息
    /// </summary>
    internal interface IControllerContext
    {
        /// <summary>
        /// 方法上的指令信息
        /// </summary>
        public BotCommandAttribute BotCommandAttribute { get; }

        public AuthenticationAttribute AuthenticationAttribute { get; }

        public DefaultTypeAttribute DefaultTypeAttribute { get; }

        public DefaultMessageAttribute DefaultMessageAttribute { get; }

        public MethodInfo MethodInfo { get; }

        public List<ParamModel> ParamModels { get; }

        Func<TelegramController, object[], Task> Action { get; }

        public Type ControllerType { get; }
    }

    /// <summary>
    /// 参数
    /// </summary>
    internal class ParamModel
    {
        public Type ParamType { get; set; }

        public Type ParamMsg { get; set; }

        public Type ParamMaker { get; set; }

        public ParamAttribute ParamAttr { get; set; }

        public ParameterInfo ParameterInfo { get; set; }
    }
}
