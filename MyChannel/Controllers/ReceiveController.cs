using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyChannel.DataBaseContext;
using MyChannel.DataBaseContext.DBModels;
using MyChannel.Senders;
using System.Text.Json;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Types;

namespace MyChannel.Controllers
{
    internal class ReceiveController(IServiceProvider serviceProvider) : TelegramController
    {
        private readonly MyDBContext dBContext = serviceProvider.GetRequiredService<MyDBContext>();
        private readonly ILogger<ReceiveController> logger = serviceProvider.GetRequiredService<ILogger<ReceiveController>>();
        private readonly AppSetting appSetting = serviceProvider.GetRequiredService<AppSetting>();

        [BotCommand("Receive", Description = "发送文件")]
        public async Task Receive([Param(Sender = typeof(DocumentSender), Name = "需要上传的文件")] Document document)
        {
            if (document == null)
            {
                _ = await SendMessage("文件获取失败，请重新发送指令");
                return;
            }

            string filePath;
            var json = JsonSerializer.Serialize(document);
            System.IO.File.WriteAllText(filePath = Path.Combine(appSetting.FileSetting!.TelegramJsonIDPath!, document!.FileUniqueId), json);

            logger.LogInformation("接收到文件, 文件保存路径: {A0}", filePath);

            _ = dBContext.FileIDInfoEntity.Add(new FileIDInfoEntity
            {
                JSONPath = filePath,
                UpdateTime = DateTime.Now,
                WaitingForUpdate = true,
            });
            _ = dBContext.SaveChanges();

            _ = SendMessage($"已经接收成功, 文件名: {document.FileName}").ConfigureAwait(false);
        }
    }
}
