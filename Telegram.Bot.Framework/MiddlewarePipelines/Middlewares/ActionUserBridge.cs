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

using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.InternalImplementation.Sessions;

namespace Telegram.Bot.Framework.MiddlewarePipelines.Middlewares
{
    /// <summary>
    /// 用户桥处理程序，用于处理用户发送到机器人的信息
    /// </summary>
    public class ActionUserBridge : IMiddleware
    {
        public async Task Execute(ITelegramSession session, MiddlewareHandle NextHandle)
        {
            //TelegramUser telegramUser = session.User;
            //IUserBridgeManager userBridgeManager = session.UserService.GetService<IUserBridgeManager>();
            //if (userBridgeManager.HasUserBrige(telegramUser))
            //{
            //    IUserBridge userBridge = userBridgeManager.GetUserBridge(telegramUser);
            //    //判断是否是关闭桥的指令
            //    if (IsCloseCommand(session))
            //    {
            //        await userBridge.Disconnect();
            //        userBridge.Dispose();
            //        return;
            //    }

            //    //桥处理（信息等处理程序）
            //    await userBridge.Send(session.Update.Message.Text);
            //    return;
            //}

            await NextHandle(session);
        }

        //private static bool IsCloseCommand(TelegramSession Context)
        //{
        //    return Context.GetCommand().ToLower() == "/bridgeclose";
        //}
    }
}
