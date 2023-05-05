//  <Telegram.Bot.Framework>
//  Copyright (C) <2022 - 2023>  <Azumo-Lab> see <https://github.com/Azumo-Lab/Telegram.Bot.Framework/>
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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.MiddlewarePipelines
{
    /// <summary>
    /// 
    /// </summary>
    internal class TelegramUpdateReceiver : IUpdateReceiver
    {
        private static readonly Update[] EmptyUpdates = Array.Empty<Update>();
        
        private readonly ITelegramBotClient _botClient;
        private readonly ReceiverOptions _receiverOptions;

        public TelegramUpdateReceiver(
        ITelegramBotClient botClient,
        ReceiverOptions receiverOptions = default)
        {
            _botClient = botClient ?? throw new ArgumentNullException(nameof(botClient));
            _receiverOptions = receiverOptions;
        }

        public async Task ReceiveAsync(IUpdateHandler updateHandler, CancellationToken cancellationToken = default)
        {
            if (updateHandler is null) { throw new ArgumentNullException(nameof(updateHandler)); }

            var allowedUpdates = _receiverOptions?.AllowedUpdates;
            var limit = _receiverOptions?.Limit ?? default;
            var messageOffset = _receiverOptions?.Offset ?? 0;
            var emptyUpdates = EmptyUpdates;

            if (_receiverOptions?.ThrowPendingUpdates is true)
            {
                try
                {
                }
                catch (OperationCanceledException)
                {
                    // ignored
                }
            }

            while (!cancellationToken.IsCancellationRequested)
            {
                var timeout = (int)_botClient.Timeout.TotalSeconds;
                var updates = emptyUpdates;
                try
                {
                    var request = new GetUpdatesRequest
                    {
                        Limit = limit,
                        Offset = messageOffset,
                        Timeout = timeout,
                        AllowedUpdates = allowedUpdates,
                    };
                    updates = await _botClient.MakeRequestAsync(
                        request: request,
                        cancellationToken:
                        cancellationToken
                    ).ConfigureAwait(false);
                }
                catch (OperationCanceledException)
                {
                    // Ignore
                }
                catch (Exception exception)
                {
                    try
                    {
                        await updateHandler.HandlePollingErrorAsync(
                            botClient: _botClient,
                            exception: exception,
                            cancellationToken: cancellationToken
                        ).ConfigureAwait(false);
                    }
                    catch (OperationCanceledException)
                    {
                        // ignored
                    }
                }

                foreach (var update in updates)
                {
                    try
                    {
                        await updateHandler.HandleUpdateAsync(
                            botClient: _botClient,
                            update: update,
                            cancellationToken: cancellationToken
                        ).ConfigureAwait(false);

                        messageOffset = update.Id + 1;
                    }
                    catch (OperationCanceledException)
                    {
                        // ignored
                    }
                }
            }
        }
    }
}
