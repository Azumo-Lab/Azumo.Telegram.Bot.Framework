namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    /// <summary>
    /// HTML TG 消息创建
    /// </summary>
    public interface IMessageBuilder
    {
        /// <summary>
        /// 添加消息类型接口实例
        /// </summary>
        /// <param name="messageContent"></param>
        /// <returns></returns>
        public IMessageBuilder Add(IMessageContent messageContent);

        /// <summary>
        /// 创建HTML消息
        /// </summary>
        /// <returns></returns>
        public string Build();
    }
}
