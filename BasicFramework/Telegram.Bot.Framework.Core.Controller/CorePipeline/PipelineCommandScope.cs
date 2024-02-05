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

using Azumo.SuperExtendedFramework.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Security.AccessControl;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Controller.CorePipeline.Model;
using Telegram.Bot.Framework.Core.Storage;
using Telegram.Bot.Framework.SimpleAuthentication;
using Telegram.Bot.Framework.SimpleAuthentication.Attributes;

namespace Telegram.Bot.Framework.Core.Controller.CorePipeline;
internal class PipelineCommandScope : IMiddleware<PipelineModel, Task>
{
    private const string RolesNameKey = "{84DB22B4-4450-432E-AA82-DAEF3E1F4C6B}";
    public async Task Invoke(PipelineModel input, PipelineMiddlewareDelegate<PipelineModel, Task> Next)
    {
        // 获取指令
        var exec = input.CommandManager.GetExecutor(input.UserContext);
        if (exec == null)   // 获取不到
            goto Next;      // 非指令，直接进行下一步操作

        // 获取指令的认证条件
        List<string> roles;
        if ((roles = exec.Session.Get<List<string>>(RolesNameKey)) == null)
        {
            roles = [];
            roles.AddRange(exec.Attributes.Where(x => x is AuthenticationAttribute).Cast<AuthenticationAttribute>().Select(x => x.RoleName));
            exec.Session.AddOrUpdate(RolesNameKey, roles);
        }
        // 开始校验
        foreach (var item in input.UserContext.UserServiceProvider.GetServices<IContextFilter>() ?? [])
            if (!item.Filter(input.UserContext, [.. roles]))
                return; // 不能通过认证，权限名称不对等情况

        // 创建新的指令级别的服务范围
        input.CommandScopeService.DeleteOldCreateNew();
        input.CommandScopeService.Session.AddCommand(exec);
        var paramManager = input.CommandScopeService.Service!.GetRequiredService<IParamManager>();
        paramManager.SetParamList(exec.Parameters);

        // 开始执行下一个个操作
    Next:
        await Next(input);
    }
}
