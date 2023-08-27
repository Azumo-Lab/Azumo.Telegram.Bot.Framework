using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts
{
    public abstract class TelegramController
    {
        protected TGChat Chat { get; private set; }

        internal void ControllerInvoke(TGChat Chat)
        {
            this.Chat = Chat;
        }
    }
}
