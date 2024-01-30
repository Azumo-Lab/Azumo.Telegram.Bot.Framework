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

using Azumo.PipelineMiddleware.Pipelines;

namespace Azumo.PipelineMiddleware;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TInput"></typeparam>
public interface IPipelineController<TInput> : IPipelineController<TInput, Task>
{

}

/// <summary>
/// 流水线控制器
/// </summary>
/// <remarks>
/// 作用是控制流水线的执行
/// </remarks>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TResult"></typeparam>
public interface IPipelineController<TInput, TResult>
{
    #region

    /// <summary>
    /// 
    /// </summary>
    internal MiddlewareDelegate<TInput, TResult>? NextHandle { get; set; }

    #endregion

    /// <summary>
    /// 获取指定的流水线
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    public IPipeline<TInput, TResult> GetPipeline(object name);

    /// <summary>
    /// 执行下一个操作
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public TResult Next(TInput input);

    /// <summary>
    /// 停止流水线的执行
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public TResult Stop(TInput input);

    /// <summary>
    /// 添加一个指定的流水线
    /// </summary>
    /// <param name="pipeline"></param>
    /// <param name="name"></param>
    public void AddPipeline(IPipeline<TInput, TResult> pipeline, object name);

    /// <summary>
    /// 删除一个指定的流水线
    /// </summary>
    /// <param name="name"></param>
    public void RemovePipeline(object name);

    /// <summary>
    /// 执行一个指定的流水线
    /// </summary>
    /// <param name="name"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    public TResult Execute(object name, TInput input);
}
