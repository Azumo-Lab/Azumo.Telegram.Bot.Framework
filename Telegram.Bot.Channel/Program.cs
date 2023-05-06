using _1Password.TokenGetter;
using Microsoft.Extensions.DependencyInjection;
using System.Collections;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml.Linq;
using Telegram.Bot.Channel.Controllers;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Abstract.Event;
using Telegram.Bot.Framework.Abstract.Groups;
using Telegram.Bot.Framework.ChannelGroup;
using Telegram.Bot.Polling;

namespace Telegram.Bot.Channel
{
    public class Program
    {
        private static Random random = new Random(Guid.NewGuid().GetHashCode());
        static Program()
        {
            Console.OutputEncoding = Encoding.UTF8;
        }
        
        public record Hello
        (
            string Name, 
            string PP
        );

        public ref struct MyStruct
        {
            
        }

        public static void Main(string[] args)
        {
            try
            {
                string botToken;
                using (OnePasswordCLI op = new())
                {
                    // 从1Password读取开发测试一号机的Token
                    botToken = op.Read("op://Tokens/rolpxqoi3qvkjnbsrq7m7ohmpq/credential");
                }

                ITelegramBot telegramBot = TelegramBotBuilder.Create()
                    .AddProxy("http://127.0.0.1:7890")
                    .AddToken(botToken)
                    .AddConfig(x =>
                    {
                        _ = x.AddScoped<IGroupMessageProcess, GroupMessage>();
                        _ = x.AddScoped<IChatMemberChange, BotTelegramEvent>();
                    })
                    .AddBotName("Test")
                    .AddReceiverOptions(new ReceiverOptions { AllowedUpdates = { } })
                    .Build();

                Task task = telegramBot.BotStart();
                task.Wait();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"发生致命错误：");
                Console.WriteLine(ex.Message);
                Environment.Exit(1);
            }
        }
    }
}