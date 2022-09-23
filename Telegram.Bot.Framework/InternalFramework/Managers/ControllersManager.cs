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
using Telegram.Bot.Framework.InternalFramework.FrameworkHelper;
using Telegram.Bot.Framework.InternalFramework.InterFaces;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.InternalFramework.Managers
{
    /// <summary>
    /// 
    /// </summary>
    internal class ControllersManager : IControllersManager, IDelegateManager
    {
        private readonly IServiceProvider serviceProvider;
        private readonly ITypeManager typeManger;

        public ControllersManager(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            typeManger = this.serviceProvider.GetService<ITypeManager>();
        }

        public Delegate CreateDelegate(string CommandName)
        {
            if (typeManger.ContainsCommandName(CommandName))
                return CreateDelegate(CommandName, GetController(CommandName));
            return null;
        }

        public Delegate CreateDelegate(string CommandName, object controller)
        {
            if (typeManger.ContainsCommandName(CommandName) && controller != null)
                return DelegateHelper.CreateDelegate(typeManger.GetControllerMethod(CommandName), controller);
            return null;
        }

        public Delegate CreateDelegate(CommandInfos type, object controller)
        {
            return DelegateHelper.CreateDelegate(type.CommandMethod, controller);
        }

        public object GetController(string CommandName)
        {
            if (CommandName == null)
                return null;
            if (typeManger.ContainsCommandName(CommandName))
                return serviceProvider.GetService(typeManger.GetControllerType(CommandName));
            return null;
        }

        public object GetController(MessageType MessageType, List<Type> ParamType)
        {
            CommandInfos commandinfo = GetMessageTypeCommandInfos(MessageType, ParamType);
            if (commandinfo == null)
                return null;
            return serviceProvider.GetService(commandinfo.Controller);
        }

        public CommandInfos GetMessageTypeCommandInfos(MessageType MessageType, List<Type> ParamType)
        {
            CommandInfos result = null;
            List<CommandInfos> commandInfos = typeManger.GetMessageController(MessageType);
            commandInfos = commandInfos.Where(x => x.ParamInfos.Count == ParamType.Count).ToList();
            if (commandInfos.Count > 1)
            {
                commandInfos = commandInfos.Where(x =>
                {
                    for (int i = 0; i < ParamType.Count; i++)
                        if (ParamType[i].FullName != x.ParamInfos[i].ParamType.FullName)
                            return false;
                    return true;
                }).ToList();
                result = commandInfos.FirstOrDefault();
            }
            else if (commandInfos.Any())
            {
                result = commandInfos.FirstOrDefault();
            }
            if (result == null)
                return null;
            return result;
        }

        public bool HasCommand(string CommandName)
        {
            return typeManger.ContainsCommandName(CommandName);
        }
    }
}
