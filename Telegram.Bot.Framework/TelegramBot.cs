//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using Azumo.Pipeline.Abstracts;
using System.Diagnostics;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Abstracts.Exec;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework
{
    /// <summary>
    /// 这个是一个机器人接口： <see cref="ITelegramBot"/> 接口的实现类
    /// </summary>
    [DebuggerDisplay("机器人：@{__BotUsername}")]
    internal class TelegramBot : ITelegramBot, IUpdateHandler, IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        private string __BotUsername;

        /// <summary>
        /// 机器人的服务
        /// </summary>
        private readonly IServiceProvider ServiceProvider;

        /// <summary>
        /// 
        /// </summary>
        private readonly ILogger __log;

        /// <summary>
        /// 
        /// </summary>
        private bool __IsEnd;

        /// <summary>
        /// 项目的启动LOGO
        /// </summary>
        private static readonly string LOGO =
@"
████████╗███████╗██╗     ███████╗ ██████╗ ██████╗  █████╗ ███╗   ███╗   ██████╗  ██████╗ ████████╗
╚══██╔══╝██╔════╝██║     ██╔════╝██╔════╝ ██╔══██╗██╔══██╗████╗ ████║   ██╔══██╗██╔═══██╗╚══██╔══╝
   ██║   █████╗  ██║     █████╗  ██║  ███╗██████╔╝███████║██╔████╔██║   ██████╔╝██║   ██║   ██║   
   ██║   ██╔══╝  ██║     ██╔══╝  ██║   ██║██╔══██╗██╔══██║██║╚██╔╝██║   ██╔══██╗██║   ██║   ██║   
   ██║   ███████╗███████╗███████╗╚██████╔╝██║  ██║██║  ██║██║ ╚═╝ ██║██╗██████╔╝╚██████╔╝   ██║██╗
   ╚═╝   ╚══════╝╚══════╝╚══════╝ ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝     ╚═╝╚═╝╚═════╝  ╚═════╝    ╚═╝╚═╝
                                                                                                  
███████╗██████╗  █████╗ ███╗   ███╗███████╗██╗    ██╗ ██████╗ ██████╗ ██╗  ██╗                    
██╔════╝██╔══██╗██╔══██╗████╗ ████║██╔════╝██║    ██║██╔═══██╗██╔══██╗██║ ██╔╝                    
█████╗  ██████╔╝███████║██╔████╔██║█████╗  ██║ █╗ ██║██║   ██║██████╔╝█████╔╝                     
██╔══╝  ██╔══██╗██╔══██║██║╚██╔╝██║██╔══╝  ██║███╗██║██║   ██║██╔══██╗██╔═██╗                     
██║     ██║  ██║██║  ██║██║ ╚═╝ ██║███████╗╚███╔███╔╝╚██████╔╝██║  ██║██║  ██╗                    
╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝ ╚══╝╚══╝  ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝                    
";

        /// <summary>
        /// 
        /// </summary>
        private readonly IUpdateHandler __UpdateHandler;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ServiceProvider"></param>
        public TelegramBot(IServiceProvider ServiceProvider)
        {
            this.ServiceProvider = ServiceProvider;

            __log = this.ServiceProvider.GetService<ILogger<TelegramBot>>();
            __UpdateHandler = this.ServiceProvider.GetService<IUpdateHandler>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await __UpdateHandler.HandlePollingErrorAsync(botClient, exception, cancellationToken);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            await __UpdateHandler.HandleUpdateAsync(botClient, update, cancellationToken);
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <returns></returns>
        public async Task StartAsync()
        {
            __log.LogInformation(LOGO);
            __log.LogInformation("启动中...");

            try
            {
                // 获取 ITelegramBotClient 接口(基础环境)
                ITelegramBotClient telegramBot = ServiceProvider.GetService<ITelegramBotClient>();
                if (telegramBot == null)
                    throw new NullReferenceException(nameof(telegramBot));

                // 在程序执行之前，开始执行任务
                List<IStartExec> startExecs = ServiceProvider.GetServices<IStartExec>().ToList();
                if (startExecs.Count != 0)
                {
                    __log.LogInformation("发现 {A0} 个任务...开始执行", startExecs.Count);
                    foreach (IStartExec execs in startExecs)
                    {
                        if (execs is IPipelineName pipelineName)
                            __log.LogInformation($"当前正在执行 ‘{pipelineName.Name}’");
                        await execs.Exec(telegramBot, ServiceProvider);
                    }
                    __log.LogInformation($"{startExecs.Count} 个任务执行完毕");
                }

                // 开始启动监听
                telegramBot.StartReceiving(this);

                // 获取机器人自己的用户名(作为一个连接测试)
                User user = await telegramBot.GetMeAsync();
                __BotUsername = user.Username;
                __log.LogInformation(message: $"机器人用户 @{user.Username} 正在运行中...");

                List<IExec> exec = ServiceProvider.GetServices<IExec>().ToList();
                foreach (IExec item in exec)
                    _ = item.Execute().ConfigureAwait(false);

                // 死循环，一直等待
                while (!__IsEnd)
                {
                    if (__IsEnd)
                        await telegramBot.CloseAsync();
                    await Task.Delay(500);
                }
            }
            catch (Exception ex)
            {
                __log.LogError("程序启动发生致命错误");
                __log.LogError(ex.ToString());
            }
        }

        /// <summary>
        /// 停止执行
        /// </summary>
        /// <returns></returns>
        public Task StopAsync()
        {
            __IsEnd = true;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            StopAsync().Wait();
            (ServiceProvider as IDisposable)?.Dispose();
        }
    }
}
