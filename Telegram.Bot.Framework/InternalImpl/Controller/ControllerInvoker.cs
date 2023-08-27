using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controller;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework.InternalImpl.Controller
{
    [DependencyInjection(ServiceLifetime.Scoped, typeof(IControllerInvoker))]
    internal class ControllerInvoker : IControllerInvoker
    {
        private readonly IServiceProvider serviceProvider;

        public ControllerInvoker(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }

        public BotCommand GetCommand(Types.Update update)
        {
            BotCommand command = BotCommandRoute.GetBotCommand(update.GetCommand());
            if(command == null)
                return null;
            return command;
        }

        public async Task InvokeAsync(BotCommand command, TGChat tGChat)
        {
            if (command == null) return;

            TelegramController telegramController = (TelegramController)ActivatorUtilities.GetServiceOrCreateInstance(serviceProvider, command.ControllerType);
            //(TelegramController)ActivatorUtilities.CreateInstance(serviceProvider, command.ControllerType, Array.Empty<object>());
            telegramController.ControllerInvoke(tGChat);
            if (command.Command(telegramController, Array.Empty<object>()) is Task task)
                await task;
        }
    }
}
