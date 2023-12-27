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
                if (botCommand.Target != null)
                    await botCommand.StaticFun(controllerParamManager.GetParams());
                else
                    await ((TelegramController)ActivatorUtilities.CreateInstance(chat.UserScopeService, botCommand!.Controller, []))
                        .ControllerInvokeAsync(chat, botCommand.Func, controllerParamManager);
            }
            catch (Exception)
            { 
                
            }
            finally
            {
                controllerParamManager.Clear();
            }

            // 执行下一个
            return await pipelineController.NextAsync(chat);
        }
    }
}
