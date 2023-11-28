using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    public interface IMessageBuilder
    {
        public IMessageBuilder Add(IMessageContent messageContent);

        public string Build();
    }
}
