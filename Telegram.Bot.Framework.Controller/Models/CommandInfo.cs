using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Telegram.Bot.Framework.Controller.Attribute;
using Telegram.Bot.Framework.Helper;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Controller.Models
{
    public sealed class CommandInfo
    {
        public bool IsCommand { get; private set; } = default!;

        public bool IsEnble { get; set; } = default!;

        public string? CommandName { get; private set; } = default!;

        public string? CommandDescription { get; private set; } = default!;

        public UpdateType? UpdateType { get; private set; } = default!;

        public MessageType? MessageType { get; private set; } = default!;

        public Type? ControllerType 
        {
            get 
            {
                return CommandMethod?.DeclaringType;
            }
        }

        private MethodInfo? __MethodInfo;
        public MethodInfo? CommandMethod 
        {
            get
            {
                return __MethodInfo;
            }
            set
            {
                if (value!.IsNull()) throw new ArgumentNullException(nameof(CommandMethod));
                __MethodInfo = value;
                HashSet<System.Attribute> attributes = System.Attribute.GetCustomAttributes(ControllerType).ToHashSet();
                foreach (System.Attribute attribute in System.Attribute.GetCustomAttributes(CommandMethod))
                {
                    attributes.Add(attribute);
                }
                Attributes = attributes;

                BotCommandAttribute botCommandAttribute = GetAttributes<BotCommandAttribute>().FirstOrDefault();
                DefaultMessageAttribute defaultMessageAttribute = GetAttributes<DefaultMessageAttribute>().FirstOrDefault();
                DefaultTypeAttribute defaultTypeAttribute = GetAttributes<DefaultTypeAttribute>().FirstOrDefault();
                if (ObjectHelper.HasAllNull(botCommandAttribute, defaultMessageAttribute, defaultTypeAttribute))
                    return;

                IsCommand = true;
                IsEnble = true;
                CommandName = botCommandAttribute?.CommandName;
                CommandDescription = botCommandAttribute?.CommandDescription;
                MessageType = defaultMessageAttribute?.MessageType;
                UpdateType = defaultTypeAttribute?.UpdateType;

                foreach (ParameterInfo? item in __MethodInfo!.GetParameters())
                {
                    ParamInfos.Add(new ParamInfo(item));
                }
            }
        }

        public HashSet<System.Attribute> Attributes { get; set; } = default!;   

        public List<ParamInfo> ParamInfos { get; } = new List<ParamInfo>();

        public IEnumerable<T> GetAttributes<T>() where T : System.Attribute
        {
            foreach (System.Attribute item in Attributes)
            {
                if (item is T)
                {
                    yield return (T)item;
                }
            }
        }

        public CommandInfo(MethodInfo methodInfo)
        {
            CommandMethod = methodInfo;
        }
    }

    public class ParamInfo
    {
        public Type ParamType { get; } = default!;

        public ParamAttribute Attribute { get; set; } = default!;

        public ParamInfo(ParameterInfo parameterInfo)
        {
            ParamType = parameterInfo.ParameterType;
        }
    }

}
