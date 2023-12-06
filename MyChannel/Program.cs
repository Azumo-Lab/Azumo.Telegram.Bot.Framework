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
using Telegram.Bot.Framework;
using Telegram.Bot.Framework.Abstracts.Exec;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Bots;

namespace MyChannel
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var argDic = GetArgs(args);
            var hasConfig = argDic.TryGetValue("-config", out var configJsonPath);

            var telegramBotBuilder = TelegramBuilder.Create().UseToken<AppSetting>(x => x.Token)
                .UseClashDefaultProxy()
                .AddServices((serviceCollection, buildService) =>
                {
                    var appSetting = buildService.GetService<AppSetting>()!;

                    _ = serviceCollection.AddDbContext<MyDBContext>(option =>
                    {
                        _ = option.EnableServiceProviderCaching();
                        _ = option.UseSqlite(appSetting.DatabaseSetting!.ConnectionString);
                    });

                    _ = serviceCollection.AddSingleton<IStartExec, StartService>();
                    _ = serviceCollection.AddSingleton<IExec, PublishService>();
                    _ = serviceCollection.AddSingleton<IExec, ScheduledService>();

                    _ = serviceCollection.RemoveAll<IAuthenticate>();
                    _ = serviceCollection.AddScoped<IAuthenticate, BlockUserAuth>();
                });

            if (hasConfig)
                _ = telegramBotBuilder.AddConfiguration<AppSetting>(configJsonPath);

            var telegramBot = telegramBotBuilder.RegisterBotCommand()
                .AddSimpleConsole()
                .AddLogger((logbuilder, buildService) =>
                {
                    var appSetting = buildService.GetService<AppSetting>()!;

                    LoggingConfiguration setting = new();
                    setting.AddTarget(new FileTarget
                    {
                        Name = "FileLog",
                        AutoFlush = true,
                        FileName = Path.Combine(appSetting.LogSetting!.LogPath!, "${date:format=yyyy-MM-dd}.log")
                    });
                    setting.AddRule(LogLevel.Info, LogLevel.Fatal, "FileLog");
                    _ = logbuilder.AddNLog(setting);
                })
                .Build();

            var botTask = telegramBot.StartAsync();
            botTask.Wait();
        }

        private static Dictionary<string, string> GetArgs(params string[] args)
        {
            Dictionary<string, string> dic = [];
            for (var i = 0; i < args.Length; i++)
            {
                var arg = args[i];
                if (!arg.StartsWith('-'))
                    continue;

                dic.Add(arg.ToLower(), args[++i].ToLower());
            }
            return dic;
        }
    }
}
