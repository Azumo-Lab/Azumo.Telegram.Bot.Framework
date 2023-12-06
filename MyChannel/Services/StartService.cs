using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyChannel.DataBaseContext;
using Telegram.Bot;
using Telegram.Bot.Framework.Abstracts.Exec;

namespace MyChannel.Services
{
    /// <summary>
    /// 系统启动
    /// </summary>
    internal class StartService : IStartExec
    {
        public async Task Exec(ITelegramBotClient bot, IServiceProvider serviceProvider)
        {
            var appSetting = serviceProvider.GetRequiredService<AppSetting>();
            if (appSetting == null)
                return;

            var logger = serviceProvider.GetService<ILogger<StartService>>()!;

            using (var serviceScope = serviceProvider.CreateScope())
            {
                var dBContext = serviceScope.ServiceProvider.GetRequiredService<MyDBContext>();

                // 创建数据库
                if (dBContext.Database.EnsureCreated())
                {
                    logger.LogInformation("创建数据库");
                }
                else
                {
                    logger.LogInformation("数据库已存在");
                }
            }

            await Task.CompletedTask;
        }
    }
}
