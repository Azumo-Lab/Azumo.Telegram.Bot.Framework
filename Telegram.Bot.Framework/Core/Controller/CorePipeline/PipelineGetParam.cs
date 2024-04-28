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
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Controller.CorePipeline.Model;
using Telegram.Bot.Framework.Core.PipelineMiddleware;

namespace Telegram.Bot.Framework.Core.Controller.CorePipeline;
internal class PipelineGetParam : IMiddleware<PipelineModel, Task>
{
    public async Task Invoke(PipelineModel input, PipelineMiddlewareDelegate<PipelineModel, Task> Next)
    {
        var paramManager = input.CommandScopeService.Service?.GetService<IParamManager>();
        if (paramManager != null)
            if (!await paramManager.Read(input.UserContext))
                return;

        await Next(input);
    }
}
