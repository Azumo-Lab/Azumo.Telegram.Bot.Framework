using Azumo.Pipeline.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts.CorePipeline
{
    internal class PipelineControllerInvoke : IProcessAsync<TGChat>
    {
        public async Task<TGChat> ExecuteAsync(TGChat t, IPipelineController<TGChat> pipelineController)
        {
            IControllerManager controllerManager = t.UserService.GetRequiredService<IControllerManager>();
            BotCommand botCommand = controllerManager.GetCommand(t);

            IControllerParamManager controllerParamManager = t.UserService.GetRequiredService<IControllerParamManager>();
            if (botCommand != null)
            {
                controllerParamManager.Clear();
                controllerParamManager.SetBotCommand(botCommand);
                controllerParamManager.ControllerParams = new List<IControllerParam>(botCommand.ControllerParams);
            }
            ResultEnum resultEnum = await controllerParamManager.NextParam(t);
            if (resultEnum != ResultEnum.Finish)
                return await pipelineController.StopAsync(t);

            botCommand ??= controllerParamManager.GetBotCommand();
            if (botCommand == null)
                return await pipelineController.StopAsync(t);

            try
            {
                TelegramController telegramController = (TelegramController)ActivatorUtilities.CreateInstance(t.UserService, botCommand!.Controller, Array.Empty<object>());
                await telegramController.ControllerInvokeAsync(t, botCommand.Func, controllerParamManager);
            }
            catch (Exception)
            {
                
            }

            return await pipelineController.NextAsync(t);
        }
    }
}
