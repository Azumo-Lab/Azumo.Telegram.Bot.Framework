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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework.InterFaces;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.InternalFramework
{
    /// <summary>
    /// 
    /// </summary>
    internal class TelegramRouteController
    {
        private readonly IServiceScope OneTimeScope;
        private readonly IServiceScope UserScope;

        private readonly IControllersManger controllersManger;

        public TelegramRouteController(IServiceScope OneTimeScope, IServiceScope UserScope)
        {
            this.OneTimeScope = OneTimeScope;
            this.UserScope = UserScope;

            controllersManger = this.UserScope.ServiceProvider.GetService<IControllersManger>();
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <returns></returns>
        public async Task StartProcess()
        {
            //Task taskResult = context.Update.Type switch
            //{
            //    // 未知消息
            //    UpdateType.Unknown => Task.Run(() => { }),
            //    // 消息
            //    UpdateType.Message => throw new NotImplementedException(),
            //    UpdateType.InlineQuery => throw new NotImplementedException(),
            //    UpdateType.ChosenInlineResult => throw new NotImplementedException(),
            //    UpdateType.CallbackQuery => throw new NotImplementedException(),
            //    UpdateType.EditedMessage => throw new NotImplementedException(),
            //    UpdateType.ChannelPost => throw new NotImplementedException(),
            //    UpdateType.EditedChannelPost => throw new NotImplementedException(),
            //    UpdateType.ShippingQuery => throw new NotImplementedException(),
            //    UpdateType.PreCheckoutQuery => throw new NotImplementedException(),
            //    UpdateType.Poll => throw new NotImplementedException(),
            //    UpdateType.PollAnswer => throw new NotImplementedException(),
            //    UpdateType.MyChatMember => throw new NotImplementedException(),
            //    UpdateType.ChatMember => throw new NotImplementedException(),
            //    UpdateType.ChatJoinRequest => throw new NotImplementedException(),
            //    _ => throw new NotImplementedException(),
            //};

            await Authentication();

            await FiltersBefore();

            if (await ParamCatch())
                return;

            if (await ControllerInvoke())
                return;

            await FiltersAfter();
        }

        private async Task Authentication()
        {

        }

        private async Task FiltersBefore()
        {

        }

        private async Task<bool> ParamCatch()
        {
            // 获取Context
            TelegramContext context = OneTimeScope.ServiceProvider.GetService<TelegramContext>();

            TelegramUser user = UserScope.ServiceProvider.GetService<TelegramUser>();
            user.ChatID = context.ChatID;

            // 获取参数管理
            IParamManger paramManger = UserScope.ServiceProvider.GetService<IParamManger>();

            return await paramManger.ReadParam(context, OneTimeScope.ServiceProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private async Task<bool> ControllerInvoke()
        {
            // 获取Context
            TelegramContext context = OneTimeScope.ServiceProvider.GetService<TelegramContext>();

            // 获取参数管理
            IParamManger paramManger = UserScope.ServiceProvider.GetService<IParamManger>();

            TelegramController controller = (TelegramController)controllersManger.GetController(paramManger.GetCommand());
            if (controller == null)
                return true;
            await controller.Invoke(context, OneTimeScope.ServiceProvider, UserScope.ServiceProvider, paramManger.GetCommand());
            return false;
        }

        private async Task FiltersAfter()
        {

        }
    }
}
