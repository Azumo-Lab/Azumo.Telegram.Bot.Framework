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
//
//  Author: 牛奶

using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Framework.InternalCore.Attritubes;
using Telegram.Bot.Framework.PipelineMiddleware;
using Telegram.Bot.Polling;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.InternalCore.TelegramBotProc
{
    /// <summary>
    /// 
    /// </summary>
    [TelegramBotProc]
    internal class BotStart : IMiddleware<IServiceProvider, Task>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="Next"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task Execute(IServiceProvider input, PipelineMiddlewareDelegate<IServiceProvider, Task> Next)
        {
            var _tokenSource = input.GetRequiredService<CancellationTokenSource>();

            // Bot开始启动
            var botClient = input.GetRequiredService<ITelegramBotClient>();

            if (!await botClient.TestApiAsync(_tokenSource.Token))
                throw new Exception();

            botClient.StartReceiving(input.GetRequiredService<IUpdateHandler>(),
                new ReceiverOptions
                {
                    AllowedUpdates = Array.Empty<UpdateType>(),
                },
                _tokenSource.Token);

            await Next(input);
        }
    }
}
