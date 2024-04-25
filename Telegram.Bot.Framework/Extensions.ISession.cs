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

using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Storage;

namespace Telegram.Bot.Framework;
public static partial class Extensions
{
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
    /// 具有泛型的值获取方法
    /// </summary>
    /// <typeparam name="T">泛型类型</typeparam>
    /// <param name="session">存储接口</param>
    /// <param name="key">键值</param>
    /// <returns>转换为 <typeparamref name="T"/> 值的数据</returns>
    public static T Get<T>(this ISession session, object key) =>
        (T)session.Get(key);
}
