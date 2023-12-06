using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyChannel.DataBaseContext;
using System.Text.Json;
using Telegram.Bot;
using Telegram.Bot.Framework.Exec;
using Telegram.Bot.Types;

namespace MyChannel.Services
{
    /// <summary>
    /// 计划任务服务
    /// </summary>
    internal class ScheduledService : AbsScheduledTasks
    {
        private readonly ILogger<ScheduledService> _logger;
        private readonly MyDBContext dBContext;
        private readonly ITelegramBotClient _botClient;

        public ScheduledService(IServiceProvider serviceProvider)
        {
            _logger = serviceProvider.GetRequiredService<ILogger<ScheduledService>>();
            dBContext = serviceProvider.GetRequiredService<MyDBContext>();
            _botClient = serviceProvider.GetRequiredService<ITelegramBotClient>();

            DayLoop = true;

            var time = DateTime.Today;

            SetSchedule(time.AddHours(8));     // 早上8点
            SetSchedule(time.AddHours(12));    // 中午12点
            SetSchedule(time.AddHours(19));    // 晚上7点
            SetSchedule(time.AddHours(20));    // 晚上8点
            SetSchedule(time.AddHours(22));    // 晚上10点
            SetSchedule(time.AddHours(23));    // 晚上11点
        }

        protected override DateTime NextInvokeTime()
        {
            var nextTime = base.NextInvokeTime();
            _logger.LogInformation("下一次计划任务执行时间：{A0}", nextTime.ToString("yyyy 年 MM 月 dd 日 HH:mm:ss"));
            return nextTime;
        }

        /// <summary>
        /// 开始执行频道推送
        /// </summary>
        /// <returns></returns>
        protected override async Task Exec()
        {
            _logger.LogInformation("开始执行计划任务");

            var channelList = dBContext.ChannelInfoEntity.ToList();
            if (channelList.Count != 0)
            {
                _logger.LogInformation("开始发送频道消息");
            }
            foreach (var channel in channelList)
            {
                _logger.LogInformation("频道用户名：{A0}，名称：{A1}，频道ID：{A2}", channel.ChannelUsername, channel.ChannelName, channel.ChatID);

                // 获取等待发送的消息
                var messageList = dBContext.MessageInfoEntity
                    .Where(x => x.WaitForSend && x.ChannelInfo != null && x.ChannelInfo.Key == channel.Key)
                    .Include(x => x.FileIDs)
                    .Include(x => x.Images)
                    .Include(x => x.Tags)
                    .Take(Random.Shared.Next(10, 20))
                    .ToList();

                _logger.LogInformation("找到等待发送的消息数量：{A1}条", messageList.Count);

                foreach (var message in messageList)
                {
                    try
                    {
                        // 发送消息
                        if (message.Images != null && message.Images.Count != 0)
                        {
                            var images = new List<InputMediaPhoto>();
                            images = message.Images.Select(x =>
                            {
                                var inputFile = x.FileID != null
                                    ? new InputFileId(x.FileID)
                                    : (InputFile)new InputFileStream(new FileStream(x.ImagePath!, FileMode.Open), Path.GetFileName(x.ImagePath));
                                return new InputMediaPhoto(inputFile);
                            }).ToList();

                            images[0].Caption = message.Content;
                            images[0].ParseMode = Telegram.Bot.Types.Enums.ParseMode.Html;

                            var resultMessage = await _botClient.SendMediaGroupAsync(channel.ChatID, images);
                            message.MessageID = resultMessage.Select(x => x.MessageId).ToList();
                        }
                        else if (message.FileIDs != null && message.FileIDs.Count != 0)
                        {
                            var resultMessage = await _botClient.SendTextMessageAsync(channel.ChatID, message.Content ?? string.Empty, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                            foreach (var item in message.FileIDs)
                            {
                                var document = JsonSerializer.Deserialize<Document>(System.IO.File.ReadAllText(item.JSONPath!));
                                if (document != null)
                                    _ = _botClient.SendDocumentAsync(channel.ChatID, new InputFileId(document.FileId), replyToMessageId: resultMessage.MessageId);
                            }
                            message.MessageID = new List<int>(resultMessage.MessageId);
                        }
                        else
                        {
                            var resultMessage = await _botClient.SendTextMessageAsync(channel.ChatID, message.Content ?? string.Empty, parseMode: Telegram.Bot.Types.Enums.ParseMode.Html);
                            message.MessageID = new List<int>(resultMessage.MessageId);
                        }

                        // 更新数据库条目
                        if (message.MessageID.Count > 0)
                        {
                            message.UpdateTime = DateTime.Now;
                            message.WaitForSend = false;
                            message.WaitingForUpdate = false;
                        }
                    }
                    catch (Exception)
                    {
                        _logger.LogError("发送消息：{A0} 时发生错误", message.Key);
                        continue;
                    }
                }
            }
            await dBContext.SaveChangesAsync();

            _logger.LogInformation("结束执行计划任务");

            await Task.CompletedTask;
        }
    }
}
