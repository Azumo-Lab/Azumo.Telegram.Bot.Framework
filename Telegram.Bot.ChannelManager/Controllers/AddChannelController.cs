﻿using System.Text;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.InternalInterface;
using Telegram.Bot.Types;

namespace Telegram.Bot.ChannelManager.Controllers
{
    public class AddChannelController : TelegramController
    {
        public AddChannelController() { }

        [BotCommand("/Test", Description = "测试用")]
        public async Task Test()
        {
            var messageHtml = GetMessageBuilder()
                .Add(new BoldMessage("测试") | new URLMessage("https://www.baidu.com", "百度") | (BaseMessage)"连接测试")
                .Add(new NewLineMessage())
                .Add("测试 Test" & new URLMessage("https://www.baidu.com", "点击直达百度"))
                .Add(new NewLineMessage())
                .Add(new HashTagMessage("测试标签") & new HashTagMessage("TAG"))
                .Add(new NewLineMessage())
                .Add(new SpoilerMessage("这是隐藏起来的内容"))
                .Add(new NewLineMessage())
                .Add(new URLMessage("https://www.baidu.com", "百度"))
                .Add(new NewLineMessage())
                .Add(new PreMessage("这是一个测试，点击可以复制"))
                .Build();
            _ = await SendFile(messageHtml,
                "C:\\Users\\ko--o\\OneDrive\\iCloud网盘\\Downloads\\头像\\1580359284346.JPG"
                , "珍藏头像001");
        }

        [BotCommand("/AddChannel", Description = "说Hello")]
        public async Task AddChannel() => _ = await Chat.BotClient.SendTextMessageAsync(Chat.UserChatID, "Hello Admin");

        [BotCommand("/ListFile", Description = "罗列出指定路径下的文件")]
        public async Task ListFile([Param(Sender = typeof(Sender))] string Path)
        {
            if (!Directory.Exists(Path))
            {
                _ = await Chat.BotClient.SendTextMessageAsync(Chat.UserChatID, "路径不是有效路径");
                return;
            }
            var paths = Directory.GetFiles(Path, "*.*", SearchOption.AllDirectories);
            StringBuilder stringBuilder = new();
            var count = 0;
            foreach (var path in paths)
            {
                _ = stringBuilder.AppendLine(path);
                if (count >= 10)
                {
                    count = 0;
                    _ = await Chat.BotClient.SendTextMessageAsync(Chat.UserChatID, stringBuilder.ToString());
                    _ = stringBuilder.Clear();
                }
                count++;
            }
            _ = stringBuilder.AppendLine("以上是全部内容.");
            _ = await Chat.BotClient.SendTextMessageAsync(Chat.UserChatID, stringBuilder.ToString());
            _ = stringBuilder.Clear();
        }
    }

    public class Sender : IControllerParamSender
    {
        public async Task Send(ITelegramBotClient botClient, ChatId chatId, ParamAttribute paramAttribute) => _ = await botClient.SendTextMessageAsync(chatId, "请输入路径：");
    }
}
