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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller.Interface;
using Telegram.Bot.Framework.Controller.Models;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller.Internal
{
    /// <summary>
    /// 
    /// </summary>
    internal class CommandInvoker : InvokeBase, ICommandInvoker
    {
        private readonly IControllerManager ControllerManager;
        public CommandInvoker(IControllerManager controllerManager)
        {
            ControllerManager = controllerManager;
        }

        public async Task CommandInvoke(string command, params object[] param)
        {
            TelegramController telegramController = ControllerManager.GetController(command, out CommandInfo commandInfo);
            if (telegramController.IsNull() || commandInfo.IsNull())
                return;

            await CommandInvoke(commandInfo, telegramController, param);
        }

        public async Task CommandInvoke(MessageType updateType, params object[] param)
        {
            TelegramController telegramController = ControllerManager.GetController(updateType, out CommandInfo commandInfo);
            if (telegramController.IsNull() || commandInfo.IsNull())
                return;

            await CommandInvoke(commandInfo, telegramController, param);
        }
    }
}
