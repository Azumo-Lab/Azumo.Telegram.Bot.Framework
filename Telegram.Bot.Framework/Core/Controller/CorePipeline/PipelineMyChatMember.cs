using Azumo.SuperExtendedFramework.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.Controller.CorePipeline.Model;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Controller.CorePipeline;
internal class PipelineMyChatMember : IMiddleware<PipelineModel, Task>
{
    public async Task Invoke(PipelineModel input, PipelineMiddlewareDelegate<PipelineModel, Task> Next)
    {
        ChatMemberUpdated? chatMemberUpdated;
        if ((chatMemberUpdated = input.UserContext.MyChatMember) == null)
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
