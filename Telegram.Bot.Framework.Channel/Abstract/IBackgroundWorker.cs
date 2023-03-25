using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Channel.Abstract
{
    public interface IBackgroundWorker
    {
        public void Invoke(params object[] objects);
    }

}
