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

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.Controller.Results;
using Telegram.Bot.Framework.Filters;
using Telegram.Bot.Framework.PipelineMiddleware;

namespace Telegram.Bot.Framework.CorePipelines
{
    /// <summary>
    /// 
    /// </summary>
    internal class ControllerPipelineControllerInvoker : IMiddleware<TelegramActionContext, Task>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="Next"></param>
        /// <returns></returns>
        public async Task Execute(TelegramActionContext input, PipelineMiddlewareDelegate<TelegramActionContext, Task> Next)
        {
            var executor = input.CommandScopeService.Executor;
            // 纯文字，纯内容，没有指令
            if(executor == null)
            {
                await Next(input);
                return;
            }

            // 带指令内容，执行指令
            var actionResultObjs = new List<object?>();
            var controllerResult = await executor.ActionExecute(input);
            switch (controllerResult.Item1)
            {
                case ControllerResult.Success:
                    try
                    {
                        actionResultObjs.Add(await executor.Invoke(input));
                    }
                    catch (Exception ex)
                    {
                        static bool IsFilterException(Exception exception) => 
                            exception is TargetInvocationException 
                            || exception is TargetParameterCountException;

                        // 参数错误
                        if (IsFilterException(ex))
                        {
                            var Execute = input.ServiceProvider.GetServices<IOnControllerExecute>();
                            foreach (var item in Execute)
                                actionResultObjs.Add(await item.OnParamterError(input));
                        }
                        else
                        {
                            throw;
                        }
                    }
                    break;
                case ControllerResult.Unauthorized:
                case ControllerResult.Forbidden:
                    var onControllerExecute = input.ServiceProvider.GetServices<IOnControllerExecute>();
                    switch (controllerResult.Item1)
                    {
                        case ControllerResult.Unauthorized:
                            foreach (var item in onControllerExecute)
                                actionResultObjs.Add(await item.OnUnauthorized(input));
                            break;
                        case ControllerResult.Forbidden:
                            foreach (var item in onControllerExecute)
                                actionResultObjs.Add(await item.OnForbidden(input));
                            break;
                    }
                    break;
                case ControllerResult.WaitParamter:
                    actionResultObjs.Add(controllerResult.Item2);
                    return;
            }

            // 执行结果
            if (!actionResultObjs.IsEmpty())
            {
                // 遍历执行结果
                foreach (var item in actionResultObjs)
                {
                    if (item != null)
                    {
                        if (item is IActionResult actionResult)
                            await actionResult.ExecuteResultAsync(input, input.CancellationToken);
                        else
                        {
                            var stringResult = item.ToString();
                            if (!string.IsNullOrEmpty(stringResult))
                                await new TextMessageResult(stringResult)
                                    .ExecuteResultAsync(input, input.CancellationToken);
                        }
                    }
                }
            }
        }
    }
}
