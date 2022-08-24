using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Framework.DependencyInjection;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    public class TelegramBot
    {
        private readonly IServiceCollection telegramServiceCollection;
        private readonly IServiceProvider serviceProvider;
        private readonly IServiceProviderBuild serviceProviderBuild;

        private readonly TelegramBotClient botClient;

        internal TelegramBot(TelegramBotClient botClient, ISetUp setUp)
        {
            telegramServiceCollection = new TelegramServiceCollection();

            new BaseSetUp(new List<ISetUp>() { setUp }).Config(telegramServiceCollection);

            serviceProviderBuild = (IServiceProviderBuild)telegramServiceCollection;

            this.botClient = botClient;

            serviceProvider = serviceProviderBuild.Build();
        }

        public void Start()
        {
            Task.Run(async () =>
            {
                async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
                {
                    TelegramContext telegramContext = new TelegramContext(botClient, update, cancellationToken);

                    TelegramRouteController process = new TelegramRouteController(telegramContext, serviceProvider);

                    await process.StartProcess();
                }

                Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
                {
                    var ErrorMessage = exception switch
                    {
                        ApiRequestException apiRequestException
                            => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                        _ => exception.ToString()
                    };

                    Console.WriteLine(ErrorMessage);
                    return Task.CompletedTask;
                }

                using (var cts = new CancellationTokenSource())
                {
                    var receiverOptions = new ReceiverOptions
                    {
                        AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
                    };
                    botClient.StartReceiving(
                        updateHandler: HandleUpdateAsync,
                        pollingErrorHandler: HandlePollingErrorAsync,
                        receiverOptions: receiverOptions,
                        cancellationToken: cts.Token
                    );

                    var me = await botClient.GetMeAsync();

                    Console.WriteLine($"Start listening for @{me.Username}");
                    Console.ReadLine();

                    // Send cancellation request to stop bot
                    cts.Cancel();
                }
            });
        }
    }
}
