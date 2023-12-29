//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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
using System.Reflection;
using System.Runtime.CompilerServices;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstracts.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class BotCommand(IServiceProvider serviceProvider)
    {
        /// <summary>
        /// 
        /// </summary>
        public string BotCommandName
        {
            get
            {
                if (__BotCommandName != null)
                    return __BotCommandName;

                __BotCommandName = BotCommandAttribute?.BotCommandName ?? string.Empty;
                if (string.IsNullOrEmpty(__BotCommandName))
                    BotCommandName = MethodInfo.Name.ToLower();
                return __BotCommandName;
            }
            private set
            {
                if (!string.IsNullOrEmpty(value) && !value.StartsWith('/'))
                    __BotCommandName = $"/{value}";
            }
        }
        private string? __BotCommandName;

        /// <summary>
        /// 
        /// </summary>
        public string Description
        {
            get
            {
                if (__Description != null)
                    return __Description;

                __Description = BotCommandAttribute?.Description ?? string.Empty;
                if (string.IsNullOrEmpty(__Description))
                    __Description = "No Message";
                return __Description;
            }
        }
        private string? __Description;

        /// <summary>
        /// 
        /// </summary>
        public MessageType MessageType
        {
            get
            {
                if (__MessageType != null)
                    return __MessageType.GetValueOrDefault(MessageType.Unknown);

                __MessageType = BotCommandAttribute?.MessageType ?? MessageType.Unknown;
                return __MessageType.GetValueOrDefault(MessageType.Unknown);
            }
        }
        private MessageType? __MessageType;

        /// <summary>
        /// 
        /// </summary>
        public Func<TelegramController, object[], Task> Func
        {
            get
            {
                if (__Func != null)
                    return __Func;

                __Func = (controller, paramsObjs) =>
                {
                    var result = MethodInfo.Invoke(controller, paramsObjs);
                    return result is Task task ? task : Task.CompletedTask;
                };
                RuntimeHelpers.PrepareDelegate(__Func);
                return __Func;
            }
        }
        private Func<TelegramController, object[], Task>? __Func;

        /// <summary>
        /// 
        /// </summary>
        public Func<object[], Task> StaticFun
        {
            get
            {
                if (__StaticFun != null)
                    return __StaticFun;

                __StaticFun = (paramsObjs) =>
                {
                    var result = MethodInfo.Invoke(Target, paramsObjs);
                    return result is Task task ? task : Task.CompletedTask;
                };
                RuntimeHelpers.PrepareDelegate(__StaticFun);
                return __StaticFun;
            }
        }
        private Func<object[], Task>? __StaticFun;

        public object? Target { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public MethodInfo MethodInfo
        {
            get => __MethodInfo!;
            set
            {
                __MethodInfo = value;
                RuntimeHelpers.PrepareMethod(__MethodInfo.MethodHandle);

                AuthenticateAttribute = Attribute.GetCustomAttribute(__MethodInfo, typeof(AuthenticateAttribute)) as AuthenticateAttribute;
                BotCommandAttribute = Attribute.GetCustomAttribute(__MethodInfo, typeof(BotCommandAttribute)) as BotCommandAttribute;
            }
        }
        private MethodInfo? __MethodInfo;

        /// <summary>
        /// 
        /// </summary>
        public Type Controller
        {
            get
            {
                if (__Controller != null)
                    return __Controller;

                __Controller = MethodInfo.DeclaringType;
                return __Controller!;
            }
        }
        private Type? __Controller;

        /// <summary>
        /// 
        /// </summary>
        public List<IControllerParam> ControllerParams 
        {
            get
            {
                if (__ControllerParams != null)
                    return __ControllerParams;

                __ControllerParams = MethodInfo.GetParameters().Select(p =>
                {
                    var controllerParamMaker = serviceProvider.GetService<IControllerParamMaker>()!;
                    if (Attribute.GetCustomAttribute(p, typeof(ParamAttribute)) is ParamAttribute paramAttribute && paramAttribute.Sender != null)
                    {
                        var controllerParamSender = ActivatorUtilities.CreateInstance(serviceProvider, paramAttribute.Sender, []) as IControllerParamSender;
                        return controllerParamMaker.Make(p, controllerParamSender!);
                    }
                    return controllerParamMaker.Make(p, null!);
                }).ToList() ?? [];

                return __ControllerParams;
            }
        }
        private List<IControllerParam>? __ControllerParams;

        /// <summary>
        /// 
        /// </summary>
        public AuthenticateAttribute? AuthenticateAttribute { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public BotCommandAttribute? BotCommandAttribute { get; private set; }

        #region 缓存
        private static readonly List<PropertyInfo> propertyInfos =
        [
            .. typeof(BotCommand)
                        .GetProperties(BindingFlags.Public | BindingFlags.Instance)
        ];
        public void Cache()
        {
            foreach (var propertyInfo in propertyInfos)
                _ = propertyInfo.GetValue(this);
        }
        #endregion
    }
}
