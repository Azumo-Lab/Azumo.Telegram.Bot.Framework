//  <Telegram.Bot.Framework>
//  Copyright (C) <2022>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;

namespace Telegram.Bot.Framework.UpdateTypeActions.Actions
{
    /// <summary>
    /// 用于处理频道群组消息的Action
    /// </summary>
    public class ActionGroupChannel : IAction
    {
        public async Task Invoke(TelegramContext Context, ActionHandle NextHandle)
        {
            BotConfig botConfig;
            IBotChatTypeProc botChatTypeProc;
            switch (Context.Update.Message.Chat.Type)
            {
                case Types.Enums.ChatType.Private:
                    IUserBridgeManager userBridgeManager = Context.UserScope.GetService<IUserBridgeManager>();
                    if (userBridgeManager.HasUserBrige(Context.TelegramUser))
                    {
                        IUserBridge userBridge = userBridgeManager.GetUserBridge(Context.TelegramUser);
                        userBridge.Send(Context.Update.Message.Text);
                        return;
                    }
                    break;
                case Types.Enums.ChatType.Group:
                    botChatTypeProc = Context.UserScope.GetService<IBotChatTypeProc>();
                    botChatTypeProc.Group(Context);
                    return;
                case Types.Enums.ChatType.Channel:
                    botConfig = Context.UserScope.GetService<BotConfig>();
                    botChatTypeProc = Context.UserScope.GetService<IBotChatTypeProc>();
                    botChatTypeProc.Channel(Context);
                    if (!botConfig.UseCommandInChannel)
                        return;
                    break;
                case Types.Enums.ChatType.Supergroup:
                    botConfig = Context.UserScope.GetService<BotConfig>();
                    botChatTypeProc = Context.UserScope.GetService<IBotChatTypeProc>();
                    botChatTypeProc.Group(Context);
                    if (!botConfig.UseCommandInGroup)
                        return;
                    break;
                case Types.Enums.ChatType.Sender:
                    break;
            }

            await NextHandle(Context);
        }
    }
}
