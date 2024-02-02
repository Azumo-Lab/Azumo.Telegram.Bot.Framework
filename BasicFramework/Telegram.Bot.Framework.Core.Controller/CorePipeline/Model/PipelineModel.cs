using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Controller.Controller;
using Telegram.Bot.Framework.Core.Storage;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Core.Controller.CorePipeline.Model;
internal class PipelineModel
{
    public TelegramUserContext UserContext { get; set; } = null!;

    public ICommandManager CommandManager { get; set; } = null!;

    public ICommandScopeService CommandScopeService { get; set; } = null!;
}
