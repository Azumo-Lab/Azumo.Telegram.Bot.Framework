using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Controller.Results;
using Telegram.Bot.Framework.InternalBasicFramework;

namespace Telegram.Bot.Framework.Controller.Params
{
    /// <summary>
    /// 
    /// </summary>
    internal class StatusMachineParamManager : IParamManager
    {
        private GetParamStatusMachine GetParamStatusMachine;

        private readonly List<object?> _param = new List<object?>();

        public object?[] GetParam() => throw new NotImplementedException();

        public async Task<(bool read, IActionResult? actionResult)> Read(TelegramActionContext telegramActionContext)
        {
            var getParam = GetParamStatusMachine.NextState();
            if (getParam == null)
                return (true, null);
            var result = GetParamStatusMachine.State(getParam, telegramActionContext);
            if (result is Task<IActionResult?> resultActionResult)
            {
                return (false, await resultActionResult);
            }
            else if (result is Task<object?> resultObject)
            {
                _param.Add(await resultObject);
                return (false, null);
            }
            else throw new InvalidOperationException();
        }
        public void SetParamList(IEnumerable<IGetParam> paramList) => 
            GetParamStatusMachine = new GetParamStatusMachine(paramList);
    }

    internal class GetParamStatusMachine : StateMachine<Func<IGetParam, TelegramActionContext, object>, IGetParam>
    {
        private static readonly Func<IGetParam, TelegramActionContext, object> _Send =
            (param, TelegramActionContext) => param.SendMessage(TelegramActionContext);

        private static readonly Func<IGetParam, TelegramActionContext, object> _Get =
            (param, TelegramActionContext) => param.GetParam(TelegramActionContext);

        private const string NONE = nameof(NONE);

        private readonly StringArrayStateMachine StringArrayStateMachine =
#if NET8_0_OR_GREATER
            new([nameof(_Send), nameof(_Get)]);
#else
            new StringArrayStateMachine(new[] { NONE, nameof(_Send), nameof(_Get) });
#endif

        private int _Index = -1;

        public override Func<IGetParam, TelegramActionContext, object> State
        {
            get
            {
                if (StringArrayStateMachine.State == nameof(_Get))
                    return _Get;
                else if (StringArrayStateMachine.State == nameof(_Send))
                    return _Send;
                throw new InvalidOperationException();
            }
        }

        private readonly IReadOnlyList<IGetParam> getParams;

        public GetParamStatusMachine(IEnumerable<IGetParam> paramList)
        {
            getParams = new List<IGetParam>(paramList);
            StringArrayStateMachine.EnterState();
        }

        public override IGetParam EnterState() => throw new NotImplementedException();
        public override IGetParam ExitState() => throw new NotImplementedException();
        public override IGetParam NextState()
        {
            StringArrayStateMachine.NextState();
            if (StringArrayStateMachine.State == NONE)
                _Index++;
            return _Index < getParams.Count ? getParams[_Index] : null!;
        }
    }
}
