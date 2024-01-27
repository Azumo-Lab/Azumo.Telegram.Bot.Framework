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
/// 一个默认的流水线执行前的过滤器
/// </summary>
/// <remarks>
/// 这个执行的作用是将下一步的委托添加到流水线控制器接口中 <see cref="IPipelineController{TInput}.NextHandle"/>
/// 这个类是必须的，否则会造成程序执行异常
/// </remarks>
/// <typeparam name="TInput">处理类型</typeparam>
internal class ControllerPipelineInvokeFilter<TInput> : IPipelineInvokeFilter<TInput>
{
    /// <summary>
    /// 在中间件执行之前，执行的过滤操作
    /// </summary>
    /// <remarks>
    /// 开始执行赋值操作，这里将委托的 <paramref name="handle"/> 转换为 <see cref="MiddlewareDelegate{TInput}"/> 类型后，
    /// 给控制器 <see cref="IPipelineController{TInput}.NextHandle"/> 的属性赋值
    /// </remarks>
    /// <param name="handle">委托，<see cref="MiddlewareDelegate{TInput}"/> 类型</param>
    /// <param name="middleware">中间件</param>
    /// <param name="input">要进行处理的数据</param>
    /// <param name="pipelineController">流水线控制器</param>
    /// <returns>返回是否继续执行的标识，<see cref="true"/> 代表继续执行，<see cref="false"/> 代表停止执行</returns>
    public bool Filter(Delegate handle, IMiddleware<TInput> middleware, TInput input, IPipelineController<TInput> pipelineController)
    {
        // 赋值操作
        pipelineController.NextHandle = (MiddlewareDelegate<TInput>)handle;
        return true;
    }
}
