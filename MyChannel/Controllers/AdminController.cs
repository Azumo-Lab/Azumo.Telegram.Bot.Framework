using MyChannel.DataBaseContext;
using MyChannel.DataBaseContext.DBModels;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;

namespace MyChannel.Controllers
{
    internal class AdminController(MyDBContext myDBContext) : TelegramController
    {
        private readonly MyDBContext __MyDBContext = myDBContext;

        [BotCommand("Admin", Description = "设定管理员用户")]
        public async Task Admin(string adminUsername, string adminPassword)
        {
            _ = __MyDBContext.UserInfoEntity.Where(x => x.AuthEnum == AuthEnum.Admin).Any();
            await Task.CompletedTask;
        }
    }
}
