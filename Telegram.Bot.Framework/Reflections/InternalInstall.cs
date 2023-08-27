using System.Linq.Expressions;
using System.Reflection;
using Telegram.Bot.Framework.Abstracts;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Helpers;

namespace Telegram.Bot.Framework.Reflections
{
    internal class InternalInstall
    {
        private readonly static List<Type> AllTypes;

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
            foreach (MethodInfo methodinfo in type.GetMethods( BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static))
            {
                if (Attribute.GetCustomAttribute(methodinfo, typeof(BotCommandAttribute)) is BotCommandAttribute botCommandAttribute)
                {
                    string botcommand = botCommandAttribute.BotCommandName;
                    if (string.IsNullOrEmpty(botcommand))
                        botcommand = $"/{methodinfo.Name.ToLower()}";

                    List<BotCommandParams> botCommandParams = null;
                    ParameterInfo[] param = methodinfo.GetParameters();
                    if (param.Any())
                    {
                        foreach(ParameterInfo paramInfo in param)
                        {
                            botCommandParams.Add(new BotCommandParams
                            {
                                ParameterInfo = paramInfo,
                            });
                        }
                    }

                    Func<TelegramController, object[], Task> func = (controller, objs) =>
                    {
                        if (objs == null || !objs.Any())
                            objs = Array.Empty<object>();
                        object result = methodinfo.Invoke(controller, objs);
                        return result as Task;
                    };

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
