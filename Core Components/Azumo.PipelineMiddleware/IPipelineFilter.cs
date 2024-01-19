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

namespace Azumo.PipelineMiddleware;

/// <summary>
/// 流水线执行过滤器
/// </summary>
/// <remarks>
/// 用于对流水线进行处理
/// </remarks>
/// <typeparam name="TInput">处理数据类型</typeparam>
public interface IPipelineFilter<TInput>
{
    /// <summary>
    /// 流水线执行前
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<bool> InvokeBefore(TInput input);

    /// <summary>
    /// 流水线执行后
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public Task<bool> InvokeAfter(TInput input);
}
