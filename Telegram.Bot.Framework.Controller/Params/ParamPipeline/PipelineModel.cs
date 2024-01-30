using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Controller.Params.ParamPipeline;
internal class PipelineModel
{
    public List<object> ParamList { get; set; } = null!;

    public BaseParameterGetter ParameterGetter { get; set; } = null!;

    public TelegramUserChatContext UserChatContext { get; set; } = null!;
}
