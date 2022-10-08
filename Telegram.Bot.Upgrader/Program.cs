// See https://aka.ms/new-console-template for more information

using Microsoft.Extensions.Configuration;
using Telegram.Bot.Framework;
using Telegram.Bot.Upgrader;

var Secrets = new ConfigurationBuilder().AddUserSecrets("221c57ce-4491-4bc3-ac05-78fb55c40269").Build();

string Token = Secrets.GetSection("Token").Value;
string Proxy = Secrets.GetSection("Proxy").Value;
int Port = int.Parse(Secrets.GetSection("Port").Value);

var bot = TelegramBotManger.Create()
    .SetToken(Token)
    .SetProxy(Proxy, Port)
    .AddConfig<StartUp>()
    .SetBotName("DEV1")
    .Build();

Task botTask = bot.Start();

botTask.Wait();
