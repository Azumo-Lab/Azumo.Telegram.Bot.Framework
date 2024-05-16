//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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

using System.Collections.Generic;
using Telegram.Bot.Framework.Core.Controller;
using Telegram.Bot.Framework.Storage;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 
    /// </summary>
    public static partial class Extensions
    {
        internal const string CommandKey = "{ADD76730-6FE8-4B6C-8E40-AAD5D6883DC8}";
        internal const string RolesKey = "{7ED1518D-121F-4B74-BFE1-1D686DF1E5DF}";

        /// <summary>
        /// 向用户存储中添加指令
        /// </summary>
        /// <param name="session">存储接口</param>
        /// <param name="executor"></param>
        internal static void AddCommand(this ISession session, IExecutor executor) =>
            session.AddOrUpdate(CommandKey, executor);

        /// <summary>
        /// 从用户存储中获取指令
        /// </summary>
        /// <param name="session">存储接口</param>
        /// <returns></returns>
        internal static IExecutor GetCommand(this ISession session) =>
            session.Get<IExecutor>(CommandKey);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="strings"></param>
        internal static void AddRoles(this ISession session, params string[] strings)
        {
            var list = session.Get<List<string>>(RolesKey);
            list.AddRange(strings);
            session.AddOrUpdate(RolesKey, list);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="strings"></param>
        internal static void AddRole(this ISession session, string strings) =>
            AddRoles(session, strings);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        internal static List<string> GetRoles(this ISession session) => 
            session.Get<List<string>>(RolesKey) ?? new List<string>();

        /// <summary>
        /// 具有泛型的值获取方法
        /// </summary>
        /// <typeparam name="T">泛型类型</typeparam>
        /// <param name="session">存储接口</param>
        /// <param name="key">键值</param>
        /// <returns>转换为 <typeparamref name="T"/> 值的数据</returns>
        public static T Get<T>(this ISession session, object key) =>
            (T)session.Get(key);
    }
}
