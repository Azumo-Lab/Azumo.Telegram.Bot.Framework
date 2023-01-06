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

namespace Telegram.Bot.Framework.TelegramAttributes
{
    /// <summary>
    /// 认证标签
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenticationAttribute : Attribute
    {
        /// <summary>
        /// 允许的用户类型
        /// </summary>
        public AuthenticationRole[] AuthenticationRole { get; }

        /// <summary>
        /// 权限
        /// </summary>
        /// <param name="authenticationRole">允许的用户类型</param>
        public AuthenticationAttribute(params AuthenticationRole[] authenticationRole)
        {
            AuthenticationRole = authenticationRole;
        }
    }

    /// <summary>
    /// 角色
    /// </summary>
    public enum AuthenticationRole
    {
        /// <summary>
        /// 管理员，Bot的管理者
        /// </summary>
        BotAdmin,

        /// <summary>
        /// 注册用户，Bot的服务对象，频道管理员群组管理员这一类
        /// </summary>
        RegisteredUser,

        /// <summary>
        /// 一般用户，呼叫Bot的用户
        /// </summary>
        GeneralUser,
    }
}
