//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2025>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller;
using Telegram.Bot.Framework.PipelineMiddleware;

namespace Telegram.Bot.Framework.CorePipelines
{
    /// <summary>
    /// 
    /// </summary>
    internal class ControllerPipelineGetCommand : IMiddleware<TelegramActionContext, Task>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="Next"></param>
        /// <returns></returns>
        public async Task Execute(TelegramActionContext input, PipelineMiddlewareDelegate<TelegramActionContext, Task> Next)
        {
            var commandManager = input.ServiceProvider.GetRequiredService<ICommandManager>();
            var exec = commandManager.GetExecutor(input.TelegramRequest);

            if (exec != null)
            {
                input.CommandScopeService.DeleteOldCreateNew();

                input.CommandScopeService.Executor = exec;
                input.CommandScopeService.ParamManager.SetParamList(exec.Parameters);
            }

            await Next(input);
        }
    }
}
