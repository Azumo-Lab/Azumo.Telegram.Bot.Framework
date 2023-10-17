using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Pipeline.Abstracts;

namespace Telegram.Bot.Framework.Pipeline
{
    internal class InternalPipelineControllerDatasFilter : IPipelineFilter
    {
        public (T result, bool next) Execute<T>(T t, IPipelineController<T> pipelineController, IProcess<T> process, PipelineDelegate<T> nextHandle)
        {
            pipelineController.NextPipeline = nextHandle;
            if (process is IPipelineName pipelineName)
                pipelineController.NextPipelineName = pipelineName.Name;
            return (t, true);
        }
    }
}
