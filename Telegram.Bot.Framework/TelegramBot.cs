using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Abstracts.Controller;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Framework.Pipeline.Abstracts;
using Telegram.Bot.Framework.Pipelines;
using Telegram.Bot.Framework.Reflections;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    internal class TelegramBot : ITelegramBot, IUpdateHandler
    {
        private readonly IServiceProvider ServiceProvider;
        private readonly ILogger __log;

        private readonly IPipelineController<TGChat> pipelineController;

        private bool __IsEnd;

        public TelegramBot(IServiceProvider ServiceProvider)
        {
            this.ServiceProvider = ServiceProvider;

            __log = this.ServiceProvider.GetService<ILogger<TelegramBot>>();

            pipelineController = PipelineFactory.CreateIPipelineBuilder<TGChat>()
                .AddProcedure(new ProcessControllerInvoke())
                .CreatePipeline(UpdateType.Message)
                .BuilderPipelineController();
        }

        public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            IChatManager chatManager = ServiceProvider.GetService<IChatManager>();

            TGChat chat = chatManager.Create(botClient, update, ServiceProvider);

            await pipelineController.SwitchTo(update.Type, chat);
        }

        public async Task StartAsync()
        {
            __log.LogInformation("Start...");
            ITelegramBotClient telegramBot = ServiceProvider.GetService<ITelegramBotClient>();
            telegramBot.StartReceiving(this);

            User user = await telegramBot.GetMeAsync();
            __log.LogInformation(message: $"@{user.Username} is Running...");

            while (!__IsEnd)
            {
                if (__IsEnd)
                    await telegramBot.CloseAsync();
            }
        }

        public Task StopAsync()
        {
            __IsEnd = true;
            return Task.CompletedTask;
        }
    }
}
