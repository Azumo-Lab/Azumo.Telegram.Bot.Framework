using Azumo.Reflection;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.InternalInterface;

namespace Telegram.Bot.Framework.Abstracts
{
    internal class TelegramInstall : ITelegramPartCreator
    {
        public TelegramInstall() 
        {
            
        }
        public void AddBuildService(IServiceCollection services)
        {
            services.AddSingleton<IControllerParamMaker, ControllerParamMaker>();
        }

        public void Build(IServiceCollection services, IServiceProvider builderService)
        {
            services.ScanService();

            ControllerManager controllerManager = new();

            AzReflection<TelegramController> azReflection = AzReflection<TelegramController>.Create();
            foreach (Type Controller in azReflection.FindAllSubclass())
            {
                MethodInfo[] methodInfos = Controller.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (MethodInfo Method in methodInfos)
                {
                    if (Attribute.IsDefined(Method, typeof(BotCommandAttribute)))
                    {
                        controllerManager.InternalCommands.Add(new BotCommand
                        {
                            BotCommandName = (Attribute.GetCustomAttribute(Method, typeof(BotCommandAttribute)) as BotCommandAttribute)!.BotCommandName,
                            Controller = Controller,
                            Func = (telegram, objs) =>
                            {
                                if (Method.Invoke(telegram, objs) is Task task)
                                    return task;
                                return Task.CompletedTask;
                            },
                            MethodInfo = Method,
                            ControllerParams = Method.GetParameters().Select(p =>
                            {
                                IControllerParamMaker controllerParamMaker = builderService.GetService<IControllerParamMaker>()!;
                                if (Attribute.GetCustomAttribute(p, typeof(ParamAttribute)) is ParamAttribute paramAttribute && paramAttribute.ControllerParamSenderType != null)
                                {
                                    IControllerParamSender controllerParamSender = ActivatorUtilities.CreateInstance<IControllerParamSender>(builderService, paramAttribute.ControllerParamSenderType, Array.Empty<object>());
                                    return controllerParamMaker.Make(p.ParameterType, controllerParamSender);
                                }
                                return controllerParamMaker.Make(p.ParameterType, null!);
                            }).ToList(),
                            MessageType = (Attribute.GetCustomAttribute(Method, typeof(BotCommandAttribute)) as BotCommandAttribute)!.MessageType ?? Types.Enums.MessageType.Unknown
                        });
                    }
                }
            }

            foreach (BotCommand item in controllerManager.InternalCommands)
            {
                RuntimeHelpers.PrepareDelegate(item.Func);
                RuntimeHelpers.PrepareMethod(item.MethodInfo.MethodHandle);
            }

            services.AddSingleton<IControllerManager>(controllerManager);
        }
    }

    public static class InstallEX
    {
        public static ITelegramBotBuilder AddBasic(this ITelegramBotBuilder telegramBotBuilder)
        {
            return telegramBotBuilder.AddTelegramPartCreator(new TelegramInstall());
        }
    }
}
