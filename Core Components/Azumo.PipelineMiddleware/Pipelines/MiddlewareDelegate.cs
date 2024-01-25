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

namespace Azumo.PipelineMiddleware.Pipelines;

/// <summary>
/// 中间件委托
/// </summary>
/// <remarks>
/// 借助这个委托，实现了流水线的处理功能。
/// </remarks>
/// <typeparam name="TInput">处理类型</typeparam>
/// <param name="input">要处理的数据</param>
/// <param name="pipelineController">流水线控制器</param>
/// <returns>异步任务</returns>
internal delegate Task MiddlewareDelegate<TInput>(TInput input, IPipelineController<TInput> pipelineController);
