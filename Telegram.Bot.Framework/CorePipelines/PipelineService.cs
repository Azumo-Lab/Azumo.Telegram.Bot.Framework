using Azumo.Pipeline;
using Azumo.Pipeline.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.CorePipelines
{
    [DependencyInjection(ServiceLifetime.Singleton, typeof(ITelegramService))]
    internal class PipelineService : ITelegramService
    {
        public void AddServices(IServiceCollection services) =>
            services.AddScoped(x => PipelineFactory.CreateIPipelineBuilder<TelegramUserChatContext>()

            .AddProcedure(new PipelineBotCommand())
            .AddProcedure(new PipelineControllerFilter())
            .AddProcedure(new PipelineControllerInvoker())
            .AddProcedure(new PipelineClear())

            .CreatePipeline(UpdateType.Message)

            .BuilderPipelineController());
    }
}
