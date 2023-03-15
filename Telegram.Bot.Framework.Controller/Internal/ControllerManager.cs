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
using Telegram.Bot.Framework.Controller.Interface;
using Telegram.Bot.Framework.Controller.Models;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller.Internal
{
    /// <summary>
    /// 
    /// </summary>
    internal class ControllerManager : IControllerManager
    {
        private readonly IServiceProvider ServiceProvider;
        public ControllerManager(IServiceProvider ServiceProvider)
        {
            this.ServiceProvider = ServiceProvider;
        }

        public TelegramController GetController(string command, out CommandInfo commandInfo)
        {
            commandInfo = null!;
            IFrameworkInfo? frameworkInfo = ServiceProvider.GetService<IFrameworkInfo>();
            if (frameworkInfo!.IsNull())
                return default!;

            commandInfo = frameworkInfo!.GetCommandInfo(command);
            if (commandInfo.IsNull())
                return default!;

            return (TelegramController)ActivatorUtilities.CreateInstance(ServiceProvider, commandInfo.ControllerType!, Array.Empty<object>());
        }

        public TelegramController GetController(MessageType updateType, out CommandInfo commandInfo)
        {
            commandInfo = null!;
            IFrameworkInfo? frameworkInfo = ServiceProvider.GetService<IFrameworkInfo>();
            if (frameworkInfo!.IsNull())
                return default!;

            commandInfo = frameworkInfo!.GetCommandInfo(updateType);
            if (commandInfo.IsNull())
                return default!;

            return (TelegramController)ActivatorUtilities.CreateInstance(ServiceProvider, commandInfo.ControllerType!, Array.Empty<object>());
        }

        public TelegramController GetController(UpdateType messageType, out CommandInfo commandInfo)
        {
            commandInfo = null!;
            IFrameworkInfo? frameworkInfo = ServiceProvider.GetService<IFrameworkInfo>();
            if (frameworkInfo!.IsNull())
                return default!;

            commandInfo = frameworkInfo!.GetCommandInfo(messageType);
            if (commandInfo.IsNull())
                return default!;

            return (TelegramController)ActivatorUtilities.CreateInstance(ServiceProvider, commandInfo.ControllerType!, Array.Empty<object>());
        }
    }
}
