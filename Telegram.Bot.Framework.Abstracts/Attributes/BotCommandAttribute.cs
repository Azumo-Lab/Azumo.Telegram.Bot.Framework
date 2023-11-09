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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="BotCommandName"></param>
        public BotCommandAttribute(string BotCommandName)
        {
            BotCommandName = BotCommandName.ToLower();
            if (!BotCommandName.StartsWith("/"))
                BotCommandName = $"/{BotCommandName}";
            this.BotCommandName = BotCommandName;
        }

        /// <summary>
        /// 
        /// </summary>
        public BotCommandAttribute()
        {
            BotCommandName = string.Empty;
        }
    }
}
