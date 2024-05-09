using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Controller;
using Telegram.Bot.Framework.Core.PipelineMiddleware;
using Telegram.Bot.Framework.InternalCore.CorePipelines.Models;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalCore.CorePipelines.ControllerInvokePipeline
{
    internal class PipelineMyChatMember : IMiddleware<PipelineModel, Task>
    {
        public async Task Invoke(PipelineModel input, PipelineMiddlewareDelegate<PipelineModel, Task> Next)
        {
            ChatMemberUpdated? chatMemberUpdated;
            if ((chatMemberUpdated = input.UserContext!.MyChatMember) == null)
                return;

            try
            {
                foreach (var item in input.UserContext.UserServiceProvider.GetServices<IChatMemberChange>())
                    await item.ChatMemberChangeAsync(input.UserContext, chatMemberUpdated.NewChatMember, chatMemberUpdated.From.Id, chatMemberUpdated.Chat.Id);
            }
            catch (Exception)
            {

            }

            await Next(input);
        }
    }
}
