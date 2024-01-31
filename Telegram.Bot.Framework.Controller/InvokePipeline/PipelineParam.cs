using Azumo.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Controller.Params;

namespace Telegram.Bot.Framework.Controller.InvokePipeline;
internal class PipelineParam : IMiddleware<PipelineInvokeModel>
{
    public PipelinePhase Phase => throw new NotImplementedException();

    public async Task Execute(PipelineInvokeModel input, IPipelineController<PipelineInvokeModel, Task> pipelineController)
    {
        var execInvoke = input.CommandManager.GetExecutor(input.Context);
        if (execInvoke == null)
            return;

        var paramManager = input.Context.UserScopeService.GetRequiredService<ParameterManager>();

        paramManager.Init(execInvoke);
        var result = await paramManager.Read(input.Context);
        if(result != EnumReadParam.OK)
            return;

        await pipelineController.Next(input);
    }
}
