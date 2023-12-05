using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyChannel.DataBaseContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.InternalInterface;
using Telegram.Bot.Types;

namespace MyChannel.Controllers
{
    internal class ReceiveController(IServiceProvider serviceProvider) : TelegramController
    {
        private readonly MyDBContext dBContext = serviceProvider.GetRequiredService<MyDBContext>();
        private readonly ILogger<ReceiveController> logger = serviceProvider.GetRequiredService<ILogger<ReceiveController>>();
        private readonly AppSetting appSetting = serviceProvider.GetRequiredService<AppSetting>();

        [BotCommand("Receive", Description = "发送文件")]
        public async Task Receive([Param(ControllerParamSenderType = typeof(DocumentSender))]Document document)
        {
            if (document == null)
            {
                await SendMessage("文件获取失败，请重新发送指令");
                return;
            }

            string filePath;
            string json = JsonSerializer.Serialize(document);
            System.IO.File.WriteAllText(filePath = Path.Combine(appSetting.FileSetting!.TelegramJsonIDPath!, document!.FileUniqueId), json);

            logger.LogInformation("接收到文件, 文件保存路径: {A0}", filePath);

            dBContext.FileIDs.Add(new DataBaseContext.DBModels.FileIDInfos
            {
                JSONPath = filePath,
                UpdateTime = DateTime.Now,
            });
            dBContext.SaveChanges();

            _ = SendMessage($"已经接收成功, 文件名: {document.FileName}").ConfigureAwait(false);
        }
    }

    [TypeFor(typeof(Document))]
    internal class DocumentSender : BaseControllerParam, IControllerParamSender
    {
        public override IControllerParamSender? ParamSender { get => this; set { _ = value; } }

        public override Task<object> CatchObjs(TGChat tGChat)
        {
            return Task.FromResult<object>(tGChat.Message?.Document!);
        }

        public async Task Send(ITelegramBotClient botClient, ChatId chatId)
        {
            await botClient.SendTextMessageAsync(chatId, "请发送文件");
        }
    }
}
