using Azumo.Pipeline;
using Azumo.Pipeline.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstracts.CorePipeline
{
    [DependencyInjection(ServiceLifetime.Singleton, typeof(ITelegramService))]
    internal class UserScope : ITelegramService
    {
        public static async Task Invoke(TGChat tGChat)
        {
            IPipelineController<TGChat> pipelineController = tGChat.UserService.GetRequiredService<IPipelineController<TGChat>>();
            await pipelineController.SwitchTo(tGChat.Type, tGChat);
        }

        public void AddServices(IServiceCollection services)
        {
            services.AddScoped(x =>
            {
                return PipelineFactory
                    .CreateIPipelineBuilder<TGChat>()
                    .AddProcedure(new PipelineNull())
                    .CreatePipeline(UpdateType.Unknown)
                    .CreatePipeline(UpdateType.Message)
                    .CreatePipeline(UpdateType.ChosenInlineResult)
                    .CreatePipeline(UpdateType.CallbackQuery)
                    .CreatePipeline(UpdateType.EditedMessage)
                    .CreatePipeline(UpdateType.ChannelPost)
                    .CreatePipeline(UpdateType.EditedChannelPost)
                    .CreatePipeline(UpdateType.ShippingQuery)
                    .CreatePipeline(UpdateType.PreCheckoutQuery)
                    .CreatePipeline(UpdateType.Poll)
                    .CreatePipeline(UpdateType.PollAnswer)
                    .CreatePipeline(UpdateType.MyChatMember)
                    .CreatePipeline(UpdateType.ChatMember)
                    .CreatePipeline(UpdateType.ChatJoinRequest)
                    .BuilderPipelineController();
            });
        }
    }
}
