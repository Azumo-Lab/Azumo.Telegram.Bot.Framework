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

        public List<CommandInfo> GetCommandInfos()
        {
            return CommandInfos;
        }

        public CommandInfo GetCommandInfo(string command)
        {
            return DicCommandInfos.TryGetValue(command, out CommandInfo commandInfo) ? commandInfo : default!;
        }

        public CommandInfo GetCommandInfo(UpdateType updateType)
        {
            return DicUpdateCommandInfos.TryGetValue(updateType, out CommandInfo commandInfo) ? commandInfo : default!;
        }
    }
}
