using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Telegram.Bot.Framework.Abstracts.Bots
{
    public interface ITelegramPartCreator
    {
        public void AddBuildService(IServiceCollection services);

        public void Build(IServiceCollection services, IServiceProvider builderService);
    }
}
