using Azumo.PipelineMiddleware;
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

namespace Telegram.Bot.Framework.Controller.Params.ParamPipeline;
internal class PipelineNone : IMiddleware<PipelineModel, Task<EnumReadParam>>, IMiddlewareName
{
    public PipelinePhase Phase => PipelinePhase.GeneralProcessing;

    public string MiddlewareName => "NONE 类型处理";

    public async Task<EnumReadParam> Execute(PipelineModel input, IPipelineController<PipelineModel, Task<EnumReadParam>> pipelineController)
    {
        await input.ParameterGetter.SendPromptMessage(input.UserChatContext);
        return input.ParameterGetter.Result;
    }
}
