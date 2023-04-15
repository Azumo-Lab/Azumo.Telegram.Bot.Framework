using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Controller;
using Telegram.Bot.Framework.Attributes;

namespace Telegram.Bot.Framework.InternalImplementation.Controller
{
    [DependencyInjection(ServiceLifetime.Scoped)]
    internal class ControllerFactory : IControllerFactory
    {
        private readonly IServiceProvider _serviceProvider;
        public ControllerFactory(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public TelegramController CreateController(IControllerContext context)
        {
            return (TelegramController)ActivatorUtilities.CreateInstance(_serviceProvider, context.ControllerType, Array.Empty<object>());
        }
    }
}
