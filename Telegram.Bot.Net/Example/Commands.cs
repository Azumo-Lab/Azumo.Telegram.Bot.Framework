using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Framework.TelegramException;
using Telegram.Bot.Framework.TelegramMessage;

namespace Telegram.Bot.Net.Example
{
    public class Commands : TelegramController
    {
        private readonly IServiceProvider serviceProvider;
        public Commands(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        [Command("Test")]
        public void Test([Param("密码", false)]string Password, [Param("请输入用户名信息", true)]string Username)
        {
            SendPhoto("XXX.JPG");
            SendSticker();
        }
    }
}
