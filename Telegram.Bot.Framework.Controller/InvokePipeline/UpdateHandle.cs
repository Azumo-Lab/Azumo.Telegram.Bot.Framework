using Azumo.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Controller.ControllerInvoker;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Controller.InvokePipeline;
internal class UpdateHandle(IServiceProvider serviceProvider) : IUpdateHandler
{
    private readonly IServiceProvider _serviceProvider = serviceProvider;

    private readonly ILogger<UpdateHandle> _logger = serviceProvider.GetService<ILogger<UpdateHandle>>()!;

    public Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        _logger?.LogError(exception, "发生致命错误");
        return Task.CompletedTask;
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        var chatManager = _serviceProvider.GetRequiredService<IChatManager>();
        var context = chatManager.Create(botClient, update, _serviceProvider);

        try
        {
            var pipeline = context.UserScopeService.GetRequiredService<IPipelineController<PipelineInvokeModel, Task>>();
            await pipeline.Execute("INVOKE", new PipelineInvokeModel
            {
                Context = context,
                CommandManager = context.UserScopeService.GetRequiredService<BotCommandManager>(),
            });
        }
        catch (Exception e)
        {
            await HandlePollingErrorAsync(botClient, e, cancellationToken);
        }
        finally
        {

        }
    }
}
