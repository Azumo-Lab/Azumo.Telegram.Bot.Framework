//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    /// <summary>
    /// 用于权限认证的标签
    /// </summary>
    /// <remarks>
    /// 这个标签可以用于权限认证
    /// </remarks>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false)]
    public class AuthenticateAttribute : Attribute
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        public HashSet<string> RoleName { get; }

        /// <summary>
        /// 有参数的初始化
        /// </summary>
        /// <param name="role"></param>
        public AuthenticateAttribute(params string[] role)
        {
            RoleName = new HashSet<string>(role);
        }

        /// <summary>
        /// 无参数的初始化
        /// </summary>
        public AuthenticateAttribute()
        {
            RoleName = [];
        }
    }
}
