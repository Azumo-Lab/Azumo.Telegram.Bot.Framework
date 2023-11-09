using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Types;

namespace Telegram.Bot.ChannelManager.Controllers
{
    public class AddChannelController : TelegramController
    {
        public AddChannelController() { }

        [BotCommand("/AddChannel", Description = "说Hello")]
        public async Task AddChannel()
        {
            await Chat.BotClient.SendTextMessageAsync(Chat.ChatId, "Hello Admin");
        }

        [BotCommand("/ListFile", Description = "罗列出指定路径下的文件")]
        public async Task ListFile([Param(ControllerParamSenderType = typeof(Sender))]string Path)
        {
            if (!Directory.Exists(Path))
            {
                await Chat.BotClient.SendTextMessageAsync(Chat.ChatId, "路径不是有效路径");
                return;
            }
            string[] paths = Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories);
            StringBuilder stringBuilder = new StringBuilder();
            int count = 0;
            foreach (string path in paths)
            {
                stringBuilder.AppendLine(path);
                if (count >= 10)
                {
                    count = 0;
                    await Chat.BotClient.SendTextMessageAsync(Chat.ChatId, stringBuilder.ToString());
                    stringBuilder.Clear();
                }
                count++;
            }
            await Chat.BotClient.SendTextMessageAsync(Chat.ChatId, stringBuilder.ToString());
            stringBuilder.Clear();
        }
    }

    public class Sender : IControllerParamSender
    {
        public async Task Send(ITelegramBotClient botClient, ChatId chatId)
        {
            await botClient.SendTextMessageAsync(chatId, "请输入路径：");
        }
    }
}
