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
using Telegram.Bot.Framework.Abstract.Actions;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Authentication.Interface;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.UpdateTypeActions.Actions
{
    /// <summary>
    /// 认证相关的类
    /// </summary>
    internal class ActionAuthentication : IAction
    {
        /// <summary>
        /// 执行认证
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="NextHandle"></param>
        /// <returns></returns>
        public async Task Invoke(TelegramSession session, ActionHandle NextHandle)
        {
            IEnumerable<IAuthentication> authentications = session.UserService.GetServices<IAuthentication>();

            if (authentications.IsEmpty())
                await NextHandle(session);

            foreach (IAuthentication authentication in authentications)
            {
                if (!await authentication.AuthUser(session))
                {
                    await authentication.ErrorMessage(session);
                    return;
                }
            }

            await NextHandle(session);
        }
    }
}
