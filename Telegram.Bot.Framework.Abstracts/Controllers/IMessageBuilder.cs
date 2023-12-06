namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public interface IMessageBuilder
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="messageContent"></param>
        /// <returns></returns>
        public IMessageBuilder Add(IMessageContent messageContent);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string Build();
    }
}
