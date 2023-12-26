using Azumo.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Bots;
using Telegram.Bot.Framework.UserAuthentication;

namespace Telegram.Bot.Example
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var dic = ArgsHelper.ToDictionary(args);
            if (!dic.TryGetValue("-setting", out var settingPath))
                ArgumentException.ThrowIfNullOrEmpty(settingPath, "请添加启动参数 -setting ，后面添加配置文件路径");

            var telegramBot = TelegramBuilder.Create()
                .AddConfiguration<AppSetting>(settingPath)
                .UseToken<AppSetting>(x => x.Token)
                .UseClashDefaultProxy()
                .AddSimpleConsole()
                .RegisterBotCommand()
                .AddUserAuthentication()
                .Build();

            var task = telegramBot.StartAsync();
            task.Wait();
        }
    }

    public class TestController : TelegramController
    {
        private static int count;

        [BotCommand]
        public async Task Test()
        {
            await Chat.BotClient.SendChatActionAsync(Chat.UserChatID, Types.Enums.ChatAction.Typing);
            _ = await Chat.BotClient.SendTextMessageAsync(Chat.UserChatID, $"第{count++}次 Hello World !!");
        }

        [Authenticate("admin")]
        [BotCommand("/Catch")]
        public async Task Test2(string str)
        {
            Logger.LogInformation($"发送的消息 : {str}");
            _ = await Chat.BotClient.SendTextMessageAsync(Chat.UserChatID, $"You Say {str}");
        }
    }
}
