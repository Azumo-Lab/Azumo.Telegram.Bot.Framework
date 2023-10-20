using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Pipeline.Abstracts;

namespace Telegram.Bot.Framework.Pipeline
{
    internal class PipelineFilter : IPipelineFilter
    {
        public (T result, bool next) Execute<T>(T t, IPipelineController<T> pipelineController, IProcessAsync<T> process, PipelineDelegate<T> nextHandle)
        {
            pipelineController.NextPipeline = nextHandle;
            pipelineController.NextPipelineName = (process is IPipelineName name) ? name.Name : null!;
            return (t, true);
        }
    }
}
