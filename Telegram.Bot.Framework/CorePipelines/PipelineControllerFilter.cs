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
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.CorePipelines
{
    internal class PipelineControllerFilter : IProcessAsync<TelegramUserChatContext>
    {
        public async Task<TelegramUserChatContext> ExecuteAsync(TelegramUserChatContext telegramUserChatContext, IPipelineController<TelegramUserChatContext> pipelineController)
        {
            var controllerManager = telegramUserChatContext.UserScopeService.GetRequiredService<IControllerManager>();
            var botCommand = controllerManager.GetCommand(telegramUserChatContext);

            if (botCommand != null)
            {
                foreach (var controllerFilter in telegramUserChatContext.UserScopeService.GetServices<IControllerFilter>() ?? [])
                    if (!await controllerFilter.Execute(telegramUserChatContext, botCommand))
                        return await pipelineController.StopAsync(telegramUserChatContext);

                var controllerParamManager = telegramUserChatContext.UserScopeService.GetRequiredService<IControllerParamManager>();
                controllerParamManager.NewBotCommandParamScope(botCommand);
            }

            return await pipelineController.NextAsync(telegramUserChatContext);
        }
    }
}
