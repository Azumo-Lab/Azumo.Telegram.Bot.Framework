using Azumo.PipelineMiddleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Controller.Params.ParamPipeline;
internal class PipelineWaitForInput : IMiddleware<PipelineModel, Task<EnumReadParam>>, IMiddlewareName
{
    public PipelinePhase Phase => PipelinePhase.GeneralProcessing;

    public string MiddlewareName => "WaitForInput";

    public Task<EnumReadParam> Execute(PipelineModel input, IPipelineController<PipelineModel, Task<EnumReadParam>> pipelineController)
    {
        input.ParamList.Add(input.ParameterGetter.GetParam(input.UserChatContext)!);
        return Task.FromResult(input.ParameterGetter.Result);
    }
}
