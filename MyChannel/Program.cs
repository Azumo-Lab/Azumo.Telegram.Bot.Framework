using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MyChannel.Controllers;
using MyChannel.DataBaseContext;
using MyChannel.Services;
using NLog;
using NLog.Config;
using NLog.Extensions.Logging;
using NLog.Targets;
using Telegram.Bot;
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Abstracts.Exec;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Bots;

namespace MyChannel
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            ITelegramBot telegramBot = TelegramBuilder.Create()
                .UseToken<AppSetting>(x => x.Token)
                .UseClashDefaultProxy()
                .AddServices((serviceCollection, buildService) =>
                {
                    AppSetting appSetting = buildService.GetService<AppSetting>()!;

                    _ = serviceCollection.AddDbContext<MyDBContext>(option =>
                    {
                        option.EnableServiceProviderCaching();
                        option.UseSqlite(appSetting.DatabaseSetting!.ConnectionString);
                    });

                    _ = serviceCollection.AddSingleton<IStartExec, StartService>();
                    _ = serviceCollection.AddSingleton<IExec, PublishService>();
                    _ = serviceCollection.AddSingleton<IExec, ScheduledService>();

                    _ = serviceCollection.RemoveAll<IAuthenticate>();
                    _ = serviceCollection.AddScoped<IAuthenticate, BlockUserAuth>();
                })
                .AddConfiguration<AppSetting>("D:\\Download\\MyChannelBot\\Setting.json")
                .RegisterBotCommand()
                .AddSimpleConsole()
                .AddLogger((logbuilder, buildService) =>
                {
                    AppSetting appSetting = buildService.GetService<AppSetting>()!;

                    LoggingConfiguration setting = new();
                    setting.AddTarget(new FileTarget
                    {
                        Name = "FileLog",
                        AutoFlush = true,
                        FileName = Path.Combine(appSetting.LogSetting!.LogPath!, "${date:format=yyyy-MM-dd}.log")
                    });
                    setting.AddRule(LogLevel.Info, LogLevel.Fatal, "FileLog");
                    logbuilder.AddNLog(setting);
                })
                .Build();

            Task botTask = telegramBot.StartAsync();
            botTask.Wait();
        }
    }
}
