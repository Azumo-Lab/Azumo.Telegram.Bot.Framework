using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel;
using System.Linq;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.Controller.Results;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram.Bot.Framework.Controller
{
    [DependencyInjection(ServiceLifetime.Scoped, ServiceType = typeof(ICallBackManager))]
    internal class CallBackManager : ICallBackManager
    {
        private readonly IServiceProvider serviceProvider;
        public CallBackManager(IServiceProvider serviceProvider) => 
            this.serviceProvider = serviceProvider;

        public InlineKeyboardButton CreateCallBackButton(ActionButtonResult buttonResult)
        {
            var callbackData = $"c{Guid.NewGuid().ToString().ToLower().Replace("-", string.Empty)}" ;

            var manager = serviceProvider.GetRequiredService<ICommandManager>();
            TypeDescriptor.AddAttributes(buttonResult.Delegate.Method, new BotCommandAttribute(callbackData));
            manager.AddExecutor(new FuncInvoker(
                buttonResult.Delegate,
                buttonResult.Delegate.Method.GetParameters().Select(x => x.GetParams()).ToList(),
                TypeDescriptor.GetAttributes(buttonResult.Delegate.Method).Cast<Attribute>().ToArray()));

            return InlineKeyboardButton.WithCallbackData(buttonResult.Text, callbackData);
        }
        public IExecutor? GetCallBack(TelegramRequest telegramRequest)
        {
            var data = telegramRequest.CallbackQuery?.Data;
            if (string.IsNullOrEmpty(data))
                return null;

            var manager = serviceProvider.GetRequiredService<ICommandManager>();
            return manager.GetExecutor($"/{data}");
        }
    }
}
