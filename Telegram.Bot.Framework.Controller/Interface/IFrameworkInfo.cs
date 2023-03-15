using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller.Models;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller.Interface
{
    public interface IFrameworkInfo
    {
        public Task CommandInvoke(IServiceProvider serviceProvider, string command, params object[] param);

        public Task CommandInvoke(IServiceProvider serviceProvider, UpdateType updateType, params object[] param);

        public TelegramController GetController(IServiceProvider serviceProvider, string command);

        public List<CommandInfo> GetCommandInfos();
    }
}
