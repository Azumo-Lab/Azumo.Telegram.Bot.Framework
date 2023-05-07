using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Session;

namespace Telegram.Bot.Framework.Abstracts.Middlewares
{
    /// <summary>
    /// 中间件接口
    /// </summary>
    public interface IMiddleware
    {
        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="Session">访问的请求对话</param>
        /// <param name="PipelineController">流水线控制器</param>
        /// <returns>异步方法</returns>;
        public Task Execute(ITelegramSession Session, IPipelineController PipelineController);
    }
}
