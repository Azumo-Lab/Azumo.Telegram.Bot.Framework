using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Framework.Abstracts.User;
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Framework.Pipeline.Abstracts;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.CorePipelines
{
    /// <summary>
    /// 框架的Handle
    /// </summary>
    internal class InternalTelegramUpdateHandle : IUpdateHandler
    {
        private readonly IServiceScope __BotScopeService;
        private readonly IUserManager __UserManager;
        private readonly IPipelineController<IChat> __PipelineController;

        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="ServiceProvider">服务提供</param>
        public InternalTelegramUpdateHandle(IServiceProvider ServiceProvider)
        {
            __BotScopeService = ServiceProvider.CreateScope();
            __UserManager = __BotScopeService.ServiceProvider.GetService<IUserManager>();

            __PipelineController = PipelineFactory.CreateIPipelineBuilder<IChat>()
                .BuilderPipelineController();
        }

        ~InternalTelegramUpdateHandle()
        {
            __BotScopeService.Dispose();
        }

        /// <summary>
        /// 错误的执行者
        /// </summary>
        /// <param name="botClient"></param>
        /// <param name="exception"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// 正确的执行者
        /// </summary>
        /// <param name="BotClient"></param>
        /// <param name="Update"></param>
        /// <param name="CancellationToken"></param>
        /// <returns></returns>
        public async Task HandleUpdateAsync(ITelegramBotClient BotClient, Update Update, CancellationToken CancellationToken)
        {
            try
            {
                // 创建 ITelegramChat 对象
                IChat chat = __UserManager.CreateIChat(BotClient, Update, __BotScopeService);

                _ = await __PipelineController.SwitchTo(Update.Type, chat);
            }
            catch (UnauthorizedAccessException)
            {
                // 用户被Ban，无视错误
            }
            catch (ApiRequestException)
            {
                // API 错误，网络不好，无视错误
            }
            catch (Exception Ex)
            {
                await HandlePollingErrorAsync(BotClient, Ex, CancellationToken);
            }
        }
    }
}
