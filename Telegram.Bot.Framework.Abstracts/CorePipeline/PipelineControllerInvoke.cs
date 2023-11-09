using Azumo.Pipeline.Abstracts;
using Microsoft.Extensions.DependencyInjection;
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
            controllerParamManager.ControllerParams = new List<IControllerParam>(botCommand.ControllerParams);
            ResultEnum resultEnum = await controllerParamManager.NextParam(t);
            if (resultEnum != ResultEnum.Finish)
                return await pipelineController.StopAsync(t);

            TelegramController telegramController = ActivatorUtilities.CreateInstance<TelegramController>(t.UserService, botCommand.Controller, Array.Empty<object>());
            await telegramController.ControllerInvokeAsync(t, botCommand.Func, controllerParamManager);

            return await pipelineController.NextAsync(t);
        }
    }
}
