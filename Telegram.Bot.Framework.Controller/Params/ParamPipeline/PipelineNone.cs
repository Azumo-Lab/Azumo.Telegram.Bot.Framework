using Azumo.PipelineMiddleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Controller.Params.ParamPipeline;
internal class PipelineNone : IMiddleware<PipelineModel, Task<EnumReadParam>>, IMiddlewareName
{
    public PipelinePhase Phase => PipelinePhase.GeneralProcessing;

    public string MiddlewareName => "NONE 类型处理";

    public async Task<EnumReadParam> Execute(PipelineModel input, IPipelineController<PipelineModel, Task<EnumReadParam>> pipelineController)
    {
        await input.ParameterGetter.SendPromptMessage(input.UserChatContext);
        return input.ParameterGetter.Result;
    }
}
