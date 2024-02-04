using Azumo.SuperExtendedFramework.PipelineMiddleware.InternalPipeline;

namespace Azumo.SuperExtendedFramework.PipelineMiddleware;

/// <summary>
/// 
/// </summary>
public class PipelineFactory
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <returns></returns>
    public static IPipelineBuilder<TInput, TResult> GetPipelineBuilder<TInput, TResult>(Func<TResult> defVal) =>
        new PipelineBuilder<TInput, TResult>(defVal);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="middleware"></param>
    /// <returns></returns>
    internal static IPipeline<TInput, TResult> GetPipeline<TInput, TResult>(PipelineMiddlewareDelegate<TInput, TResult> middleware) => 
        new Pipeline<TInput, TResult>(middleware);

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="dics"></param>
    /// <returns></returns>
    internal static IPipelineController<TInput, TResult> GetPipelineController<TInput, TResult>(Dictionary<object, IPipeline<TInput, TResult>> dics) => 
        new PipelineController<TInput, TResult>(dics);
}
