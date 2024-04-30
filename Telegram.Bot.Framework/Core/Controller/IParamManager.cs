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

namespace Telegram.Bot.Framework.Core.Controller;

/// <summary>
/// 参数获取管理接口
/// </summary>
internal interface IParamManager
{
    /// <summary>
    /// 设置参数列表
    /// </summary>
    /// <param name="paramList">参数列表</param>
    public void SetParamList(IEnumerable<IGetParam> paramList);

    /// <summary>
    /// 读取参数
    /// </summary>
    /// <param name="userContext">用户上下文</param>
    /// <returns>参数是否读取完成</returns>
    public Task<bool> Read(TelegramUserContext userContext);

    /// <summary>
    /// 获取读取到的参数
    /// </summary>
    /// <returns>读取到的参数数组</returns>
    public object?[] GetParam();
}
