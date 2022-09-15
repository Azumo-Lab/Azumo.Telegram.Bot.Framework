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
using System.Security.AccessControl;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalFramework.InterFaces;
using Telegram.Bot.Framework.InternalFramework.Models;
using Telegram.Bot.Framework.TelegramAttributes;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.InternalFramework.Mangers
{
    /// <summary>
    /// 
    /// </summary>
    internal class TypeManger : TypeHelper, ITypeManger
    {
        public Guid ID { get; } = Guid.NewGuid();
        private readonly Dictionary<string, CommandInfos> CommandInfos;
        public TypeManger(IServiceCollection services) : base(services)
        {
            GetTypes(typeof(IAction)).ForEach(x => 
            {
                services.AddScoped(typeof(IAction), x);
            });
            CommandInfos = base.GetCommandInfos();
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

        public new List<CommandInfos> GetCommandInfos()
        {
            return CommandInfos.Values.ToList();
        }

        public Dictionary<string, CommandInfos> GetCommandInfosDic()
        {
            return CommandInfos;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    internal class TypeHelper
    {
        protected readonly IServiceCollection services;

        private readonly List<Type> AllType = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();

        private readonly List<Type> ParamMakerType;
        protected List<Type> GetTypes(Type type) =>
            AllType.Where(x => type.IsAssignableFrom(x) && !x.IsAbstract && !x.IsInterface).ToList();

        private List<Type> GetControllers() =>
            GetTypes(typeof(TelegramController));

        private T GetAttribute<T>(MemberInfo memberInfo) where T : Attribute =>
            (T)Attribute.GetCustomAttribute(memberInfo, typeof(T));

        private T GetAttribute<T>(ParameterInfo memberInfo) where T : Attribute =>
            (T)Attribute.GetCustomAttribute(memberInfo, typeof(T));

        private List<Type> GetParamMakerType() =>
            ParamMakerType;

        private MethodInfo[] GetMethods(Type ControllerType) =>
            ControllerType.GetMethods(BindingFlags.Public | BindingFlags.Instance);

        protected TypeHelper(IServiceCollection services)
        {
            this.services = services;
            ParamMakerType = GetTypes(typeof(IParamMaker)).Where(x => Attribute.IsDefined(x, typeof(ParamMakerAttribute))).ToList();
            ParamMakerType.ForEach(x => this.services.AddScoped(x));
        }

        /// <summary>
        /// 获取参数信息
        /// </summary>
        /// <param name="paramsInfos"></param>
        /// <returns></returns>
        private List<ParamInfos> GetParamInfos(ParameterInfo[] paramsInfos)
        {
            List<ParamInfos> paramInfos = new List<ParamInfos>();
            foreach (var parameter in paramsInfos)
            {
                string message = null;
                Type messageType = null;

                ParamAttribute paramAttr = GetAttribute<ParamAttribute>(parameter);
                if (paramAttr != null)
                {
                    if (paramAttr.UseCustom)
                        message = paramAttr.CustomInfos;
                    else
                        message = $"请输入【{paramAttr.CustomInfos}】的值";
                    if (paramAttr.CustomMessageType != null)
                        messageType = paramAttr.CustomMessageType;
                    else
                    {
                        messageType = GetParamMakerType().Where(x =>
                        {
                            ParamMakerAttribute paramMaker = GetAttribute<ParamMakerAttribute>(x);
                            return paramMaker.MakerType.FullName == parameter.ParameterType.FullName;
                        }).FirstOrDefault();
                    }
                }
                else
                {
                    messageType = GetParamMakerType().Where(x =>
                    {
                        ParamMakerAttribute paramMaker = GetAttribute<ParamMakerAttribute>(x);
                        return paramMaker.MakerType.FullName == parameter.ParameterType.FullName;
                    }).FirstOrDefault();
                }
                paramInfos.Add(new ParamInfos
                {
                    MessageInfo = message ?? string.Empty,
                    MessageType = messageType,
                });
            }
            return paramInfos;
        }

        /// <summary>
        /// 开始解析
        /// </summary>
        /// <param name="ControllerType"></param>
        /// <param name="commandInfos"></param>
        private void ConfigInfos(Type ControllerType, List<CommandInfos> commandInfos)
        {
            services.AddScoped(ControllerType);

            List<string> BotNames = new List<string>();

            BotNameAttribute botNameController = GetAttribute<BotNameAttribute>(ControllerType);
            if (botNameController != null)
                BotNames = botNameController.BotName.ToList();

            foreach (MethodInfo method in GetMethods(ControllerType))
            {
                HashSet<string> commandBotNames = new HashSet<string>(BotNames);
                
                BotNameAttribute BotNameAttr = GetAttribute<BotNameAttribute>(method);
                if (BotNameAttr != null)
                {
                    if (BotNameAttr.OverWrite)
                        commandBotNames.Clear();
                    BotNameAttr.BotName.ToList().ForEach(x => commandBotNames.Add(x));
                }
                CommandAttribute commandAttr = GetAttribute<CommandAttribute>(method);
                if (commandAttr == null)
                    continue;

                commandInfos.Add(new CommandInfos
                {
                    CommandMethod = method,
                    CommandName = commandAttr.CommandName.ToLower(),
                    Controller = ControllerType,
                    BotNames = commandBotNames,
                    ParamInfos = GetParamInfos(method.GetParameters()),
                    CommandAttribute = commandAttr,
                });
            }
        }

        /// <summary>
        /// 获取Command信息
        /// </summary>
        /// <returns></returns>
        protected Dictionary<string, CommandInfos> GetCommandInfos()
        {
            List<CommandInfos> commandInfos = new List<CommandInfos>();
            foreach (Type ControllerType in GetControllers())
                ConfigInfos(ControllerType, commandInfos);
            return commandInfos.ToDictionary(k => k.CommandName, v => v);
        }
    }
}
