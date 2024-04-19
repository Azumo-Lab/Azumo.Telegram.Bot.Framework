namespace Azumo.SuperExtendedFramework.PipelineMiddleware;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TInput"></typeparam>
/// <typeparam name="TResult"></typeparam>
/// <param name="input"></param>
/// <returns></returns>
public delegate TResult PipelineMiddlewareDelegate<TInput, TResult>(TInput input);
