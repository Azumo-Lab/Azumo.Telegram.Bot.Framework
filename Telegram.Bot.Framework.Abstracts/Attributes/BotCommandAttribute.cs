using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BotCommandAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        public string BotCommandName { get; }

        public MessageType? MessageType { get; }

        public string Description { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BotCommandName"></param>
        public BotCommandAttribute(string BotCommandName) : this(null, BotCommandName) { }

        public BotCommandAttribute(MessageType messageType) : this(messageType, string.Empty) { }

        public BotCommandAttribute(MessageType? MessageType, string BotCommandName)
        {
            BotCommandName = BotCommandName.ToLower();
            if (!BotCommandName.StartsWith("/"))
                BotCommandName = $"/{BotCommandName}";
            this.BotCommandName = BotCommandName.ToLower();

            this.MessageType = MessageType;
        }

        /// <summary>
        /// 
        /// </summary>
        public BotCommandAttribute()
        {
            BotCommandName = null!;
        }
    }
}
