//  < Telegram.Bot.Framework >
//  Copyright (C) <2022>  <Sokushu> see <https://github.com/sokushu/Telegram.Bot.Net/>
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Framework.InternalFramework;
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

        internal TelegramBot(TelegramBotClient botClient, IConfig setUp)
        {
            telegramServiceCollection = new ServiceCollection();

            new FrameworkConfig(new List<IConfig>() { setUp }).Config(telegramServiceCollection);
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
        }
    }
}
