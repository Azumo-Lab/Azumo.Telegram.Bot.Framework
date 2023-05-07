using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.Abstracts.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public interface IPipelineBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public IPipelineBuilder AddMiddleware<T>() where T : IMiddleware;

        public MiddlewareDelegate Builder(IPipelineController PipelineController);
    }
}
