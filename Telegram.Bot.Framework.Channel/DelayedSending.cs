using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Channel.Abstract;
using Telegram.Bot.Framework.Channel.Models;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.Channel
{
    public class DelayedSending : AbsChannelDelayedSending
    {
        protected override void BackgroundWorker_DoWork(object? sender, DoWorkEventArgs e)
        {
            if (e.Argument is not object[] param) return;
            if (param.IsEmpty()) return;
            // 获得IServiceProvider
            if (param[0] is not ITelegramBotClient telegramBotClient) return;
            if (telegramBotClient.IsNull()) return;

            // 获得ChatID
            if (param[1] is not long chatID) return;

            // 获得发送信息
            if (param[2] is not Photo photo) return;
            if (photo.IsNull()) return;

            telegramBotClient.SendPhotoAsync(chatID, new Types.InputFiles.InputOnlineFile(new MemoryStream(photo.PhotoBytes)), photo.Context, Types.Enums.ParseMode.MarkdownV2);
        }
    }
}
