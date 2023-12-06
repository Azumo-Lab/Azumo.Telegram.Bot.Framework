using Microsoft.Extensions.Logging;
using Telegram.Bot.Framework.Exec;

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
        protected override Task Exec()
        {
            _logger.LogInformation("开始执行计划任务");
            return Task.CompletedTask;
        }
    }
}
