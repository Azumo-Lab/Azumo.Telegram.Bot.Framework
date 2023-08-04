//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Microsoft.Extensions.DependencyInjection;
using System;
using Telegram.Bot.Framework.Abstracts;
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

        #region 销毁相关

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; } = DateTime.Now;

        /// <summary>
        /// 属于活跃用户
        /// </summary>
        public TimeSpan HotHitTimeSpan { get; } = TimeSpan.FromHours(24);

        /// <summary>
        /// 访问时间
        /// </summary>
        public DateTime VisitTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 是否需要移除
        /// </summary>
        /// <returns></returns>
        public bool HasRemove()
        {
            return VisitTime - CreateTime < TimeSpan.FromDays(5) && VisitTime + HotHitTimeSpan < DateTime.Now;
        }
        #endregion

        public void Dispose()
        {
            __ServiceScope.Dispose();
        }
    }
}
