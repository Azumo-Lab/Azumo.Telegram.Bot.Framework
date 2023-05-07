using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Session;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstracts.Middlewares
{
    /// <summary>
    /// 中间件流水线
    /// </summary>
    public interface IMiddlewarePipeline
    {
        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="session">访问的请求对话</param>
        /// <returns>异步方法</returns>
        public Task Execute(ITelegramSession session);
    }
}
