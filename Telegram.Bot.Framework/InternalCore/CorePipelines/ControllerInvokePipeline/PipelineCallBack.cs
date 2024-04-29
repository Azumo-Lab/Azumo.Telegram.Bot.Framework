using Telegram.Bot.Framework.Core.PipelineMiddleware;
using Telegram.Bot.Framework.InternalCore.CorePipelines.Models;

namespace Telegram.Bot.Framework.InternalCore.CorePipelines.ControllerInvokePipeline;
internal class PipelineCallBack : IMiddleware<PipelineModel, Task>
{
    public async Task Invoke(PipelineModel input, PipelineMiddlewareDelegate<PipelineModel, Task> Next) => await Task.CompletedTask;
}
