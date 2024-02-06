using Azumo.SuperExtendedFramework.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Core;
using Telegram.Bot.Framework.Core.Execs;
using Telegram.Bot.Framework.Internal;

namespace Telegram.Bot.Framework.TelegramBotProc;

[TelegramBotStartProc]
internal class InvokeStartTask : IMiddleware<IServiceProvider, Task>
{
    public async Task Invoke(IServiceProvider input, PipelineMiddlewareDelegate<IServiceProvider, Task> Next)
    {
        var logger = input.GetService<ILogger<InvokeStartTask>>();
        var tasks = input.GetServices<IStartTask>().ToList();
        if (tasks.Count != 0)
        {
            logger?.LogInformation("总共找到 {A0} 个任务", tasks.Count);
            foreach (var task in tasks)
            {
                var name = (task as IName)?.Name;
                if (!string.IsNullOrEmpty(name))
                    logger?.LogInformation("开始执行任务：{A0}", name);

                await task.Exec();
            }
        }

        await Next(input);
    }
}
