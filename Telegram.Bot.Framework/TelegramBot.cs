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
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Framework.InternalFramework;
using Telegram.Bot.Framework.InternalFramework.InterFaces;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework
{
    public class TelegramBot
    {
        private readonly IServiceCollection telegramServiceCollection;
        private readonly IServiceProvider serviceProvider;

        private readonly string BotName;
        private readonly bool UseBotName = false;

        internal TelegramBot(TelegramBotClient botClient, IConfig setUp, string BotName = null)
        {
            this.BotName = BotName;
            if (!string.IsNullOrEmpty(this.BotName))
                UseBotName = true;

            telegramServiceCollection = new ServiceCollection();

            new FrameworkConfig(new List<IConfig>() { setUp }, UseBotName, botClient).Config(telegramServiceCollection);

            serviceProvider = new DefaultServiceProviderFactory().CreateServiceProvider(telegramServiceCollection);
        }

        public void Start()
        {
            Task.Run(async () =>
            {
                CancellationTokenSource cts = serviceProvider.GetService<CancellationTokenSource>();
                ITelegramBotClient botClient = serviceProvider.GetService<ITelegramBotClient>();

                ReceiverOptions receiverOptions = new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>() // receive all update types
                };
                IUpdateHandler updateHandler = serviceProvider.GetService<IUpdateHandler>();
                botClient.StartReceiving(
                    updateHandler: updateHandler.HandleUpdateAsync,
                    pollingErrorHandler: updateHandler.HandlePollingErrorAsync,
                    receiverOptions: receiverOptions,
                    cancellationToken: cts.Token
                );

                User me = await botClient.GetMeAsync();

                Console.WriteLine($"Start listening for @{me.Username}");
                Console.ReadLine();

                // Send cancellation request to stop bot
                cts.Cancel();
            });
            Console.ReadLine();
        }
    }
}
