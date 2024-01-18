using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azumo.PipelineMiddleware;
public interface IPipelineControllerBuilder<TInput>
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="pipelines"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    IPipelineControllerBuilder<TInput> AddRange(IEnumerable<IPipeline<TInput>> pipelines, object name);
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IPipelineController<TInput> Build();
}
