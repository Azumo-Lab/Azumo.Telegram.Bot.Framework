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

using System.Reflection;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Helpers;

namespace Telegram.Bot.Framework.Reflections
{
    internal class InternalInstall
    {
        private static readonly List<Type> AllTypes;

        static InternalInstall()
        {
            AllTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
        }

        public InternalInstall() { }

        public static void StartInstall()
        {
            List<Type> controllerType = typeof(TelegramController).FindTypeOf();
            foreach (Type item in controllerType)
            {
                Install(item);
            }
        }

        public static void Install(Type type)
        {
            foreach (MethodInfo methodinfo in type.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static))
            {
                if (Attribute.GetCustomAttribute(methodinfo, typeof(BotCommandAttribute)) is BotCommandAttribute botCommandAttribute)
                {
                    string botcommand = botCommandAttribute.BotCommandName;
                    if (string.IsNullOrEmpty(botcommand))
                        botcommand = $"/{methodinfo.Name.ToLower()}";

                    List<BotCommandParams> botCommandParams = new List<BotCommandParams>();
                    ParameterInfo[] param = methodinfo.GetParameters();
                    if (param.Any())
                    {
                        foreach (ParameterInfo paramInfo in param)
                        {
                            botCommandParams.Add(new BotCommandParams
                            {
                                ParameterInfo = paramInfo,
                            });
                        }
                    }

                    Task func(TelegramController controller, object[] objs)
                    {
                        if (objs == null || !objs.Any())
                            objs = Array.Empty<object>();
                        object result = methodinfo.Invoke(controller, objs);
                        return result as Task;
                    }

                    BotCommandRoute.AddBotCommand(new BotCommand
                    {
                        BotCommandName = botcommand,
                        BotCommandParams = botCommandParams,
                        Command = func,
                        MessageType = null,
                        ControllerType = type,
                    });
                }
            }

        }
    }
}
