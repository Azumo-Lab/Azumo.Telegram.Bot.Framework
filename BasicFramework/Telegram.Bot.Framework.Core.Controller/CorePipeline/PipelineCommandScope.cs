using Azumo.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Controller.CorePipeline.Model;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Core.Controller.CorePipeline;
internal class PipelineCommandScope : IMiddleware<PipelineModel>
{
    public PipelinePhase Phase => PipelinePhase.PreProcessing;

    public async Task Execute(PipelineModel input, IPipelineController<PipelineModel, Task> pipelineController)
    {
        var command = input.UserContext.GetCommand();
        if (command != null)
        {
            input.CommandScopeService.DeleteOldCreateNew();

            var exec = input.CommandManager.GetExecutor(input.UserContext);
            input.CommandScopeService.Session.AddCommand(exec);

            var paramManager = input.CommandScopeService.Service!.GetRequiredService<IParamManager>();
            paramManager.SetParamList(exec.Parameters);
        }
            

        await pipelineController.Next(input);
    }
}
