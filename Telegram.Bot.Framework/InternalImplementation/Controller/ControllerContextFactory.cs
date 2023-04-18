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
using Telegram.Bot.Framework.Abstract.Controller;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.Attributes;
using Telegram.Bot.Framework.ExtensionMethods;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.InternalImplementation.Controller
{
    /// <summary>
    /// 
    /// </summary>
    [DependencyInjection(ServiceLifetime.Singleton)]
    internal class ControllerContextFactory : IControllerContextFactory
    {
        /// <summary>
        /// 指令类型对应
        /// </summary>
        private static readonly Dictionary<string, IControllerContext> ControllerContextMapCommandName = new();
        private static readonly Dictionary<MessageType, IControllerContext> ControllerContextMapMessageType = new();
        private static readonly Dictionary<UpdateType, IControllerContext> ControllerContextMapUpdateType = new();

        public void AddContext(IControllerContextBuilder controllerContextBuilder)
        {
            IControllerContext controllerContext = controllerContextBuilder.Build();

            if (controllerContext.BotCommandAttribute != null)
                ControllerContextMapCommandName.Add(controllerContext.BotCommandAttribute.Command.ToLower(), controllerContext);
            if (controllerContext.DefaultMessageAttribute != null)
                ControllerContextMapMessageType.Add(controllerContext.DefaultMessageAttribute.MessageType, controllerContext);
            if (controllerContext.DefaultTypeAttribute != null)
                ControllerContextMapUpdateType.Add(controllerContext.DefaultTypeAttribute.UpdateType, controllerContext);
        }

        public IControllerContext CreateControllerContext(ITelegramSession telegramSession)
        {
            string Command;
            IControllerContext controllerContext;
            Command = telegramSession.GetCommand();
            if(Command == null)
            {
                Command = telegramSession.Session.GetCommand();
            }
            else
            {
                telegramSession.Session.SetCommand(Command);
            }

            if (Command == null)
            {
                if (ControllerContextMapMessageType.TryGetValue(telegramSession.Update.Message.Type, out controllerContext))
                    return controllerContext;
                else if (ControllerContextMapUpdateType.TryGetValue(telegramSession.Update.Type, out controllerContext))
                    return controllerContext;
            }
            else
            {
                if (ControllerContextMapCommandName.TryGetValue(Command.ToLower(), out controllerContext))
                    return controllerContext;
            }
            return default;
        }
    }
}
