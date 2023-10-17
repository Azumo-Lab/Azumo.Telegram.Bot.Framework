using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Bots
{
    /// <summary>
    /// 机器人接口
    /// </summary>
    public interface ITelegramBot
    {
        /// <summary>
        /// 启动
        /// </summary>
        /// <returns></returns>
        public Task StartAsync();

        /// <summary>
        /// 停止
        /// </summary>
        /// <returns></returns>
        public Task StopAsync();
    }
}
