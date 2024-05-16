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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Core.Controller;
using Telegram.Bot.Framework.Filters;
using Telegram.Bot.Framework.InternalCore.CorePipelines.Models;
using Telegram.Bot.Framework.PipelineMiddleware;

namespace Telegram.Bot.Framework.InternalCore.CorePipelines.ControllerInvokePipeline
{
    internal class PipelineCommandScope : IMiddleware<PipelineModel, Task>
    {
        private const string RolesNameKey = "{84DB22B4-4450-432E-AA82-DAEF3E1F4C6B}";
        public async Task Invoke(PipelineModel input, PipelineMiddlewareDelegate<PipelineModel, Task> Next)
        {
            // 获取指令
            var exec = input.CommandManager!.GetExecutor(input.UserContext!);
            if (exec == null)   // 获取不到
                if ((exec = input.CommandScopeService!.Session?.GetCommand()) == null)
                    goto Next;  // 非指令，直接进行下一步操作
                else
                {
                    // 创建新的指令级别的服务范围
                    input.CommandScopeService.DeleteOldCreateNew();
                    input.CommandScopeService.Session!.AddCommand(exec);
                    var paramManager = input.CommandScopeService.Service!.GetRequiredService<IParamManager>();
                    paramManager.SetParamList(exec.Parameters);
                }

            // 开始校验
            // 获取指令的认证条件
            if (!(exec.Cache.TryGetValue(RolesNameKey, out var val) && val is List<string> roles))
            {
                var roleNames = exec.Attributes.Where(x => x is AuthenticationAttribute).Cast<AuthenticationAttribute>().SelectMany(x => x.RoleNames);
                roles =
#if NET8_0_OR_GREATER
                [
                    .. roleNames,
                ];
#else
                new List<string>();
                roles.AddRange(roleNames);
#endif
                exec.Cache[RolesNameKey] = roles;
            }
            foreach (var item in input.UserContext.UserServiceProvider.GetServices<IFilter>())
                if (!await item.InvokeAsync(input.UserContext, exec))
                    return;

                Next:
            // 开始执行下一个操作
            await Next(input);
        }
    }
}
