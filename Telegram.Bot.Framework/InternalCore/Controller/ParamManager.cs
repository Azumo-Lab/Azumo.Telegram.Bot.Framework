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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Core;
using Telegram.Bot.Framework.Core.Attributes;
using Telegram.Bot.Framework.Core.Controller;
using Telegram.Bot.Framework.Core.PipelineMiddleware;

namespace Telegram.Bot.Framework.InternalCore.Controller
{
    /// <summary>
    /// 参数获取管理器实现类
    /// </summary>
    [DependencyInjection(ServiceLifetime.Scoped, ServiceType = typeof(IParamManager))]
    internal class ParamManager : IParamManager
    {
        /// <summary>
        /// 
        /// </summary>
        private readonly List<object?> _Param =
#if NET8_0_OR_GREATER
            [];
#else
            new List<object?>();
#endif
        /// <summary>
        /// 
        /// </summary>
        private readonly List<IGetParam> getParams =
#if NET8_0_OR_GREATER
            [];
#else
            new List<IGetParam>();
#endif
        /// <summary>
        /// 
        /// </summary>
        private readonly StateMachine _StateMachine;

        /// <summary>
        /// 
        /// </summary>
        private const string None = nameof(None);

        /// <summary>
        /// 
        /// </summary>
        private const string WaitForInput = nameof(WaitForInput);

        // 参数流水线
        private readonly IPipelineController<ParamPipelineModel, Task<(bool result, object? paramVal)>> pipelineController;

        /// <summary>
        /// 初始化
        /// </summary>
        public ParamManager()
        {
            _StateMachine = new StateMachine();
            _StateMachine.StateList.AddRange(new List<string> { None, WaitForInput });
            pipelineController = PipelineFactory.GetPipelineBuilder<ParamPipelineModel, Task<(bool result, object? paramVal)>>(() => Task.FromResult<(bool, object?)>((false, null)))
                .Use(new NoneState())
                .CreatePipeline(None)
                .Use(new WaitForInputState())
                .CreatePipeline(WaitForInput)
                .Build();
        }

        /// <summary>
        /// 获取读取到的参数
        /// </summary>
        /// <returns>读取到的参数数组</returns>
        public object?[] GetParam() => _Param.ToArray();

        /// <summary>
        /// 读取参数
        /// </summary>
        /// <param name="userContext">用户上下文</param>
        /// <returns>参数是否读取完成</returns>
        public async Task<bool> Read(TelegramContext userContext)
        {
        // 无状态
        None:
            var param = getParams.FirstOrDefault();
            if (param == null)
                return true;

            // 输入等待状态
            WaitForInput:
            (var result, var paramVal) = await pipelineController[_StateMachine.State].Invoke(new ParamPipelineModel
            {
                Param = param,
                StateMachine = _StateMachine,
                UserContext = userContext,
            });
            if (result)
            {
                // 第一次，发送提示消息时候，如果不需要提示消息，可以直接跳过
                // 进入输入模式
                if (_StateMachine.State == WaitForInput)
                    goto WaitForInput;

                // 读取到参数，并删除第一个参数
                _Param.Add(paramVal);
                getParams.RemoveAt(0);
            }

            // 如果这个参数读取完成，进入下一个参数
            if (_StateMachine.State == None)
                goto None;

            return result;
        }

        /// <summary>
        /// 设置参数列表
        /// </summary>
        /// <param name="paramList">参数列表</param>
        public void SetParamList(IEnumerable<IGetParam> paramList)
        {
            getParams.Clear();
            getParams.AddRange(paramList);
        }

        /// <summary>
        /// 无状态
        /// </summary>
        private class NoneState : IMiddleware<ParamPipelineModel, Task<(bool result, object? paramVal)>>
        {
            public async Task<(bool result, object? paramVal)> Invoke(ParamPipelineModel input, PipelineMiddlewareDelegate<ParamPipelineModel, Task<(bool result, object? paramVal)>> Next)
            {
                var result = await input.Param.SendMessage(input.UserContext);
                input.StateMachine.NextState();

                return (result, null);
            }
        }

        /// <summary>
        /// 等待输入状态
        /// </summary>
        private class WaitForInputState : IMiddleware<ParamPipelineModel, Task<(bool result, object? paramVal)>>
        {
            public async Task<(bool result, object? paramVal)> Invoke(ParamPipelineModel input, PipelineMiddlewareDelegate<ParamPipelineModel, Task<(bool result, object? paramVal)>> Next)
            {
                var result = await input.Param.GetParam(input.UserContext);
                input.StateMachine.Reset();

                return (true, result);
            }
        }

        /// <summary>
        /// 参数输入的参数模型
        /// </summary>
        private class ParamPipelineModel
        {
            /// <summary>
            /// 当前参数获取接口
            /// </summary>
            public IGetParam Param { get; set; } = null!;

            /// <summary>
            /// 用户上下文
            /// </summary>
            public TelegramContext UserContext { get; set; } = null!;

            /// <summary>
            /// 简易状态机器
            /// </summary>
            public StateMachine StateMachine { get; set; } = null!;
        }

        /// <summary>
        /// 简易状态机器
        /// </summary>
        private class StateMachine
        {
            /// <summary>
            /// 当前状态
            /// </summary>
            public string State => StateList[_stateIndex];

            /// <summary>
            /// 状态在数组的位置
            /// </summary>
            private int _stateIndex;

            /// <summary>
            /// 状态数组
            /// </summary>
            public List<string> StateList { get; } =
#if NET8_0_OR_GREATER
                [];
#else
                new List<string>();
#endif

            /// <summary>
            /// 进入下一个状态
            /// </summary>
            public void NextState()
            {
                if (++_stateIndex >= StateList.Count)
                    _stateIndex = 0;
            }

            /// <summary>
            /// 重置状态
            /// </summary>
            public void Reset() =>
                _stateIndex = 0;
        }
    }
}
