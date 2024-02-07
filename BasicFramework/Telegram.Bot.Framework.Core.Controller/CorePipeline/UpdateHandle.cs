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

using Azumo.SuperExtendedFramework.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Controller.CorePipeline.Model;
using Telegram.Bot.Framework.Core.Users;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Controller.CorePipeline;

[DependencyInjection(ServiceLifetime.Singleton, ServiceType = typeof(IUpdateHandler))]
internal class UpdateHandle(IServiceProvider serviceProvider) : IUpdateHandler
{
    private readonly IServiceProvider BotServiceProvider = serviceProvider;

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken) => await Task.CompletedTask;

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var contextFactory = BotServiceProvider.GetRequiredService<IContextFactory>();
        TelegramUserContext? context;
        if ((context = contextFactory.GetOrCreateUserContext(BotServiceProvider, update)) == null)
            return;

        var pipeline = context.UserServiceProvider.GetRequiredService<IPipelineController<PipelineModel, Task>>();

        try
        {
            await pipeline[update.Type].Invoke(new PipelineModel
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
