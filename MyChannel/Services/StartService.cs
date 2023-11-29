using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MyChannel.DataBaseContext;
using MyChannel.DataBaseContext.DBModels;
using Telegram.Bot;
using Telegram.Bot.Framework.Interfaces;

namespace MyChannel.Services
{
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
                _ = dBContext.Database.EnsureCreated();

                DirectoryInfo sendImagePathDir;
                string? sendImagePath = appSetting.ImagesInfo?.SendImagePath;
                if (!string.IsNullOrEmpty(sendImagePath))
                    sendImagePathDir = Directory.CreateDirectory(sendImagePath);

                DirectoryInfo imagePathDir;
                string? imagePath = appSetting.ImagesInfo?.ImagePath;
                if (!string.IsNullOrEmpty(imagePath))
                    imagePathDir = Directory.CreateDirectory(imagePath);

                dBContext.Images.Add(new ImageInfo
                {

                });

                await dBContext.SaveChangesAsync();
            }
        }
    }
}
