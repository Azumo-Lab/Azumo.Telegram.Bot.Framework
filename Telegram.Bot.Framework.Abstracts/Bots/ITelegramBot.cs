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
