//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Abstract.Config;
using Telegram.Bot.Framework.UpdateTypeActions;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Bots
{
    /// <summary>
    /// 
    /// </summary>
    internal class TelegramBot : ITelegramBot
    {
        private readonly IServiceProvider ServiceProvider;
        private bool IsStop;
        public TelegramBot(IServiceProvider ServiceProvider)
        {
            // 获取IServiceCollection
            IServiceCollection services = ServiceProvider.GetService<IServiceCollection>();

            // 配置框架Service
            List<IConfig> configs = ServiceProvider.GetServices<IConfig>()?.ToList() ?? new();
            configs.ForEach(config => { config.ConfigureServices(services); });

            // 配置一些基本的设置
            services.AddSingleton<CancellationTokenSource>();
            services.AddSingleton<ITelegramBot>(this);
            services.AddSingleton<ITelegramBotClient, TelegramBotClient>();
            services.AddSingleton<IUpdateHandler, TelegramUpdateHandle>();
            
            // 创建服务
            this.ServiceProvider = services.BuildServiceProvider();
        }

        public async Task BotReStart()
        {
            await BotStop();
            await BotStart();
        }

        public async Task BotStart()
        {
            ITelegramBotClient botClient = ServiceProvider.GetService<ITelegramBotClient>();
            CancellationTokenSource cancellationTokenSource = ServiceProvider.GetService<CancellationTokenSource>();

            if (!await botClient.TestApiAsync(cancellationTokenSource.Token))
                throw new ArgumentException("API Error");

            botClient.StartReceiving(
                ServiceProvider.GetService<IUpdateHandler>(),
                new ReceiverOptions { AllowedUpdates = { } },
                cancellationTokenSource.Token
                );

            await Task.Run(async () => 
            {
                User user = await botClient.GetMeAsync(cancellationTokenSource.Token);
                Console.WriteLine($"Start {user.Username}");
                while (!IsStop)
                    await Task.Delay(1000);

                await botClient.CloseAsync();
            });
        }

        public async Task BotStop()
        {
            IsStop = true;

            await Task.CompletedTask;
        }
    }
}
