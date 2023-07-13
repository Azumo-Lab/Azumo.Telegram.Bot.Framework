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
using Telegram.Bot.Framework.Abstract.CallBack;
using Telegram.Bot.Framework.Abstract.Commands;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.InternalImplementation.Sessions
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Transient, ServiceType = typeof(ITelegramChat))]
    internal class InternalTelegramChat : ITelegramChat
    {
        private Update __Update;
        public Update Update
        {
            get
            {
                return __Update;
            }
            set
            {
                __Update = value;
                switch (__Update.Type)
                {
                    case Types.Enums.UpdateType.Unknown:
                        break;
                    case Types.Enums.UpdateType.Message:
                        break;
                    case Types.Enums.UpdateType.InlineQuery:
                        break;
                    case Types.Enums.UpdateType.ChosenInlineResult:
                        break;
                    case Types.Enums.UpdateType.CallbackQuery:
                        CallbackQuery CallBackQuery = __Update.CallbackQuery;
                        CallBackManager.SetCallBackKey(CallBackQuery.Data);
                        break;
                    case Types.Enums.UpdateType.EditedMessage:
                        break;
                    case Types.Enums.UpdateType.ChannelPost:
                        break;
                    case Types.Enums.UpdateType.EditedChannelPost:
                        break;
                    case Types.Enums.UpdateType.ShippingQuery:
                        break;
                    case Types.Enums.UpdateType.PreCheckoutQuery:
                        break;
                    case Types.Enums.UpdateType.Poll:
                        break;
                    case Types.Enums.UpdateType.PollAnswer:
                        break;
                    case Types.Enums.UpdateType.MyChatMember:
                        break;
                    case Types.Enums.UpdateType.ChatMember:
                        break;
                    case Types.Enums.UpdateType.ChatJoinRequest:
                        break;
                    default:
                        break;
                }
            }
        }
        public IServiceProvider ChatService => __ChatScope.ServiceProvider;
        public IServiceProvider BotService { get; set; }
        public ITelegramBotClient BotClient { get; set; }

        public ISession Session { get; set; }

        public ICallBackService CallBackManager { get; set; }

        public ICommandService CommandService { get; set; }

        public Chat Chat { get; set; }

        private readonly IServiceScope __ChatScope;

        public InternalTelegramChat(IServiceProvider BotService)
        {
            this.BotService = BotService;
            __ChatScope = BotService.CreateScope();

            Session = ChatService.GetService<ISession>();
            CallBackManager = ChatService.GetService<ICallBackService>();
            CommandService = ChatService.GetService<ICommandService>();
        }

        public void Dispose()
        {
            Session?.Dispose();
            __ChatScope?.Dispose();

            GC.SuppressFinalize(this);
        }
    }
}
