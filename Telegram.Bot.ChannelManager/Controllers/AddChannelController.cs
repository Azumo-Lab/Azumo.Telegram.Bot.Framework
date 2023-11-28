using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.InternalInterface;
using Telegram.Bot.Types;

namespace Telegram.Bot.ChannelManager.Controllers
{
    public class AddChannelController : TelegramController
    {
        public AddChannelController() { }

        [BotCommand("/Test", Description = "测试用")]
        public async Task Test()
        {
            string message = @"
<b>bold</b>, <strong>bold</strong>
<i>italic</i>, <em>italic</em>
<u>underline</u>, <ins>underline</ins>
<s>strikethrough</s>, <strike>strikethrough</strike>, <del>strikethrough</del>
<span class=""tg-spoiler"">spoiler</span>, <tg-spoiler>spoiler</tg-spoiler>
<b>bold <i>italic bold <s>italic bold strikethrough <span class=""tg-spoiler"">italic bold strikethrough spoiler</span></s> <u>underline italic bold</u></i> bold</b>
<a href=""http://www.example.com/"">inline URL</a>
<a href=""tg://user?id=123456789"">inline mention of a user</a>
<tg-emoji emoji-id=""5368324170671202286"">👍</tg-emoji>
<code>inline fixed-width code</code>
<pre>pre-formatted fixed-width code block</pre>
<pre><code class=""language-python"">pre-formatted fixed-width code block written in the Python programming language</code></pre>
#123
";
            string messageHtml = MessageBuilder()
                .Add((BaseMessage)"测试" | "Test" | new URLMessage("https://www.baidu.com", "百度"))
                .Add(new NewLineMessage())
                .Add(new HashTagMessage("测试标签"))
                .Add(new NewLineMessage())
                .Add(new SpoilerMessage("这是隐藏起来的内容"))
                .Add(new URLMessage("https://www.baidu.com", "百度"))
                .Add(new PreMessage("这是一个测试，点击可以复制"))
                .Build();
            await SendMediaGroup(messageHtml, 
                [
                    "C:\\Users\\ko--o\\OneDrive\\iCloud网盘\\Downloads\\头像\\1580359284346.JPG",
                    "C:\\Users\\ko--o\\OneDrive\\iCloud网盘\\Downloads\\头像\\1580187599750.JPG"
                ]);
        }

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
            stringBuilder.AppendLine("以上是全部内容.");
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
