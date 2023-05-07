using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram.Bot.Framework.Abstracts.Middlewares
{
    /// <summary>
    /// 中间件的模板方法(用于向中间件流水线中添加固定的执行中间件)
    /// </summary>
    internal interface IMiddlewareTemplate
    {
        /// <summary>
        /// 在用户的中间件执行之前
        /// </summary>
        /// <param name="ServiceProvider">服务提供</param>
        void BeforeAddMiddlewareHandles(IServiceProvider ServiceProvider);

        /// <summary>
        /// 在用户的中间件执行之后
        /// </summary>
        /// <param name="ServiceProvider">服务提供</param>
        void AfterAddMiddlewareHandles(IServiceProvider ServiceProvider);
    }
}
