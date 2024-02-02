using Azumo.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Controller.CorePipeline.Model;

namespace Telegram.Bot.Framework.Core.Controller.CorePipeline;
internal class PipelineControllerInvoke : IMiddleware<PipelineModel>
{
    public PipelinePhase Phase => PipelinePhase.GeneralProcessing;

    public async Task Execute(PipelineModel input, IPipelineController<PipelineModel, Task> pipelineController)
    {
        var paramManager = input.CommandScopeService.Service!.GetRequiredService<IParamManager>();
        var exec = input.CommandScopeService.Session.GetCommand();

        try
        {
            await exec.Invoke(paramManager.GetParam());
        }
        catch (Exception)
        {

        }
        finally
        {
            input.CommandScopeService.Delete();
        }

        await pipelineController.Next(input);
    }
}
