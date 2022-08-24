using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    public class TelegramBot
    {
        private readonly IServiceCollection telegramServiceCollection;
        private readonly IServiceProvider serviceProvider;

        private readonly TelegramBotClient botClient;

        internal TelegramBot(TelegramBotClient botClient, ISetUp setUp)
        {
            telegramServiceCollection = new ServiceCollection();

            new BaseSetUp(new List<ISetUp>() { setUp }).Config(telegramServiceCollection);
            var factory = new DefaultServiceProviderFactory();
            serviceProvider = factory.CreateServiceProvider(telegramServiceCollection);

            this.botClient = botClient;
        }

        public void Start()
        {
            Task.Run(async () =>
            {
                async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
                {
                    using (IServiceScope service = serviceProvider.CreateScope())
                    {
                        TelegramContext telegramContext = new TelegramContext(botClient, update, cancellationToken);

                        TelegramRouteController process = new TelegramRouteController(telegramContext, service.ServiceProvider);

                        await process.StartProcess();
                    }
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
            Console.ReadLine();
        }
    }
}
