using Azumo.Pipeline.Abstracts;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts.CorePipeline
{
    internal class PipelineControllerInvoke : IProcessAsync<TGChat>
    {
        public Task<TGChat> ExecuteAsync(TGChat t, IPipelineController<TGChat> pipelineController)
        {
            
        }
    }
}
