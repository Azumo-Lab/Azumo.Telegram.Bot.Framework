using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Controllers;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts.InternalInterface
{
    [DependencyInjection(ServiceLifetime.Scoped, typeof(IControllerParamManager))]
    internal class ControllerParamManager : IControllerParamManager
    {
        private IControllerParam? Now = null!;
        private List<IControllerParam> __ControllerParamsCopy = new();
        private List<IControllerParam> __ControllerParams = new();
        private ResultEnum __NowResult = ResultEnum.NoStatus;
        public List<IControllerParam> ControllerParams
        {
            get
            {
                return __ControllerParams;
            }
            set
            {
                __ControllerParams = value;
                __ControllerParamsCopy = new List<IControllerParam>(value);
            }
        }
        private BotCommand? __BotCommand;

        private readonly List<object> _params = new();

        public object[] GetParams()
        {
            return _params.ToArray();
        }

        public async Task<ResultEnum> NextParam(TGChat tGChat)
        {
            switch (__NowResult)
            {
                case ResultEnum.NoStatus:
                    try
                    {
                        Now = __ControllerParamsCopy.FirstOrDefault();
                        if (Now == null)
                        {
                            return __NowResult = ResultEnum.Finish;
                        }
                    }
                    finally
                    {
                        if (__ControllerParamsCopy.Any())
                            __ControllerParamsCopy.RemoveAt(0);
                    }
                    __NowResult = ResultEnum.SendMessage;
                    await NextParam(tGChat);
                    break;
                case ResultEnum.SendMessage:
                    if (Now != null)
                    {
                        await Now.SendMessage(tGChat);
                    }
                    __NowResult = ResultEnum.ReceiveParameters;
                    break;
                case ResultEnum.ReceiveParameters:
                    if (Now != null)
                    {
                        _params.Add(await Now.CatchObjs(tGChat));
                    }
                    __NowResult = ResultEnum.NextParam;
                    await NextParam(tGChat);
                    break;
                case ResultEnum.NextParam:
                case ResultEnum.Finish:
                    __NowResult = ResultEnum.NoStatus;
                    await NextParam(tGChat);
                    break;
                default:
                    break;
            }
            return __NowResult;
        }

        public void Clear()
        {
            Now = null;
            ControllerParams = new();
            _params.Clear();
            __NowResult = ResultEnum.NoStatus;
            __BotCommand = null;
        }

        public BotCommand GetBotCommand()
        {
            return __BotCommand!;
        }

        public void SetBotCommand(BotCommand botCommand)
        {
            __BotCommand = botCommand;
        }
    }
}
