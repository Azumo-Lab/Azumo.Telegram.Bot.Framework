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
            var botCommand = chat.Session.GetBotCommand();
            // 获取参数
            var controllerParamManager = chat.UserScopeService.GetRequiredService<IControllerParamManager>();
            if (botCommand != null)
            {
                controllerParamManager.Clear();
                controllerParamManager.SetBotCommand(botCommand);
                controllerParamManager.ControllerParams = new List<IControllerParam>(botCommand.ControllerParams);
            }
            var resultEnum = await controllerParamManager.NextParam(chat);
            if (resultEnum != ResultEnum.Finish)
                return await pipelineController.StopAsync(chat);

            botCommand ??= controllerParamManager.GetBotCommand();
            if (botCommand == null)
                return await pipelineController.StopAsync(chat);

            // 执行控制器
            try
            {
                var telegramController = (TelegramController)ActivatorUtilities.CreateInstance(chat.UserScopeService, botCommand!.Controller, []);
                await telegramController.ControllerInvokeAsync(chat, botCommand.Func, controllerParamManager);
            }
            catch (Exception)
            { }

            // 执行下一个
            return await pipelineController.NextAsync(chat);
        }
    }
}
