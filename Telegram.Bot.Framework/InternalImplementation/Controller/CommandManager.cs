using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Telegram.Bot.Framework.Abstract.Controller;
using Telegram.Bot.Framework.Controller.Attribute;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Framework.InternalImplementation.Controller;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller.Internal
{
    internal class CommandManager : ICommandManager
    {
        public CommandManager() { }

        private static readonly List<CommandInfo> CommandInfos = new();

        private static readonly Dictionary<string, CommandInfo> DicCommandInfos = new();
        private static readonly Dictionary<MessageType, CommandInfo> DicMessageCommandInfos = new();
        private static readonly Dictionary<UpdateType, CommandInfo> DicUpdateCommandInfos = new();

        static CommandManager()
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
            DicMessageCommandInfos = CommandInfos.Where(x =>
            {
                DefaultMessageAttribute defaultMessageAttribute = x.GetAttributes<DefaultMessageAttribute>().FirstOrDefault();
                return defaultMessageAttribute != null;
            }).ToDictionary(x =>
            {
                return x.GetAttributes<DefaultMessageAttribute>().FirstOrDefault().MessageType;
            }, y => y);
            DicUpdateCommandInfos = CommandInfos.Where(x =>
            {
                DefaultTypeAttribute defaultTypeAttribute = x.GetAttributes<DefaultTypeAttribute>().FirstOrDefault();
                if (defaultTypeAttribute == null)
                    return false;

                if (x.ParamInfos.Any())
                    throw new Exception($"方法：{x?.CommandMethod?.Name} 控制器：{x?.ControllerType?.FullName} 参数个数：{x?.ParamInfos.Count} \n目前框架不支持带有参数的默认处理，请将参数删除后重试。");

                return true;
            }).ToDictionary(x =>
            {
                return x.GetAttributes<DefaultTypeAttribute>().FirstOrDefault().UpdateType;
            }, y => y);
        }

        public List<CommandInfo> GetCommandInfos()
        {
            return CommandInfos;
        }

        public CommandInfo GetCommandInfo(string command)
        {
            if (command.IsNullOrEmpty())
                return default!;
            return DicCommandInfos.TryGetValue(command, out CommandInfo commandInfo) ? commandInfo : default!;
        }

        public CommandInfo GetCommandInfo(MessageType messageType)
        {
            if (messageType.IsNull())
                return default!;
            return DicMessageCommandInfos.TryGetValue(messageType, out CommandInfo commandInfo) ? commandInfo : default!;
        }

        public CommandInfo GetCommandInfo(UpdateType messageType)
        {
            if (messageType.IsNull())
                return default!;
            return DicUpdateCommandInfos.TryGetValue(messageType, out CommandInfo commandInfo) ? commandInfo : default!;
        }
    }
}
