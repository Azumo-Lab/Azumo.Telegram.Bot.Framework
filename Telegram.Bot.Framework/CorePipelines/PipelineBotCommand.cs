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
    internal class PipelineBotCommand : IProcessAsync<TelegramUserChatContext>
    {
        public Task<TelegramUserChatContext> ExecuteAsync(TelegramUserChatContext telegramUserChatContext, IPipelineController<TelegramUserChatContext> pipelineController)
        {
            var controllerManager = telegramUserChatContext.UserScopeService.GetRequiredService<IControllerManager>();
            var botCommand = controllerManager.GetCommand(telegramUserChatContext);
            if (botCommand != null)
                telegramUserChatContext.Session.SetBotCommand(botCommand);

            return pipelineController.NextAsync(telegramUserChatContext);
        }
    }
}
