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
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Authentication.Interface;
using Telegram.Bot.Framework.Authentication.Internal;

namespace Telegram.Bot.Framework.Authentication
{
    /// <summary>
    /// 
    /// </summary>
    public static class Setup
    {
        public static IBuilder AddAuthentication<T>(this IBuilder builder) where T : class, IAuthentication
        {
            builder.RuntimeServices.AddScoped<IAuthentication, T>();
            return builder;
        }

        public static IBuilder AddAuthenticationRole<T>(this IBuilder builder) where T : class, IAuthenticationRole
        {
            builder.RuntimeServices.AddScoped<IAuthenticationRole, T>();
            return builder;
        }

        public static IBuilder UseAuthentication(this IBuilder builder)
        {
            builder.RuntimeServices.AddScoped<IAuthenticationRole, AuthRoleDefault>();
            builder.RuntimeServices.AddScoped<IAuthenticationRole, AuthRoleChatMember>();
            builder.RuntimeServices.AddScoped<IAuthenticationRole, AuthRoleChatAdministrators>();
            builder.RuntimeServices.AddScoped<IAuthenticationRole, AuthRoleChat>();
            builder.RuntimeServices.AddScoped<IAuthenticationRole, AuthRoleAllPrivateChats>();
            builder.RuntimeServices.AddScoped<IAuthenticationRole, AuthRoleAllGruopChats>();
            builder.RuntimeServices.AddScoped<IAuthenticationRole, AuthRoleAllChatAdministrators>();
            return builder;
        }
    }
}
