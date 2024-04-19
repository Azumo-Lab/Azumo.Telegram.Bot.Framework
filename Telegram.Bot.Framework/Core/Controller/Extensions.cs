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

namespace Telegram.Bot.Framework.Core.Controller;

/// <summary>
/// 
/// </summary>
public static class Extensions
{
    /// <summary>
    /// 
    /// </summary>
    private const string CommandKey = "{ADD76730-6FE8-4B6C-8E40-AAD5D6883DC8}";

    /// <summary>
    /// 向用户存储中添加指令
    /// </summary>
    /// <param name="session"></param>
    /// <param name="executor"></param>
    internal static void AddCommand(this ISession session, IExecutor executor) =>
        session.AddOrUpdate(CommandKey, executor);

    /// <summary>
    /// 从用户存储中获取指令
    /// </summary>
    /// <param name="session"></param>
    /// <returns></returns>
    internal static IExecutor GetCommand(this ISession session) =>
        session.Get<IExecutor>(CommandKey);
}
