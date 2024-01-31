using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Controller.ControllerInvoker;

namespace Telegram.Bot.Framework.Controller.InvokePipeline;
internal class PipelineInvokeModel
{
    public TelegramUserChatContext Context { get; set; } = null!;

    public BotCommandManager CommandManager { get; set; } = null!;
}
