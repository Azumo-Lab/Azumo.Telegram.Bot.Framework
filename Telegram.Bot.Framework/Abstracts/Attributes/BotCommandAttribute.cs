namespace Telegram.Bot.Framework.Abstracts.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class BotCommandAttribute : Attribute
    {
        public string BotCommandName { get; }
        public BotCommandAttribute(string BotCommandName)
        {
            BotCommandName = BotCommandName.ToLower();
            if (!BotCommandName.StartsWith("/"))
                BotCommandName = $"/{BotCommandName}";
            this.BotCommandName = BotCommandName;
        }
        public BotCommandAttribute()
        {
            BotCommandName = string.Empty;
        }
    }
}
