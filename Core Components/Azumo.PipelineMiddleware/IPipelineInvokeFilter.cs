using Azumo.PipelineMiddleware.Pipelines;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.PipelineMiddleware;
public interface IPipelineInvokeFilter<TInput>
{
    public bool Filter(Delegate handle, IMiddleware<TInput> middleware, TInput input, IPipelineController<TInput> pipelineController);
}
