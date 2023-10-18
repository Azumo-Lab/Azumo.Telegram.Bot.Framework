using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Pipeline.Abstracts
{
    public enum PipelineResultEnum
    {
        Success,
        Failure,
        TryAgain,
    }
}
