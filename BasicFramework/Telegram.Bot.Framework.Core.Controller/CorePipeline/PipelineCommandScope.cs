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
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Controller.CorePipeline.Model;

namespace Telegram.Bot.Framework.Core.Controller.CorePipeline;
internal class PipelineCommandScope : IMiddleware<PipelineModel, Task>
{
    public async Task Invoke(PipelineModel input, PipelineMiddlewareDelegate<PipelineModel, Task> Next)
    {
        var exec = input.CommandManager.GetExecutor(input.UserContext);
        if (exec == null)
            goto Next;

        input.CommandScopeService.DeleteOldCreateNew();
        input.CommandScopeService.Session.AddCommand(exec);
        var paramManager = input.CommandScopeService.Service!.GetRequiredService<IParamManager>();
        paramManager.SetParamList(exec.Parameters);

    Next:
        await Next(input);
    }
}
