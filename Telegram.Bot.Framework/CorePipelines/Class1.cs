using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.User;
using Telegram.Bot.Framework.Pipeline;
using Telegram.Bot.Framework.Pipeline.Abstracts;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.CorePipelines
{
    internal class Class1
    {
        public void Test()
        {
            IPipelineController<IChat> pipelineController =
                PipelineFactory.CreateIPipelineBuilder<IChat>()
                               .AddProcedure(default)
                               .CreatePipeline(UpdateType.Message)
                               .AddProcedure(default)
                               .CreatePipeline(UpdateType.CallbackQuery)
                               .BuilderPipelineController();

            pipelineController.SwitchTo(UpdateType.Message, default);
        }

    }
}
