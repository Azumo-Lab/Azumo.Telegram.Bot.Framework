using MyChannel.DataBaseContext;
using MyChannel.DataBaseContext.DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            __MyDBContext.Users.Where(x => x.AuthEnum == AuthEnum.Admin).Any();
            await Task.CompletedTask;
        }
    }
}
