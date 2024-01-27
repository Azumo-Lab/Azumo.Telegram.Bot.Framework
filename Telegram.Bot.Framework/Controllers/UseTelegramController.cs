using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.Controllers;
internal class UseTelegramController : ITelegramPartCreator
{
    public void AddBuildService(IServiceCollection services) => throw new NotImplementedException();
    public void Build(IServiceCollection services, IServiceProvider builderService) => throw new NotImplementedException();
}
