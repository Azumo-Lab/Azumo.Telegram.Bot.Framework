using Azumo.PipelineMiddleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Controller.Params.ParamPipeline;
internal static class ParamPipelineStatic
{
    public static IPipelineController<PipelineModel, Task<EnumReadParam>> PipelineController =
        PipelineFactory.GetPipelineBuilder<PipelineModel, Task<EnumReadParam>>(() => Task.FromResult(EnumReadParam.None))
        .NewPipeline(EnumReadParam.None)
        .Use(new PipelineNone())
        .NewPipeline(EnumReadParam.WaitInput)
        .Use(new PipelineWaitForInput())
        .Build();
}
