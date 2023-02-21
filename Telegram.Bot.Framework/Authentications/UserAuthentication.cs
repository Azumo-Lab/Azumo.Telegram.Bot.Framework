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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Framework.Authentications
{
    /// <summary>
    /// 用户的权限认证
    /// </summary>
    public class UserAuthentication : IAuthentication
    {
        public virtual Task<bool> Auth(TelegramContext context)
        {
            string Command = context.GetCommand();
            if (Command.IsNull())
                return Task.FromResult(true);

            IControllerManager controllerManager = context.UserScope.GetRequiredService<IControllerManager>();
            CommandInfos commandInfo = controllerManager.GetCommandInfo(Command);
            if (commandInfo.IsNull() || commandInfo.AuthenticationAttribute.IsNull())
                return Task.FromResult(true);

            HashSet<AuthenticationRole> roles = commandInfo.AuthenticationAttribute.AuthenticationRole.ToHashSet();
            return Task.FromResult(roles.Contains(context.AuthenticationRole));
        }
    }
}
