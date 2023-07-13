﻿//  <Telegram.Bot.Framework>
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
using Telegram.Bot.Framework.Abstract.BackgroundProcess;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Abstract.Config;
using Telegram.Bot.Framework.ExtensionMethods;
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
        #region 私有变量

        /// <summary>
        /// 由外部注入
        /// </summary>
        private readonly IServiceProvider ServiceProvider;

        /// <summary>
        /// 判断当前机器人是否停止
        /// </summary>
        /// <remarks>
        /// 在 <see cref="BotStart(bool)"/> 和 <see cref="BotStop"/> 中有所使用
        /// </remarks>
        private bool IsStop;

        /// <summary>
        /// 是否要异步堵塞的一个标志物
        /// </summary>
        private bool awaitFlag;

        private bool disposedValue;

        private const string AzumoLab_Logo =
@"
                                        _           _                                                                  
     /\                                | |         | |                                                                 
    /  \    _____   _ _ __ ___   ___   | |     __ _| |__                                                               
   / /\ \  |_  / | | | '_ ` _ \ / _ \  | |    / _` | '_ \                                                              
  / ____ \  / /| |_| | | | | | | (_) | | |___| (_| | |_) |                                                             
 /_/    \_\/___|\__,_|_| |_| |_|\___/  |______\__,_|_.__/                                                              
  _______   _                                  ____        _     ______                                           _    
 |__   __| | |                                |  _ \      | |   |  ____|                                         | |   
    | | ___| | ___  __ _ _ __ __ _ _ __ ___   | |_) | ___ | |_  | |__ _ __ __ _ _ __ ___   _____      _____  _ __| | __
    | |/ _ \ |/ _ \/ _` | '__/ _` | '_ ` _ \  |  _ < / _ \| __| |  __| '__/ _` | '_ ` _ \ / _ \ \ /\ / / _ \| '__| |/ /
    | |  __/ |  __/ (_| | | | (_| | | | | | |_| |_) | (_) | |_ _| |  | | | (_| | | | | | |  __/\ V  V / (_) | |  |   < 
    |_|\___|_|\___|\__, |_|  \__,_|_| |_| |_(_)____/ \___/ \__(_)_|  |_|  \__,_|_| |_| |_|\___| \_/\_/ \___/|_|  |_|\_\
                    __/ |                                                                                              
                   |___/                                                                                               ";
        #endregion

        #region 公开的变量属性

        /// <summary>
        /// 这个机器人用户的信息
        /// </summary>
        public User ThisBot { get; private set; }

        /// <summary>
        /// Bot的一些信息
        /// </summary>
        public IBotInfo BotInfo { get; private set; }

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ServiceProvider"></param>
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
            Services.AddSingleton(BotInfo = ServiceProvider.GetService<IBotInfo>());

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
            await Task.Delay(2000);
            // 重启
            await BotStart();
        }

        /// <summary>
        /// 启动机器人
        /// </summary>
        /// <remarks>
        /// 机器人启动的时候，可以配置 <paramref name="awaitFlag"/> 参数 <br/>
        /// 当 <paramref name="awaitFlag"/> = true 时候，执行 <c>task.Wait()</c> 时候，会一直等待整个机器人结束执行。<br/>
        /// 当 <paramref name="awaitFlag"/> = false 时候，执行 <c>task.Wait()</c> 时候，机器人启动完毕就会继续执行后面的代码
        /// </remarks>
        /// <param name="awaitFlag">是否可等待</param>
        /// <returns>可等待任务</returns>
        /// <exception cref="ArgumentException">API 配置不对的话会触发</exception>
        public async Task BotStart(bool awaitFlag = true)
        {
            // 是否可能等待的值
            this.awaitFlag = awaitFlag;
            // 清除旧的控制台信息
            ConsoleHelper.Clear();
            // 输出Logo图像
            ConsoleHelper.WriteLine(AzumoLab_Logo);
            // 设置全局异常处理
            AppDomain.CurrentDomain.UnhandledException += GlobalExceptionHandling;

            // 程序启动前执行的任务
            await StartExec<IStartBeforeExec>();

            // 获取<ITelegramBotClient>接口
            ITelegramBotClient botClient = ServiceProvider.GetService<ITelegramBotClient>();
            CancellationTokenSource cancellationTokenSource = ServiceProvider.GetService<CancellationTokenSource>();

            // 测试API能否连接
            if (!await botClient.TestApiAsync(cancellationTokenSource.Token))
                throw new ArgumentException("API Error");
            
            // 开始启动
            botClient.StartReceiving(
                ServiceProvider.GetService<IUpdateHandler>(),
                ServiceProvider.GetService<ReceiverOptions>() ?? new ReceiverOptions { AllowedUpdates = { } },
                cancellationTokenSource.Token
                );

            // 获取机器人自身
            ThisBot = await botClient.GetMeAsync(cancellationTokenSource.Token);
            ConsoleHelper.Info($"Start OK @{ThisBot.Username}");

            // 程序启动后执行的任务
            await StartExec<IStartAfterExec>();
            // 程序执行的定时任务
            await StartExec<ITimedTask>();

            // 阻塞等待
            await Task.Run(async () =>
            {
                // 如果可等待选项在这里是False，那么就不会堵塞运行
                if (awaitFlag)
                {
                    while (!IsStop)
                        await Task.Delay(1000);

                    await botClient.CloseAsync();
                }
            });
        }

        /// <summary>
        /// 启动实现了 <see cref="IExec"/> 接口的类
        /// </summary>
        /// <typeparam name="T">IExec接口</typeparam>
        /// <returns>异步执行任务</returns>
        private async Task StartExec<T>() where T : IExec
        {
            IEnumerable<T> iExecs = ServiceProvider.GetServices<T>();
            foreach (IExec exec in iExecs)
                await exec.Exec();
        }

        /// <summary>
        /// 一个全局异常处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GlobalExceptionHandling(object sender, UnhandledExceptionEventArgs e)
        {
            // 获取异常，并输出到控制台
            Exception ex;
            if ((ex = e.ExceptionObject as Exception) != null || (ex = sender as Exception) != null)
                ConsoleHelper.Error(ex.ToString());
            else
                ConsoleHelper.Error("Error");
        }

        /// <summary>
        /// 停止当前机器人的执行(非实时停止)
        /// </summary>
        /// <returns>可等待任务</returns>
        public async Task BotStop()
        {
            IsStop = true;
            if (!awaitFlag)
            {
                ITelegramBotClient botClient = ServiceProvider.GetService<ITelegramBotClient>();
                await botClient.CloseAsync();
            }
            await Task.CompletedTask;
        }

        protected virtual async void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // 释放托管状态(托管对象)
                    await BotStop();
                    (ServiceProvider as IDisposable)?.Dispose();
                }

                GC.Collect();
                // 释放未托管的资源(未托管的对象)并重写终结器
                // 将大型字段设置为 null
                disposedValue = true;
            }
        }

        // // 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
        // ~TelegramBot()
        // {
        //     // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
