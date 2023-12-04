using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;

namespace Telegram.Bot.ChannelManager.Controllers
{
    public class LoginController : TelegramController
    {
        public LoginController() { }

        [BotCommand("/Login", Description = "输入密码，进行登录")]
        public async Task Log(string password)
        {
            if (password == Secret.AdminPassword)
            {
                _ = await Chat.BotClient.SendTextMessageAsync(Chat.ChatId, "登陆成功，欢迎管理员");
            }
            await Task.CompletedTask;
        }
    }
}
