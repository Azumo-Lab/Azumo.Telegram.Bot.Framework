using Azumo.SuperExtendedFramework.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Internal;

namespace Telegram.Bot.Framework.TelegramBotProc;

[TelegramBotEndProc]
internal class ShowBotUsername : IMiddleware<IServiceProvider, Task>
{
    public async Task Invoke(IServiceProvider input, PipelineMiddlewareDelegate<IServiceProvider, Task> Next)
    {
        var logger = input.GetService<ILogger<ShowBotUsername>>();

        var botClient = input.GetRequiredService<ITelegramBotClient>();

        var user = await botClient.GetMeAsync();

        logger?.LogInformation("用户名：@{A0}，ID：{A1}，名字：{A2}，正在执行中 ...", user.Username, user.Id, $"{user.FirstName} {user.LastName}");

        await Next(input);
    }
}
