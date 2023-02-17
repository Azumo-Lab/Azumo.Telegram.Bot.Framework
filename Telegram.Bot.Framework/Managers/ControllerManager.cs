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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.InternalFramework.Abstract;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Managers
{
    /// <summary>
    /// 控制器管理的实现类
    /// </summary>
    internal class ControllerManager : IControllerManager
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ITypeManager typeManager;
        /// <summary>
        /// 初始化
        /// </summary>
        public ControllerManager(IServiceProvider serviceProvider, ITypeManager typeManager)
        {
            this.serviceProvider = serviceProvider;
            this.typeManager = typeManager;
        }

        public TelegramController CreateController(string CommandName)
        {
            // 获取控制器类型
            Type controllerType = typeManager.GetControllerType(CommandName);
            
            // 创建控制器实例
            return (TelegramController)ActivatorUtilities.CreateInstance(serviceProvider, controllerType, Array.Empty<object>());
        }

        public TelegramController CreateController(MessageType messageType)
        {
            throw new NotImplementedException();
        }

        public CommandInfos GetCommandInfo(string CommandName)
        {
            if (typeManager.GetCommandInfosDic().TryGetValue(CommandName, out CommandInfos cmdInfoDic))
                return cmdInfoDic;
            return default;
        }
    }
}
