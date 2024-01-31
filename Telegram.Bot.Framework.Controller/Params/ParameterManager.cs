//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2024>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using System.Collections.Generic;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Framework.Controller.ControllerInvoker;
using Telegram.Bot.Framework.Controller.Params.ParamPipeline;

namespace Telegram.Bot.Framework.Controller.Params;

/// <summary>
/// 
/// </summary>
internal class ParameterManager
{
    /// <summary>
    /// 
    /// </summary>
    private readonly List<BaseParameterGetter> _Params = [];

    /// <summary>
    /// 
    /// </summary>
    private readonly List<object> _param = [];

    /// <summary>
    /// 
    /// </summary>
    public IExecutor Executor { get; set; } = null!;

    /// <summary>
    /// 
    /// </summary>
    public void Init(IExecutor executor)
    {
        Executor = executor;
        _Params.Clear();
        _Params.AddRange(Executor.Parameters);
        _Params.ForEach(x => x.Init());
    }

    public async Task<EnumReadParam> Read(TelegramUserChatContext context)
    {
        var param = _Params.FirstOrDefault();
        if (param == null)
            return EnumReadParam.OK;

        EnumReadParam result;
        result = await ParamPipelineStatic.PipelineController.Execute(param.Result,
            new PipelineModel
            {
                ParameterGetter = param,
                ParamList = _param,
                UserChatContext = context,
            });

        return result;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public object[] GetParams() => _param.ToArray();
}

