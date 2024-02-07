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

using Azumo.SuperExtendedFramework;
using Azumo.SuperExtendedFramework.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Telegram.Bot.Framework.Core.BotBuilder;
using Telegram.Bot.Framework.Internal;

namespace Telegram.Bot.Framework;

/// <summary>
/// 
/// </summary>
[DebuggerDisplay("{RuntimeServiceProvider}")]
public class TelegramBot : ITelegramBot, ITelegramModuleBuilder
{
    /// <summary>
    /// 
    /// </summary>
    private readonly List<ITelegramModule> telegramModules = [];

    /// <summary>
    /// 
    /// </summary>
    private readonly IServiceCollection BuildServiceCollection = new ServiceCollection();

    /// <summary>
    /// 
    /// </summary>
    private IServiceProvider RuntimeServiceProvider = null!;

    /// <summary>
    /// 
    /// </summary>
    private ILogger<TelegramBot>? Logger;

    /// <summary>
    /// 
    /// </summary>
    private TelegramBot() { }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static ITelegramModuleBuilder CreateBuilder() =>
        new TelegramBot();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="module"></param>
    /// <returns></returns>
    public ITelegramModuleBuilder AddModule(ITelegramModule? module)
    {
        ArgumentNullException.ThrowIfNull(module, nameof(module));

        telegramModules.Add(module);
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objects"></param>
    /// <returns></returns>
    /// <exception cref="NullReferenceException"></exception>
    public ITelegramModuleBuilder AddModule<T>(params object[] objects) where T : ITelegramModule =>
        AddModule((ITelegramModule?)Activator.CreateInstance(typeof(T), objects));

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public ITelegramBot Build()
    {
        foreach (var module in telegramModules)
            module.AddBuildService(BuildServiceCollection);

        var serviceProvider = BuildServiceCollection.BuildServiceProvider();
        var runtimeServiceCollection = new ServiceCollection();

        foreach (var module in telegramModules)
            module.Build(runtimeServiceCollection, serviceProvider);

        _ = runtimeServiceCollection.AddSingleton<CancellationTokenSource>();

        RuntimeServiceProvider = runtimeServiceCollection.BuildServiceProvider();
        Logger = RuntimeServiceProvider.GetService<ILogger<TelegramBot>>();
        return this;
    }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose()
    {
        StopAsync().Wait();
        (RuntimeServiceProvider as IDisposable)?.Dispose();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task StartAsync(bool wait = false)
    {
        ArgumentNullException.ThrowIfNull(RuntimeServiceProvider, nameof(RuntimeServiceProvider));
        
        // 执行前处理
        await PipelineProc<TelegramBotStartProcAttribute>();
        // 启动处理
        await PipelineProc<TelegramBotProcAttribute>();
        // 执行后处理
        await PipelineProc<TelegramBotEndProcAttribute>();

        if(wait)
        {
            var token = RuntimeServiceProvider.GetRequiredService<CancellationTokenSource>();
            while (!token.Token.IsCancellationRequested)
                await Task.Delay((int)TimeSpan.FromSeconds(0.5).TotalMilliseconds);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    private async Task PipelineProc<T>() where T : Attribute
    {
        var builder = PipelineFactory.GetPipelineBuilder<IServiceProvider, Task>(() => Task.CompletedTask);
        foreach (var item in typeof(T).GetHasAttributeType())
            if (ActivatorUtilities.CreateInstance(RuntimeServiceProvider, item.Item1, []) is IMiddleware<IServiceProvider, Task> middleware)
                _ = builder.Use(middleware);
        var controller = builder.Build();
        await controller.CurrentPipeline.Invoke(RuntimeServiceProvider);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task StopAsync()
    {
        var botClient = RuntimeServiceProvider.GetRequiredService<ITelegramBotClient>();
        await botClient.CloseAsync(RuntimeServiceProvider.GetRequiredService<CancellationTokenSource>().Token);
    }
}
