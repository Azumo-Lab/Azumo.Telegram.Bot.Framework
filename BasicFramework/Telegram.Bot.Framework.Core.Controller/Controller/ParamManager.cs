using Azumo.SuperExtendedFramework.PipelineMiddleware;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Users;

namespace Telegram.Bot.Framework.Core.Controller.Controller;

[DependencyInjection(ServiceLifetime.Scoped, ServiceType = typeof(IParamManager))]
internal class ParamManager : IParamManager
{
    private readonly List<object?> _Param = [];
    private readonly List<IGetParam> getParams = [];

    private readonly StateMachine _StateMachine;

    private const string None = nameof(None);
    private const string WaitForInput = nameof(WaitForInput);

    private readonly IPipelineController<ParamPipelineModel, Task<(bool result, object? paramVal)>> pipelineController;

    public ParamManager()
    {
        _StateMachine = new StateMachine();
        _StateMachine.StateList.AddRange([None, WaitForInput]);
        pipelineController = PipelineFactory.GetPipelineBuilder<ParamPipelineModel, Task<(bool result, object? paramVal)>>(() => Task.FromResult<(bool, object?)>((false, null)))
            .Use(new NoneState())
            .CreatePipeline(None)
            .Use(new WaitForInputState())
            .CreatePipeline(WaitForInput)
            .Build();
    }

    public object?[] GetParam() => _Param.ToArray();

    public async Task<bool> Read(TelegramUserContext userContext)
    {
        var param = getParams.FirstOrDefault();
        if (param == null)
            return true;

        (var result, var paramVal) = await pipelineController[_StateMachine.State].Invoke(new ParamPipelineModel
        {
            Param = param,
            StateMachine = _StateMachine,
            UserContext = userContext,
        });
        if (result)
        {
            _Param.Add(paramVal);
            getParams.RemoveAt(0);
        }

        return result;
    }
    public void SetParamList(IEnumerable<IGetParam> paramList)
    {
        getParams.Clear();
        getParams.AddRange(paramList);
    }

    private class NoneState : IMiddleware<ParamPipelineModel, Task<(bool result, object? paramVal)>>
    {
        public async Task<(bool result, object? paramVal)> Invoke(ParamPipelineModel input, PipelineMiddlewareDelegate<ParamPipelineModel, Task<(bool result, object? paramVal)>> Next)
        {
            await input.Param.SendMessage(input.UserContext);
            input.StateMachine.NextState();

            return (false, null);
        }
    }

    private class WaitForInputState : IMiddleware<ParamPipelineModel, Task<(bool result, object? paramVal)>>
    {
        public async Task<(bool result, object? paramVal)> Invoke(ParamPipelineModel input, PipelineMiddlewareDelegate<ParamPipelineModel, Task<(bool result, object? paramVal)>> Next)
        {
            var result = await input.Param.GetParam(input.UserContext);
            input.StateMachine.Reset();

            return (true, result);
        }
    }

    private class ParamPipelineModel
    {
        public IGetParam Param { get; set; } = null!;

        public TelegramUserContext UserContext { get; set; } = null!;

        public StateMachine StateMachine { get; set; } = null!;
    }

    private class StateMachine
    {
        public string State => StateList[_stateIndex];

        private int _stateIndex;
        public List<string> StateList { get; } = [];

        public void NextState()
        {
            if (++_stateIndex >= StateList.Count)
                _stateIndex = 0;
        }

        public void Reset() => 
            _stateIndex = 0;
    }
}
