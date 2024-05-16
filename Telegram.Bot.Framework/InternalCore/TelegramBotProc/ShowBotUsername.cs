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
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalCore.Attritubes;
using Telegram.Bot.Framework.PipelineMiddleware;

namespace Telegram.Bot.Framework.InternalCore.TelegramBotProc
{
    /// <summary>
    /// 
    /// </summary>
    [TelegramBotEndProc]
    internal class ShowBotUsername : IMiddleware<IServiceProvider, Task>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="Next"></param>
        /// <returns></returns>
        public async Task Invoke(IServiceProvider input, PipelineMiddlewareDelegate<IServiceProvider, Task> Next)
        {
            var logger = input.GetService<ILogger<ShowBotUsername>>();

            var botClient = input.GetRequiredService<ITelegramBotClient>();

            var user = await botClient.GetMeAsync();

            logger?.LogInformation("用户名：@{A0}，ID：{A1}，名字：{A2}，正在执行中 ...", user.Username, user.Id, $"{user.FirstName} {user.LastName}");

            await Next(input);
        }
    }
}
