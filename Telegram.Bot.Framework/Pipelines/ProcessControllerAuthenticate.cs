using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controller;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Pipeline.Abstracts;

namespace Telegram.Bot.Framework.Pipelines
{
    internal class ProcessControllerAuthenticate : IProcessAsync<(TGChat tGChat, IControllerContext controllerContext)>
    {
        public async Task<(TGChat tGChat, IControllerContext controllerContext)> ExecuteAsync((TGChat tGChat, IControllerContext controllerContext) t, IPipelineController<(TGChat tGChat, IControllerContext controllerContext)> pipelineController)
        {
            List<AuthenticateAttribute> attributes = new List<AuthenticateAttribute>();
            if (t.controllerContext?.BotCommand?.MethodAttributes != null)
                attributes.AddRange(t.controllerContext.BotCommand.MethodAttributes.Where(x => x is AuthenticateAttribute).Select(x => (AuthenticateAttribute)x));
            if (t.controllerContext?.BotCommand?.ControllerAttributes != null)
                attributes.AddRange(t.controllerContext.BotCommand.ControllerAttributes.Where(x => x is AuthenticateAttribute).Select(x => (AuthenticateAttribute)x));

            if (!attributes.Any())
                return await pipelineController.NextAsync(t);

            foreach (AuthenticateAttribute authenticate in attributes)
                if (string.IsNullOrEmpty(t.tGChat.Authenticate.RoleName))
                    return await pipelineController.StopAsync(t);
                else if (t.tGChat.Authenticate.RoleName == authenticate.RoleName)
                    return await pipelineController.NextAsync(t);

            return await pipelineController.StopAsync(t);
        }
    }
}
