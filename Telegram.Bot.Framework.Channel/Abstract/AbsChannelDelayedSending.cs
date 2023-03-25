using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timer = System.Timers.Timer;
using Telegram.Bot.Framework.Channel.Models;
using Telegram.Bot.Framework.Helper;

namespace Telegram.Bot.Framework.Channel.Abstract
{
    public abstract class AbsChannelDelayedSending : AbsBackgroundWorker
    {
        private ITelegramBotClient _botClient = default!;
        private List<(long chatID, Photo photo, DateTime dateTime)> values = default!;
        private DateTime _lastSendTime = DateTime.MinValue;
        private int __ExecutionInterval;
        public int ExecutionInterval
        {
            get
            {
                return __ExecutionInterval;
            }
            set
            {
                __ExecutionInterval = value;
                _timer.Interval = __ExecutionInterval;
            }
        }

        private readonly Timer _timer = new Timer();

        public void SetDatas(ITelegramBotClient botClient, List<(long chatID, Photo photo, DateTime dateTime)> values)
        {
            if(_botClient.IsNull())
                _botClient = botClient;
            if(values.IsNull())
                this.values = values;
            else
                values.AddRange(values);

            _timer.Elapsed += Timer_Elapsed;
        }

        private void Timer_Elapsed(object? sender, System.Timers.ElapsedEventArgs e)
        {
            List<(long chatID, Photo photo, DateTime dateTime)> copyDatas = new(values);

            DateTime Now = DateTime.Now;
            foreach ((long chatID, Photo photo, DateTime dateTime) in copyDatas)
            {
                if (_lastSendTime <= dateTime && dateTime <= Now)
                {
                    Invoke(_botClient, chatID, photo);
                }
            }
            _lastSendTime = Now;
        }

        public void Invoke(ITelegramBotClient telegramBotClient, long ChatID, Photo photo)
        {
            List<object> param = new List<object>
            {
                telegramBotClient, 
                ChatID,
                photo,
            };
            Invoke(param.ToArray());
        }

        public override void Invoke(params object[] objects)
        {
            if (!_timer.Enabled)
                _timer.Start();
            base.Invoke(objects);
        }
    }
}
