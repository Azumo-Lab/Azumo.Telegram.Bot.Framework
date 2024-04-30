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

using Telegram.Bot.Framework.Core.Controller;

namespace Telegram.Bot.Framework.InternalCore.Controller;

/// <summary>
/// 
/// </summary>
/// <param name="func"></param>
/// <param name="paramList"></param>
/// <param name="attributes"></param>
internal class BotCommandInvoker(Func<IServiceProvider, object?[], object> func, List<IGetParam> paramList, Attribute[] attributes)
    : IExecutor
{
    /// <summary>
    /// 
    /// </summary>
    private readonly Func<IServiceProvider, object?[], object> _func = func;

    /// <summary>
    /// 
    /// </summary>
    public IReadOnlyList<IGetParam> Parameters { get; } = new List<IGetParam>(paramList);

    /// <summary>
    /// 
    /// </summary>
    public Attribute[] Attributes { get; } = attributes;

    /// <summary>
    /// 
    /// </summary>
    public Dictionary<string, object> Cache { get; } = [];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="serviceProvider"></param>
    /// <param name="param"></param>
    /// <returns></returns>
    public Task Invoke(IServiceProvider serviceProvider, object?[] param) =>
        _func(serviceProvider, param) is Task task ? task : Task.CompletedTask;
}
