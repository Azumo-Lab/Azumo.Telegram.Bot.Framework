using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Controller;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Pipeline.Abstracts;

namespace Telegram.Bot.Framework.Pipelines
{
    internal class ProcessControllerInvoke : IProcess<TGChat>
    {
        public async Task<TGChat> Execute(TGChat t, IPipelineController<TGChat> pipelineController)
        {
            IControllerInvoker controllerInvoker = t.UserService.GetService<IControllerInvoker>();
            try
            {
                await controllerInvoker.InvokeAsync(controllerInvoker.GetCommand(t), t);
            }
            catch (Exception)
            {

            }
            return await pipelineController.Next(t);
        }
    }
}
