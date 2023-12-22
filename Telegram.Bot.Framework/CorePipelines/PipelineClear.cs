using Azumo.Pipeline.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.CorePipelines
{
    internal class PipelineClear : IProcessAsync<TelegramUserChatContext>
    {
        public Task<TelegramUserChatContext> ExecuteAsync(TelegramUserChatContext telegramUserChatContext, IPipelineController<TelegramUserChatContext> pipelineController)
        {
            telegramUserChatContext.Session.RemoveBotCommand();

            return pipelineController.NextAsync(telegramUserChatContext);
        }
    }
}
