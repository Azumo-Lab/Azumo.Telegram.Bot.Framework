using Azumo.SuperExtendedFramework.PipelineMiddleware.InternalPipeline;

namespace Azumo.SuperExtendedFramework.PipelineMiddleware;

/// <summary>
/// 管道工厂
/// </summary>
public class PipelineFactory
{
    /// <summary>
    /// 创建一个管道构建器
    /// </summary>
    /// <typeparam name="TInput">传入类型</typeparam>
    /// <typeparam name="TResult">结果类型</typeparam>
    /// <returns>管道构造器 <see cref="IPipelineBuilder{TInput, TResult}"/> 的实例</returns>
    public static IPipelineBuilder<TInput, TResult> GetPipelineBuilder<TInput, TResult>(Func<TResult> defVal) =>
        new PipelineBuilder<TInput, TResult>(defVal);

    /// <summary>
    /// 内部方法，用于创建一个管道
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="middleware"></param>
    /// <returns></returns>
    internal static IPipeline<TInput, TResult> GetPipeline<TInput, TResult>(PipelineMiddlewareDelegate<TInput, TResult> middleware) => 
        new Pipeline<TInput, TResult>(middleware);

    /// <summary>
    /// 内部方法，用于创建一个管道控制器
    /// </summary>
    /// <typeparam name="TInput"></typeparam>
    /// <typeparam name="TResult"></typeparam>
    /// <param name="dics"></param>
    /// <returns></returns>
    internal static IPipelineController<TInput, TResult> GetPipelineController<TInput, TResult>(Dictionary<object, IPipeline<TInput, TResult>> dics) => 
        new PipelineController<TInput, TResult>(dics);
}
