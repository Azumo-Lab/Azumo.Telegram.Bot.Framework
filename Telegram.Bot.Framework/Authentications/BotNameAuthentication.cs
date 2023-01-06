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
using Telegram.Bot.Framework.InternalFramework.Models;

namespace Telegram.Bot.Framework.Authentications
{
    /// <summary>
    /// 
    /// </summary>
    public class BotNameAuthentication : IAuthentication
    {
        public async Task<bool> Auth(TelegramContext Context)
        {
            IBotNameManager botNameManager = Context.UserScope.GetService<IBotNameManager>();
            IControllerManager controllerManager = Context.UserScope.GetService<IControllerManager>();

            CommandInfos commandInfos = controllerManager.GetCommandInfo(Context.GetCommand());

            //没有这个指令，直接通过
            if (commandInfos == null)
                return true;
            //没有BotName的标签，所以允许所有的通过
            if (commandInfos.BotNames.IsEmpty())
                return true;

            // 判断是否具有指定的名称
            bool HasBotName = commandInfos.BotNames.Contains(botNameManager.GetBotName());
            return await Task.FromResult(HasBotName);
        }
    }
}
