using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
            IConfiguration configuration = serviceProvider.GetRequiredService<IConfiguration>();
            AppSetting appSetting = (AppSetting)configuration.GetValue(typeof(AppSetting), nameof(AppSetting))!;

            if (appSetting == null)
                return;

            using (IServiceScope serviceScope = serviceProvider.CreateScope())
            {
                MyDBContext dBContext = serviceScope.ServiceProvider.GetRequiredService<MyDBContext>();

                // 创建数据库
                if (!dBContext.Database.EnsureCreated())
                    return;

                DirectoryInfo sendImagePathDir;
                string? sendImagePath = appSetting.ImagesInfo?.SendImagePath;
                if (!string.IsNullOrEmpty(sendImagePath))
                    sendImagePathDir = Directory.CreateDirectory(sendImagePath);

                DirectoryInfo imagePathDir;
                string? imagePath = appSetting.ImagesInfo?.ImagePath;
                if (!string.IsNullOrEmpty(imagePath))
                    imagePathDir = Directory.CreateDirectory(imagePath);

                _ = dBContext.Images.Add(new ImageInfo
                {

                });

                _ = await dBContext.SaveChangesAsync();
            }
        }
    }
}
