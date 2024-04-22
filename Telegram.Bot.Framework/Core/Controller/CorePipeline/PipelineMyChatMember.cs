using Azumo.SuperExtendedFramework.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.Controller.CorePipeline.Model;

namespace Telegram.Bot.Framework.Core.Controller.CorePipeline;
internal class PipelineMyChatMember : IMiddleware<PipelineModel, Task>
{
    public async Task Invoke(PipelineModel input, PipelineMiddlewareDelegate<PipelineModel, Task> Next)
    {
        foreach (var item in input.UserContext.UserServiceProvider.GetServices<IChatMemberChange>())
            await item.ChatMemberChangeAsync(input.UserContext, input.UserContext.MyChatMember!.NewChatMember,
                input.UserContext.MyChatMember.From.Id, input.UserContext.MyChatMember.Chat.Id);

        await Next(input);
    }
}
