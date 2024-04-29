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
using Telegram.Bot.Framework.Core.PipelineMiddleware;
using Telegram.Bot.Framework.InternalCore.CorePipelines.Models;
using Telegram.Bot.Framework.SimpleAuthentication;

namespace Telegram.Bot.Framework.Core.Attributes;

/// <summary>
/// 
/// </summary>
/// <param name="roleName"></param>
[AttributeUsage(AttributeTargets.Method | AttributeTargets.Delegate, AllowMultiple = true)]
public class AuthenticationAttribute(params string[] roleName) : Attribute, IMiddleware<PipelineModel, Task>
{
    /// <summary>
    /// 
    /// </summary>
    public string[] RoleNames { get; } = roleName;

    async Task IMiddleware<PipelineModel, Task>.Invoke(PipelineModel input, PipelineMiddlewareDelegate<PipelineModel, Task> Next)
    {
        var userManager = input.UserContext.UserServiceProvider.GetRequiredService<IContextFilter>();
        if (userManager.Filter(input.UserContext, RoleNames))
            await Next(input);
        return;
    }
}
