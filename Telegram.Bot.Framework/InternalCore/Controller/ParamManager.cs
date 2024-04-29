//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Azumo.Telegram.Bot.Framework>
//
//  This file is part of <Telegram.Bot.Framework>: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.

using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Core;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Controller;
using Telegram.Bot.Framework.Core.PipelineMiddleware;

namespace Telegram.Bot.Framework.InternalCore.Controller;

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

    public object?[] GetParam() => [.. _Param];

    public async Task<bool> Read(TelegramUserContext userContext)
    {
    None:
        var param = getParams.FirstOrDefault();
        if (param == null)
            return true;

        WaitForInput:
        (var result, var paramVal) = await pipelineController[_StateMachine.State].Invoke(new ParamPipelineModel
        {
            Param = param,
            StateMachine = _StateMachine,
            UserContext = userContext,
        });
        if (result)
        {
            if (_StateMachine.State == WaitForInput)
                goto WaitForInput;

            _Param.Add(paramVal);
            getParams.RemoveAt(0);
        }

        if (_StateMachine.State == None)
            goto None;

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
            var result = await input.Param.SendMessage(input.UserContext);
            input.StateMachine.NextState();

            return (result, null);
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
