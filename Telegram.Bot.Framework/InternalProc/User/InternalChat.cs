using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Services;
using Telegram.Bot.Framework.Abstracts.User;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalProc.User
{
    internal class InternalChat : IChat
    {
        public InternalChat(ITelegramBotClient botClient, Update update, IServiceScope BotService)
        {
            BotClient = botClient;
            this.BotService = BotService.ServiceProvider;
            __ServiceScope = this.BotService.CreateScope();

            Request = new InternalRequest(update);
            ChatInfo = new InternalChatInfo(update.GetChat(), update.GetChatUser());

            SessionCache = ChatService.GetService<ISessionCache>();
            CallbackService = ChatService.GetService<ICallbackService>();
            CommandService = ChatService.GetService<ICommandService>();
            AuthenticationService = ChatService.GetService<IAuthenticationService>();
            TaskService = ChatService.GetService<ITaskService>();
        }

        private readonly IServiceScope __ServiceScope;

        public ISessionCache SessionCache { get; set; }

        public IRequest Request { get; set; }

        public IChatInfo ChatInfo { get; set; }

        public IServiceProvider BotService { get; set; }

        public IServiceProvider ChatService => __ServiceScope.ServiceProvider;

        public ICallbackService CallbackService { get; set; }

        public ICommandService CommandService { get; set; }

        public IAuthenticationService AuthenticationService { get; set; }

        public ITaskService TaskService { get; set; }

        public ITelegramBotClient BotClient { get; set; }

        public void Dispose()
        {
            __ServiceScope.Dispose();
        }
    }
}
