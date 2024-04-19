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

namespace Telegram.Bot.Framework.Core.BotBuilder;

/// <summary>
/// Telegram模块创建器
/// </summary>
public interface ITelegramModuleBuilder
{
    /// <summary>
    /// 添加模块
    /// </summary>
    /// <param name="module"></param>
    /// <returns></returns>
    public ITelegramModuleBuilder AddModule(ITelegramModule module);

    /// <summary>
    /// 添加模块
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public ITelegramModuleBuilder AddModule<T>(params object[] objects) where T : ITelegramModule;

    /// <summary>
    /// 开始创建
    /// </summary>
    /// <returns></returns>
    public ITelegramBot Build();
}
