using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyChannel.DataBaseContext;
using MyChannel.DataBaseContext.DBModels;
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
            AppSetting appSetting = serviceProvider.GetRequiredService<AppSetting>();
            if (appSetting == null)
                return;

            ILogger<StartService> logger = serviceProvider.GetService<ILogger<StartService>>()!;

            using (IServiceScope serviceScope = serviceProvider.CreateScope())
            {
                MyDBContext dBContext = serviceScope.ServiceProvider.GetRequiredService<MyDBContext>();

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
