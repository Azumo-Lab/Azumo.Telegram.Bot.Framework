using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Bots;

namespace Telegram.Bot.Framework.PaymentMethod.Bots
{
    internal class TelegramPiadRoles(string[] roles) : ITelegramPartCreator
    {
        private readonly List<string> __Roles = new(roles ?? []);
        public void AddBuildService(IServiceCollection services)
        {

        }
        public void Build(IServiceCollection services, IServiceProvider builderService)
        {

        }
    }
}
