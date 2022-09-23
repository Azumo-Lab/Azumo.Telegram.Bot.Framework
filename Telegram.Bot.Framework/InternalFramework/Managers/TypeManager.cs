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
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Security.AccessControl;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework.FrameworkHelper;
using Telegram.Bot.Framework.InternalFramework.InterFaces;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Framework.InternalFramework.ParameterManager;
using Telegram.Bot.Framework.InternalFramework.TypeConfigs;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.InternalFramework.Managers
{
    /// <summary>
    /// 
    /// </summary>
    internal class TypeManager : ITypeManager
    {
        public Guid ID { get; } = Guid.NewGuid();
        private readonly Dictionary<string, CommandInfos> CommandInfos;
        private readonly Dictionary<MessageType?, List<CommandInfos>> MessageInfos;

        public TypeManager(IServiceCollection services)
        {
            TypesHelper.GetTypes<IAction>().ForEach(x => { services.AddScoped(typeof(IAction), x); });
            TypesHelper.GetTypes<IParamMaker>().ForEach(x => { services.AddScoped(x); });
            TypesHelper.GetTypes<IParamMessage>().ForEach(x => { services.AddScoped(x); });
            TypesHelper.GetTypes<TelegramController>().ForEach(x => { services.AddScoped(x); });

            ConfigManager configManager = new ConfigManager();

            CommandInfos = configManager.GetCommandInfos().Where(x => x.CommandName != null)
                .ToDictionary(k => k.CommandName, v => v);
            MessageInfos = configManager.GetMessageTypeInfos().GroupBy(x => x.MessageType).ToDictionary(k => k.Key, v => v.ToList());
        }

        public string BotName { get; set; }

        public bool ContainsBotName(string CommandName)
        {
            return GetCommandBotNames(CommandName).Contains(BotName);
        }

        public HashSet<string> GetCommandBotNames(string CommandName)
        {
            if (CommandInfos.ContainsKey(CommandName))
                return CommandInfos[CommandName].BotNames;
            return new HashSet<string>();
        }

        public MethodInfo GetControllerMethod(string CommandName)
        {
            if (CommandInfos.ContainsKey(CommandName))
                return CommandInfos[CommandName].CommandMethod;
            return default;
        }

        public Type GetControllerType(string CommandName)
        {
            if (CommandInfos.ContainsKey(CommandName))
                return CommandInfos[CommandName].Controller;
            return default;
        }

        public bool ContainsCommandName(string CommandName)
        {
            return CommandInfos.ContainsKey(CommandName);
        }

        public List<CommandInfos> GetCommandInfos()
        {
            return CommandInfos.Values.ToList();
        }

        public Dictionary<string, CommandInfos> GetCommandInfosDic()
        {
            return CommandInfos;
        }

        public List<CommandInfos> GetMessageController(MessageType messageType)
        {
            if (MessageInfos.ContainsKey(messageType))
                return MessageInfos[messageType];
            return null;
        }
    }
}
