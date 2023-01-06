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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract;

namespace Telegram.Bot.Framework.UpdateTypeActions.Actions
{
    /// <summary>
    /// 用户桥处理程序，用于处理用户发送到机器人的信息
    /// </summary>
    public class ActionUserBridge : IAction
    {
        public async Task Invoke(TelegramContext Context, ActionHandle NextHandle)
        {
            TelegramUser telegramUser = Context.TelegramUser;
            IUserBridgeManager userBridgeManager = Context.UserScope.GetService<IUserBridgeManager>();
            if (userBridgeManager.HasUserBrige(telegramUser))
            {
                IUserBridge userBridge = userBridgeManager.GetUserBridge(telegramUser);
                //判断是否是关闭桥的指令
                if (IsCloseCommand(Context))
                {
                    userBridge.Disconnect();
                    userBridge.Dispose();
                    return;
                }

                //桥处理（信息等处理程序）
                userBridge.Send(Context.Update.Message.Text);
                return;
            }

            await NextHandle(Context);
        }

        private static bool IsCloseCommand(TelegramContext Context)
        {
            return Context.GetCommand().ToLower() == "/bridgeclose";
        }
    }
}
