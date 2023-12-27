using Azumo.Pipeline.Abstracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.InternalInterface;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.CorePipelines
{
    internal class PipelineControllerParams : IProcessAsync<TelegramUserChatContext>
    {
        public async Task<TelegramUserChatContext> ExecuteAsync(TelegramUserChatContext t, IPipelineController<TelegramUserChatContext> pipelineController)
        {
            var controllerManager = t.UserScopeService.GetService<IControllerParamManager>();
            var resultEnum = await controllerManager.NextParam(t);
            
            return resultEnum != ResultEnum.Finish ? await pipelineController.StopAsync(t) : await pipelineController.NextAsync(t);
        }
    }
}
