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

using Azumo.Reflection;
using System.Reflection;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.InternalInterface
{
    [DependencyInjection(ServiceLifetime.Singleton, typeof(IControllerParamMaker))]
    internal class ControllerParamMaker : IControllerParamMaker
    {
        private readonly Dictionary<Type, Type> __AllType = [];
        private readonly IServiceProvider ServiceProvider;
        public ControllerParamMaker(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
            AzAttribute<TypeForAttribute> azAttribute = new();
            foreach (var item in azAttribute.GetAllType())
            {
                var typeForAttribute = (TypeForAttribute)Attribute.GetCustomAttribute(item, typeof(TypeForAttribute))!;
                _ = __AllType.TryAdd(typeForAttribute.Type, item);
            }
        }
        public IControllerParam Make(ParameterInfo parameterInfo, IControllerParamSender controllerParamSender)
        {
            IControllerParam controllerParam;
            if (__AllType.TryGetValue(parameterInfo.ParameterType, out var IControllerParamType))
            {
                IControllerParamType ??= typeof(NullControllerParam);
                controllerParam = (IControllerParam)ActivatorUtilities.CreateInstance(ServiceProvider, IControllerParamType, []);
            }
            else
            {
                controllerParam = new NullControllerParam();
            }

            if (controllerParamSender != null)
                controllerParam.ParamSender = controllerParamSender;
            if (Attribute.GetCustomAttribute(parameterInfo, typeof(ParamAttribute)) is ParamAttribute attribute)
                controllerParam.ParamAttribute = attribute;

            return controllerParam;
        }

        /// <summary>
        /// 
        /// </summary>
        private class NullControllerParam : IControllerParam
        {
            /// <summary>
            /// 
            /// </summary>
            public IControllerParamSender ParamSender { get; set; }

            /// <summary>
            /// 
            /// </summary>
            public ParamAttribute ParamAttribute { get; set; }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="tGChat"></param>
            /// <returns></returns>
            public Task<object> CatchObjs(TelegramUserChatContext tGChat) => Task.FromResult<object>(null!);
            
            /// <summary>
            /// 
            /// </summary>
            /// <param name="tGChat"></param>
            /// <param name="paramAttribute"></param>
            /// <returns></returns>
            public Task SendMessage(TelegramUserChatContext tGChat, ParamAttribute paramAttribute) => Task.CompletedTask;
        }
    }
}
