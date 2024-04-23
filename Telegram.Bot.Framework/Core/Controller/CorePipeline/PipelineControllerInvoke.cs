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

using Azumo.SuperExtendedFramework.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Controller.CorePipeline.Model;

namespace Telegram.Bot.Framework.Core.Controller.CorePipeline;

/// <summary>
/// 控制器执行流程
/// </summary>
internal class PipelineControllerInvoke : IMiddleware<PipelineModel, Task>
{
    /// <summary>
    /// 执行控制器
    /// </summary>
    /// <param name="input"></param>
    /// <param name="Next"></param>
    /// <returns></returns>
    public async Task Invoke(PipelineModel input, PipelineMiddlewareDelegate<PipelineModel, Task> Next)
    {
        var paramManager = input.CommandScopeService.Service?.GetRequiredService<IParamManager>();
        var exec = input.CommandScopeService.Session?.GetCommand();

        try
        {
            // 获取指令
            if (exec != null)
                await exec.Invoke(input.UserContext.UserServiceProvider, paramManager?.GetParam() ?? []);
            await Next(input);
        }
        catch (Exception)
        {
            
        }
        finally
        {
            input.CommandScopeService.Delete();
        }
    }
}
