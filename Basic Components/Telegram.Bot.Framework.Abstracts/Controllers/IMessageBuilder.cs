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

namespace Telegram.Bot.Framework.Abstracts.Controllers;

/// <summary>
/// HTML TG 消息创建
/// </summary>
public interface IMessageBuilder
{
    /// <summary>
    /// 添加消息类型接口实例
    /// </summary>
    /// <param name="messageContent"></param>
    /// <returns></returns>
    public IMessageBuilder Add(IMessageContent messageContent);

    /// <summary>
    /// 创建HTML消息
    /// </summary>
    /// <returns></returns>
    public string Build();
}
