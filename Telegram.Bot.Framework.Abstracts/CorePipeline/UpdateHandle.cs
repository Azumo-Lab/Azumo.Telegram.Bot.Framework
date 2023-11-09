using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.CorePipeline
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Singleton, typeof(IUpdateHandler))]
    internal class UpdateHandle : IUpdateHandler
    {
        private readonly IServiceProvider __ServiceProvider;
        private readonly ILogger<UpdateHandle> __Logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="logger"></param>
        public UpdateHandle(IServiceProvider serviceProvider, ILogger<UpdateHandle> logger)
        {
            __ServiceProvider = serviceProvider;
            __Logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            __Logger.LogError(exception, "发生错误");
            await Task.CompletedTask;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="update"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            IChatManager chatManager = __ServiceProvider.GetRequiredService<IChatManager>();

            TGChat tGChat = chatManager.Create(botClient, update, __ServiceProvider);

            await UserScope.Invoke(tGChat);
        }
    }
}
