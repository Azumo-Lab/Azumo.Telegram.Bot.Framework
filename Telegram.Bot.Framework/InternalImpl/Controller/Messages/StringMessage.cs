using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controller;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.InternalImpl.Controller.Messages
{
    [TypeFor(typeof(string))]
    internal class StringMessage : IMessage
    {
        public async Task SendAsync(TGChat chat, ParameterInfo parameterInfo)
        {
            await chat.BotClient.SendTextMessageAsync(chat.ChatId, "请输入参数");
        }
    }
}
