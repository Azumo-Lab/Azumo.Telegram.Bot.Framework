using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Controllers
{
    public class Authentication : IControllerFilter
    {
        public virtual async Task<bool> Execute(TGChat tGChat, BotCommand botCommand)
        {
            if (botCommand.AuthenticateAttribute == null)
                return true;

            AuthenticateAttribute authenticateAttribute = botCommand.AuthenticateAttribute;
            return await Task.FromResult(tGChat.Authenticate.IsAuthenticated(tGChat, authenticateAttribute));
        }
    }
}
