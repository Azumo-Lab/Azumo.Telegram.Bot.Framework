﻿using Telegram.Bot.Framework.Core.Controller.CorePipeline.Model;
using Telegram.Bot.Framework.Core.PipelineMiddleware;

namespace Telegram.Bot.Framework.Core.Controller.CorePipeline;
internal class PipelineCallBack : IMiddleware<PipelineModel, Task>
{
    public async Task Invoke(PipelineModel input, PipelineMiddlewareDelegate<PipelineModel, Task> Next) => await Task.CompletedTask;
}
