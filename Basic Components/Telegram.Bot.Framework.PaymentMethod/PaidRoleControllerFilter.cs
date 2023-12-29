using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.PaymentMethod
{
    internal class PaidRoleControllerFilter : IControllerFilter
    {
        public async Task<bool> Execute(TelegramUserChatContext tGChat, BotCommand botCommand)
        {
            if (botCommand == null || botCommand.AuthenticateAttribute == null)
                return true;

            var authenticateAttribute = botCommand.AuthenticateAttribute;

            

            return await tGChat.UserServices.UserAuthenticate.IsAuthenticated(tGChat, authenticateAttribute);
        }
    }
}
