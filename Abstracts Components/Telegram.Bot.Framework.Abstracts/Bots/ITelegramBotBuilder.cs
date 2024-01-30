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

namespace Telegram.Bot.Framework.Abstracts.Bots;

/// <summary>
/// 用于构建 <see cref="ITelegramBot"/> 接口
/// </summary>
/// <remarks>
/// 
/// </remarks>
public interface ITelegramBotBuilder
{
    /// <summary>
    /// 添加内容设置
    /// </summary>
    /// <remarks>
    /// 通过添加实现了 <see cref="ITelegramPartCreator"/> 接口的类，来处理和添加相对应的服务
    /// </remarks>
    /// <param name="telegramPartCreator">可扩展的内容设置 <see cref="ITelegramPartCreator"/> </param>
    /// <returns></returns>
    public ITelegramBotBuilder AddTelegramPartCreator(ITelegramPartCreator telegramPartCreator);

    /// <summary>
    /// 设置完成后，进行 <see cref="ITelegramBot"/> 对象的构建
    /// </summary>
    /// <remarks>
    /// 完成所有的服务构建后，开始构建 <see cref="ITelegramBot"/> 对象
    /// </remarks>
    /// <returns> 构建 <see cref="ITelegramBot"/> 对象 </returns>
    public ITelegramBot Build();
}
