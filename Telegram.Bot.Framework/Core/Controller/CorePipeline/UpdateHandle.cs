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

using Azumo.SuperExtendedFramework.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Controller.CorePipeline.Model;
using Telegram.Bot.Framework.Core.Users;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Controller.CorePipeline;

/// <summary>
/// 
/// </summary>
/// <param name="serviceProvider"></param>
[DependencyInjection(ServiceLifetime.Singleton, ServiceType = typeof(IUpdateHandler))]
internal class UpdateHandle(IServiceProvider serviceProvider) : IUpdateHandler
{
    /// <summary>
    /// 
    /// </summary>
    private readonly IServiceProvider BotServiceProvider = serviceProvider;

    /// <summary>
    /// 
    /// </summary>
    private readonly ILogger<UpdateHandle>? _logger = serviceProvider.GetService<ILogger<UpdateHandle>>();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="botClient"></param>
    /// <param name="exception"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        // 发生错误，将信息写入Log中
        _logger?.LogError("发生错误，错误类型：{A0}，错误信息：{A1}", exception.GetType().FullName, exception.ToString());
        await Task.CompletedTask;
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
        // 创建 TelegramUserContext
        var contextFactory = BotServiceProvider.GetRequiredService<IContextFactory>();
        TelegramUserContext? context;
        if ((context = contextFactory.GetOrCreateUserContext(BotServiceProvider, update)) == null)
            return;

        try
        {
            // 获取用户的流水线
            var pipeline = context.UserServiceProvider.GetRequiredService<IPipelineController<PipelineModel, Task>>();

            // 执行流水线
            await pipeline[context.Type].Invoke(new PipelineModel
            {
                UserContext = context,
                CommandManager = BotServiceProvider.GetRequiredService<ICommandManager>(),
                CommandScopeService = context.UserServiceProvider.GetRequiredService<ICommandScopeService>(),
            });
        }
        catch (Exception ex)
        {
            await HandlePollingErrorAsync(botClient, ex, cancellationToken);
        }
        finally
        {

        }
    }
}
