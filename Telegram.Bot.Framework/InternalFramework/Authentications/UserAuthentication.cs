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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;
using Telegram.Bot.Framework.InternalFramework.Abstract;
using Telegram.Bot.Framework.InternalFramework.Managers;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Framework.TelegramAttributes;

namespace Telegram.Bot.Framework.InternalFramework.Authentications
{
    /// <summary>
    /// 
    /// </summary>
    internal class UserAuthentication : IAuthentication
    {
        private Dictionary<AuthenticationRole, long> UserRole = new Dictionary<AuthenticationRole, long>();

        private IServiceProvider UserService;
        public UserAuthentication(IServiceProvider UserService)
        {
            this.UserService = UserService;
        }

        public bool Auth(TelegramContext context)
        {
            string Command = context.GetCommand();
            if (Command == null)
                return true;
            ITypeManager typeManager = UserService.GetService<ITypeManager>();
            Dictionary<string, CommandInfos> Dic = typeManager.GetCommandInfosDic();
            if (Dic.ContainsKey(Command))
            {
                CommandInfos commandInfos = Dic[Command];
                AuthenticationRole[] role = commandInfos.AuthenticationAttribute?.AuthenticationRole;
                if (role == null)
                    return true;

                if (role.Contains(AuthenticationRole.ADMIN))
                {
                    return context.AuthenticationRole == AuthenticationRole.ADMIN;
                }
            }
            return true;
        }
    }
}
