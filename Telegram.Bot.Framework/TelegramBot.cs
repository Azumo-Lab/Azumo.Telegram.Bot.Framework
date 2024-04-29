//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
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
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Telegram.Bot.Framework.Core.BotBuilder;
using Telegram.Bot.Framework.Core.PipelineMiddleware;
using Telegram.Bot.Framework.InternalCore.Attritubes;
using Telegram.Bot.Framework.InternalCore.StaticI18N;

namespace Telegram.Bot.Framework;

/// <summary>
/// Telegram Bot实现
/// </summary>
/// <remarks>
/// 机器人，实现了机器人接口，用于实现Bot启动，使用 <see cref="ITelegramBot.StartAsync(bool)"/> 方法来进行启动，
/// 启动之前需要对机器人进行一系列的配置。<br></br><br></br>
/// 机器人接口 <see cref="ITelegramBot"/><br></br><br></br>
/// 程序模块接口 <see cref="ITelegramModule"/> 用于实现最基础的应用 <br></br><br></br>
/// 模块构建接口 <see cref="ITelegramModuleBuilder"/> 实现了构建功能 <br></br><br></br>
/// </remarks>
[DebuggerDisplay("{RuntimeServiceProvider}")]
public class TelegramBot : ITelegramBot, ITelegramModuleBuilder, ITelegramModule
{
    #region 基础实例变量

    /// <summary>
    /// 添加程序模块
    /// </summary>
    private readonly List<ITelegramModule> telegramModules = [];

    /// <summary>
    /// 程序创建时期的服务集合
    /// </summary>
    private readonly IServiceCollection BuildServiceCollection = new ServiceCollection();

    /// <summary>
    /// 程序执行时的服务提供者
    /// </summary>
    private IServiceProvider RuntimeServiceProvider = null!;

    /// <summary>
    /// 程序执行日志
    /// </summary>
    private ILogger<TelegramBot>? Logger;

    #endregion

    #region 常量

    /// <summary>
    /// 这个框架的字符LOGO
    /// </summary>
    private const string LogoType3 = @"
████████╗███████╗██╗     ███████╗ ██████╗ ██████╗  █████╗ ███╗   ███╗   ██████╗  ██████╗ ████████╗
╚══██╔══╝██╔════╝██║     ██╔════╝██╔════╝ ██╔══██╗██╔══██╗████╗ ████║   ██╔══██╗██╔═══██╗╚══██╔══╝
   ██║   █████╗  ██║     █████╗  ██║  ███╗██████╔╝███████║██╔████╔██║   ██████╔╝██║   ██║   ██║   
   ██║   ██╔══╝  ██║     ██╔══╝  ██║   ██║██╔══██╗██╔══██║██║╚██╔╝██║   ██╔══██╗██║   ██║   ██║   
   ██║   ███████╗███████╗███████╗╚██████╔╝██║  ██║██║  ██║██║ ╚═╝ ██║██╗██████╔╝╚██████╔╝   ██║   
   ╚═╝   ╚══════╝╚══════╝╚══════╝ ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝     ╚═╝╚═╝╚═════╝  ╚═════╝    ╚═╝   
                                                                                                  
   ███████╗██████╗  █████╗ ███╗   ███╗███████╗██╗    ██╗ ██████╗ ██████╗ ██╗  ██╗                 
   ██╔════╝██╔══██╗██╔══██╗████╗ ████║██╔════╝██║    ██║██╔═══██╗██╔══██╗██║ ██╔╝                 
   █████╗  ██████╔╝███████║██╔████╔██║█████╗  ██║ █╗ ██║██║   ██║██████╔╝█████╔╝                  
   ██╔══╝  ██╔══██╗██╔══██║██║╚██╔╝██║██╔══╝  ██║███╗██║██║   ██║██╔══██╗██╔═██╗                  
██╗██║     ██║  ██║██║  ██║██║ ╚═╝ ██║███████╗╚███╔███╔╝╚██████╔╝██║  ██║██║  ██╗                 
╚═╝╚═╝     ╚═╝  ╚═╝╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝ ╚══╝╚══╝  ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝                 
";
    private const string License =
@"
<Telegram.Bot.Framework>
Copyright (C) <2022 - {A0}>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>

This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <https://www.gnu.org/licenses/>.
";
    #endregion

    #region 创建与销毁

    /// <summary>
    /// 初始化
    /// </summary>
    private TelegramBot() =>
        AddModule(this);

    /// <summary>
    /// 创建一个构建器
    /// </summary>
    /// <returns></returns>
    public static ITelegramModuleBuilder CreateBuilder() =>
        new TelegramBot();

    /// <summary>
    /// 销毁
    /// </summary>
    public void Dispose()
    {
        StopAsync().Wait();
        (RuntimeServiceProvider as IDisposable)?.Dispose();
        GC.SuppressFinalize(this);
    }

    #endregion

    /// <summary>
    /// 开始启动Bot
    /// </summary>
    /// <param name="wait">是否阻塞执行</param>
    /// <returns>异步任务</returns>
    public async Task StartAsync(bool wait = false)
    {
        ArgumentNullException.ThrowIfNull(RuntimeServiceProvider, nameof(RuntimeServiceProvider));

        Logger?.LogInformation("{A0}", LogoType3);
        Logger?.LogInformation(License, DateTime.Now.Year);

        // 执行前处理
        await PipelineProc<TelegramBotStartProcAttribute>();
        // 启动处理
        await PipelineProc<TelegramBotProcAttribute>();
        // 执行后处理
        await PipelineProc<TelegramBotEndProcAttribute>();

        if (wait)
        {
            var token = RuntimeServiceProvider.GetRequiredService<CancellationTokenSource>();
            while (!token.Token.IsCancellationRequested)
                await Task.Delay(TimeSpan.FromMinutes(1), token.Token);
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
        foreach (var (classType, attributes) in typeof(T).GetTypesWithAttribute())
            if (ActivatorUtilities.CreateInstance(RuntimeServiceProvider, classType, []) is IMiddleware<IServiceProvider, Task> middleware)
                _ = builder.Use(middleware);
        var controller = builder.Build();
        await controller.CurrentPipeline.Invoke(RuntimeServiceProvider);
    }

    /// <summary>
    /// 停止执行
    /// </summary>
    /// <returns>异步任务</returns>
    public async Task StopAsync()
    {
        try
        {
            RuntimeServiceProvider.GetRequiredService<CancellationTokenSource>().CancelAfter(TimeSpan.FromSeconds(5));
            var botClient = RuntimeServiceProvider.GetRequiredService<ITelegramBotClient>();
            await botClient.CloseAsync(RuntimeServiceProvider.GetRequiredService<CancellationTokenSource>().Token);
        }
        catch (Exception)
        {

        }
    }

    #region ITelegramModuleBuilder 模块，构建服务

    /// <summary>
    /// 添加模块
    /// </summary>
    /// <param name="module">模块</param>
    /// <returns>返回构建器</returns>
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

    #endregion

    #region ITelegramModule 模块，用于初始化时候的基础服务构建

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    public void AddBuildService(IServiceCollection services)
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="builderService"></param>
    public void Build(IServiceCollection services, IServiceProvider builderService)
    {

    }

    #endregion
}
