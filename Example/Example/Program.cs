// See https://aka.ms/new-console-template for more information

using System.Text;
using Telegram.Bot.Framework;
using Telegram.Bot.Types;

//var telegramBot = TelegramBot.CreateBuilder()
//    .UseClashDefaultProxy()
//    .UseToken("<Token>")
//    .Build();

//await telegramBot.StartAsync(true);

var command = "!$#@~!@#$%^&***+_(手上(_*";
// 对指令进行验证
foreach (var item in command)
{
    if (item is not <= (char)sbyte.MaxValue or not >= (char)0)
        throw new Exception("Not ASCII Command");
}
Console.WriteLine("OK");