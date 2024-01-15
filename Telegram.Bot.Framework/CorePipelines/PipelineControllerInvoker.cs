using Azumo.Pipeline.Abstracts;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.CorePipelines
{
    internal class PipelineControllerInvoker : IProcessAsync<TelegramUserChatContext>
    {
        public async Task<TelegramUserChatContext> ExecuteAsync(TelegramUserChatContext chat, IPipelineController<TelegramUserChatContext> pipelineController)
        {
            // 获取参数
            var controllerParamManager = chat.UserScopeService.GetRequiredService<IControllerParamManager>();

            var botCommand = controllerParamManager.GetBotCommand();
            if (botCommand == null)
                return await pipelineController.StopAsync(chat);

            // 执行控制器
            try
            {
                var controllerObj = botCommand.ObjectFactory(chat.UserScopeService, []);
                await ((TelegramController)controllerObj)
                    .ControllerInvokeAsync(chat, botCommand.Func, controllerParamManager);
            }
            catch (Exception)
            { 
                
            }
            finally
            {
                controllerParamManager.Dispose();
            }

            // 执行下一个
            return await pipelineController.NextAsync(chat);
        }
    }
}
