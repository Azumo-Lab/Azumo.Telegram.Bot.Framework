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

using Azumo.Pipeline.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts.CorePipeline
{
    internal class PipelineControllerInvoke : IProcessAsync<TGChat>
    {
        public async Task<TGChat> ExecuteAsync(TGChat t, IPipelineController<TGChat> pipelineController)
        {
            IControllerManager controllerManager = t.UserService.GetRequiredService<IControllerManager>();
            BotCommand botCommand = controllerManager.GetCommand(t);

            IControllerParamManager controllerParamManager = t.UserService.GetRequiredService<IControllerParamManager>();
            if (botCommand != null)
            {
                controllerParamManager.Clear();
                controllerParamManager.SetBotCommand(botCommand);
                controllerParamManager.ControllerParams = new List<IControllerParam>(botCommand.ControllerParams);
            }
            ResultEnum resultEnum = await controllerParamManager.NextParam(t);
            if (resultEnum != ResultEnum.Finish)
                return await pipelineController.StopAsync(t);

            botCommand ??= controllerParamManager.GetBotCommand();
            if (botCommand == null)
                return await pipelineController.StopAsync(t);

            try
            {
                TelegramController telegramController = (TelegramController)ActivatorUtilities.CreateInstance(t.UserService, botCommand!.Controller, Array.Empty<object>());
                await telegramController.ControllerInvokeAsync(t, botCommand.Func, controllerParamManager);
            }
            catch (Exception)
            {

            }

            return await pipelineController.NextAsync(t);
        }
    }
}
