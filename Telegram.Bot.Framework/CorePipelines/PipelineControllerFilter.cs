using Azumo.Pipeline.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.CorePipelines
{
    internal class PipelineControllerFilter : IProcessAsync<TelegramUserChatContext>
    {
        public async Task<TelegramUserChatContext> ExecuteAsync(TelegramUserChatContext telegramUserChatContext, IPipelineController<TelegramUserChatContext> pipelineController)
        {
            var botCommand = telegramUserChatContext.Session.GetBotCommand();
            if (botCommand != null)
            {
                telegramUserChatContext.Session.SetBotCommand(botCommand);
                var controllerFilters = telegramUserChatContext.UserScopeService.GetServices<IControllerFilter>();
                foreach (var controllerFilter in controllerFilters)
                    if (await controllerFilter.Execute(telegramUserChatContext, botCommand))
                        return await pipelineController.StopAsync(telegramUserChatContext);
            }

            return await pipelineController.NextAsync(telegramUserChatContext);
        }
    }
}
