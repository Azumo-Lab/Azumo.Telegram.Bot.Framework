using Azumo.SuperExtendedFramework.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Internal;
using Telegram.Bot.Polling;

namespace Telegram.Bot.Framework.TelegramBotProc;

[TelegramBotProc]
internal class BotStart : IMiddleware<IServiceProvider, Task>
{
    public async Task Invoke(IServiceProvider input, PipelineMiddlewareDelegate<IServiceProvider, Task> Next)
    {
        var _tokenSource = input.GetRequiredService<CancellationTokenSource>();

        // Bot开始启动
        var botClient = input.GetRequiredService<ITelegramBotClient>();

        if (!await botClient.TestApiAsync(_tokenSource.Token))
            throw new Exception();

        botClient.StartReceiving(input.GetRequiredService<IUpdateHandler>(),
            new ReceiverOptions
            {
                AllowedUpdates = []
            },
            _tokenSource.Token);

        await Next(input);
    }
}
