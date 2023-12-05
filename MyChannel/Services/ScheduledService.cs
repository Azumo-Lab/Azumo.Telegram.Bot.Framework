using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Abstracts.Exec;

namespace MyChannel.Services
{
    /// <summary>
    /// 计划任务服务
    /// </summary>
    internal class ScheduledService : AbsScheduledTasks
    {
        private readonly ILogger<ScheduledService> _logger;
        public ScheduledService(ILogger<ScheduledService> _logger)
        {
            this._logger = _logger;

            DayLoop = true;

            DateTime time = DateTime.Today;

            AddTime(time.AddHours(8));     // 早上8点
            AddTime(time.AddHours(12));    // 中午12点
            AddTime(time.AddHours(19));    // 晚上7点
            AddTime(time.AddHours(20));    // 晚上8点
            AddTime(time.AddHours(22));    // 晚上10点
            AddTime(time.AddHours(23));    // 晚上11点
        }

        private void AddTime(DateTime dateTime)
        {
            if (dateTime > DateTime.Now)
                Scheduled.Add(dateTime);
            else if (dateTime < DateTime.Now)
                Scheduled.Add(dateTime.AddDays(1));
        }

        protected override DateTime NextInvokeTime()
        {
            DateTime nextTime = base.NextInvokeTime();
            _logger.LogInformation("下一次计划任务执行时间：{A0}", nextTime.ToString("yyyy 年 MM 月 dd 日 HH:mm:ss"));
            return nextTime;
        }

        /// <summary>
        /// 开始执行频道推送
        /// </summary>
        /// <returns></returns>
        protected override Task Exec()
        {
            _logger.LogInformation("开始执行计划任务");
            return Task.CompletedTask;
        }
    }
}
