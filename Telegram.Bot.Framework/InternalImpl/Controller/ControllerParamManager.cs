using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controller;
using Telegram.Bot.Framework.Reflections;

namespace Telegram.Bot.Framework.InternalImpl.Controller
{
    [DependencyInjection(ServiceLifetime.Scoped, typeof(IControllerParamManager))]
    internal class ControllerParamManager : IControllerParamManager
    {
        private readonly List<object> __Params = new();

        BotCommand IControllerParamManager.BotCommand
        {
            get => __BotCommand;
            set => __BotCommand = value;
        }
        int IControllerParamManager.Index
        {
            get => __Index;
            set => __Index = value;
        }
        ParamStauts IControllerParamManager.ParamStauts { get; set; } = ParamStauts.Read;

        private int __Index;
        private BotCommand __BotCommand;

        void IControllerParamManager.AddObject(object obj)
        {
            __Params.Add(obj);
        }

        void IControllerParamManager.Clear()
        {
            __Params.Clear();
            __Index = 0;
            __BotCommand = null;
        }

        object[] IControllerParamManager.GetObjects()
        {
            return __Params.ToArray();
        }
    }
}
