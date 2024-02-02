using Azumo.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Controller.CorePipeline.Model;
using Telegram.Bot.Framework.Core.Storage;
using Telegram.Bot.Framework.Core.Users;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Core.Controller.CorePipeline;
internal class UpdateHandle(IServiceProvider serviceProvider) : IUpdateHandler
{
    private readonly IServiceProvider BotServiceProvider = serviceProvider;

    public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {

        await Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var contextFactory = BotServiceProvider.GetRequiredService<IContextFactory>();
        TelegramUserContext? context;
        if ((context = contextFactory.GetOrCreateUserContext(BotServiceProvider, update)) == null)
            return;

        var pipeline = context.UserServiceProvider.GetRequiredService<IPipelineController<PipelineModel>>();

        try
        {
            await pipeline.Execute("", new PipelineModel
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
