using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller.Interface;
using Telegram.Bot.Framework.Controller.Models;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller.Internal
{
    internal class FrameworkInfo : IFrameworkInfo
    {
        public FrameworkInfo() { }

        private readonly static List<CommandInfo> CommandInfos = new List<CommandInfo>();

        private readonly static Dictionary<string, CommandInfo> DicCommandInfos = new Dictionary<string, CommandInfo>();
        private readonly static Dictionary<UpdateType, CommandInfo> DicUpdateCommandInfos = new Dictionary<UpdateType, CommandInfo>();

        static FrameworkInfo()
        {
            List<Type> controllerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(x => x.GetTypes())
                .Where(x => typeof(TelegramController).IsAssignableFrom(x))
                .ToList();
            foreach (Type item in controllerTypes)
            {
                MethodInfo[] methodInfos = item.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static);
                foreach (MethodInfo method in methodInfos)
                {
                    CommandInfos.Add(new CommandInfo(method));
                }
            }
            CommandInfos = CommandInfos.Where(x => x.IsCommand).ToList();
            DicCommandInfos = CommandInfos.ToDictionary(x => x.CommandName, x => x)!;
        }

        public async Task CommandInvoke(IServiceProvider serviceProvider, string command, params object[] param)
        {
            TelegramController telegramController = GetController(serviceProvider, command);
            if (telegramController == null)
                return;
            CommandInfo commandInfo = DicCommandInfos[command];
            Delegate @delegate = Delegate.CreateDelegate(typeof(TelegramController), telegramController, commandInfo.CommandMethod);
            Task? task;
            if (param.IsEmpty())
                task = @delegate.DynamicInvoke() as Task;
            else
                task = @delegate.DynamicInvoke(param) as Task;
            if (task != null)
                await task;
        }

        public async Task CommandInvoke(IServiceProvider serviceProvider, UpdateType updateType, params object[] param)
        {
            if (!DicUpdateCommandInfos.TryGetValue(updateType, out CommandInfo commandInfo))
                return;
            Type controllerType = commandInfo.ControllerType!;
            TelegramController telegramController = (TelegramController)ActivatorUtilities.CreateInstance(serviceProvider, controllerType, Array.Empty<object>());
            Delegate @delegate = Delegate.CreateDelegate(typeof(TelegramController), telegramController, commandInfo.CommandMethod);
            Task? task;
            if (param.IsEmpty())
                task = @delegate.DynamicInvoke() as Task;
            else
                task = @delegate.DynamicInvoke(param) as Task;
            if (task != null)
                await task;
        }

        public TelegramController GetController(IServiceProvider serviceProvider, string command)
        {
            if (DicCommandInfos.TryGetValue(command, out CommandInfo commandInfo))
                return (TelegramController)ActivatorUtilities.CreateInstance(serviceProvider, commandInfo.ControllerType!, Array.Empty<object>());
            return default!;
        }
    }
}
