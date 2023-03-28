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
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Abstract.Config;
using Telegram.Bot.Framework.MiddlewarePipelines;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 创建一个TelegramBot
    /// </summary>
    internal class TelegramBot : ITelegramBot
    {
        private readonly IServiceProvider ServiceProvider;
        private bool IsStop;
        public TelegramBot(IServiceProvider ServiceProvider)
        {
            // 获取IServiceCollection
            IServiceCollection Services = ServiceProvider.GetService<IServiceCollection>();

            // 配置框架Service
            List<IConfig> configs = ServiceProvider.GetServices<IConfig>()?.ToList() ?? new();
            configs.ForEach(config => { config.ConfigureServices(Services); });

            // 配置一些基本的设置
            Services.AddSingleton<CancellationTokenSource>();
            Services.AddSingleton<ITelegramBot>(this);
            Services.AddSingleton<IUpdateHandler, TelegramUpdateHandle>();

            // 创建服务
            this.ServiceProvider = Services.BuildServiceProvider();
        }

        /// <summary>
        /// 对当前Bot进行重启
        /// </summary>
        /// <returns>可等待任务</returns>
        public async Task BotReStart()
        {
            // 停止
            await BotStop();
            // 等待一段时间，等待停止指令的执行
            await Task.Delay(5000);
            // 重启
            await BotStart();
        }

        /// <summary>
        /// 启动机器人
        /// </summary>
        /// <returns>可等待任务</returns>
        /// <exception cref="ArgumentException">API 配置不对的话会触发</exception>
        public async Task BotStart()
        {
            ITelegramBotClient botClient = ServiceProvider.GetService<ITelegramBotClient>();
            CancellationTokenSource cancellationTokenSource = ServiceProvider.GetService<CancellationTokenSource>();

            if (!await botClient.TestApiAsync(cancellationTokenSource.Token))
                throw new ArgumentException("API Error");

            botClient.StartReceiving(
                ServiceProvider.GetService<IUpdateHandler>(),
                ServiceProvider.GetService<ReceiverOptions>() ?? new ReceiverOptions { AllowedUpdates = { } },
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

        /// <summary>
        /// 停止当前机器人的执行(非实时停止)
        /// </summary>
        /// <returns>可等待任务</returns>
        public async Task BotStop()
        {
            IsStop = true;

            await Task.CompletedTask;
        }
    }
}
