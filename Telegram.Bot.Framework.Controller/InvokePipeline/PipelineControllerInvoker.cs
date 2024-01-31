using Azumo.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller.Params;

namespace Telegram.Bot.Framework.Controller.InvokePipeline;
internal class PipelineControllerInvoker : IMiddleware<PipelineInvokeModel>
{
    public PipelinePhase Phase => PipelinePhase.GeneralProcessing;

    public async Task Execute(PipelineInvokeModel input, IPipelineController<PipelineInvokeModel, Task> pipelineController)
    {
        var paramManager = input.Context.UserScopeService.GetRequiredService<ParameterManager>();
        await paramManager.Executor.Invoke(input.Context.UserScopeService, paramManager.GetParams());

        await pipelineController.Next(input);
    }
}
