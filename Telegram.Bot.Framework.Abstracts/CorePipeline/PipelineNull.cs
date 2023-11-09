using Azumo.Pipeline.Abstracts;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts.CorePipeline
{
    /// <summary>
    /// 
    /// </summary>
    internal class PipelineNull : IProcessAsync<TGChat>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pipelineController"></param>
        /// <returns></returns>
        public Task<TGChat> ExecuteAsync(TGChat t, IPipelineController<TGChat> pipelineController)
        {
            return pipelineController.StopAsync(t);
        }
    }
}
