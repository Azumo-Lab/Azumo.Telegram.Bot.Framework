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

using Azumo.Pipeline.Abstracts;
using Telegram.Bot.Framework.Abstracts.Users;

namespace Telegram.Bot.Framework.Abstracts.CorePipeline
{
    /// <summary>
    /// 不进行任何处理
    /// </summary>
    internal class PipelineNull : IProcessAsync<TelegramUserChatContext>
    {
        /// <summary>
        /// 不进行任何处理
        /// </summary>
        /// <param name="t">传入参数</param>
        /// <param name="pipelineController">控制器</param>
        /// <returns>直接对传入参数进行返回</returns>
        public Task<TelegramUserChatContext> ExecuteAsync(TelegramUserChatContext t, IPipelineController<TelegramUserChatContext> pipelineController) =>
            // 结束处理
            pipelineController.StopAsync(t);
    }
}
