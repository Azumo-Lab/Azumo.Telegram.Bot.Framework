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

using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics;
using Telegram.Bot.Framework.Core.BotBuilder;
using Telegram.Bot.Polling;

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
    private readonly CancellationTokenSource _tokenSource = new();

    /// <summary>
    /// 
    /// </summary>
    private IServiceProvider RuntimeServiceProvider = null!;

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
    public ITelegramModuleBuilder AddModule(ITelegramModule module)
    {
        telegramModules.Add(module);
        return this;
    }

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

        RuntimeServiceProvider = runtimeServiceCollection.BuildServiceProvider();
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
    /// <exception cref="Exception"></exception>
    public async Task StartAsync()
    {
        ArgumentNullException.ThrowIfNull(RuntimeServiceProvider, nameof(RuntimeServiceProvider));

        var botClient = RuntimeServiceProvider.GetRequiredService<ITelegramBotClient>();

        if (!await botClient.TestApiAsync(_tokenSource.Token))
            throw new Exception();

        botClient.StartReceiving(RuntimeServiceProvider.GetRequiredService<IUpdateHandler>(),
            new ReceiverOptions
            {
                AllowedUpdates = []
            },
            _tokenSource.Token);

        var user = await botClient.GetMeAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public async Task StopAsync()
    {
        var botClient = RuntimeServiceProvider.GetRequiredService<ITelegramBotClient>();
        await botClient.CloseAsync(_tokenSource.Token);
    }
}
