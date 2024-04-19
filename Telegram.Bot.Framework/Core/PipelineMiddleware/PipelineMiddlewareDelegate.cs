namespace Azumo.SuperExtendedFramework.PipelineMiddleware;

public delegate TResult PipelineMiddlewareDelegate<TInput, TResult>(TInput input);
