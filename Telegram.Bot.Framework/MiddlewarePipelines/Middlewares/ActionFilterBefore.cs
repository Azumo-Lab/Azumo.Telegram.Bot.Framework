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

using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Bots;
using Telegram.Bot.Framework.Abstract.Middlewares;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.InternalImplementation.Sessions;
using Telegram.Bot.Types;

namespace Telegram.Bot.Framework.MiddlewarePipelines.Middlewares
{
    /// <summary>
    /// 
    /// </summary>
    public class ActionFilterBefore : IMiddleware
    {
        public async Task Execute(ITelegramSession Session, IPipelineController PipelineController)
        {
            List<IFilter> filters = Session.UserService.GetServices<IFilter>().ToList();
            foreach (IFilter item in filters)
                if (await item.FilterBefore(Session))
                    return;

            // 这里回复的消息不是机器人的消息
            Message ReplyToMessage = Session.Update.Message?.ReplyToMessage;
            if (ReplyToMessage != null) // 回复的消息
            {
                ITelegramBot telegramBot = Session.UserService.GetRequiredService<ITelegramBot>();
                if (ReplyToMessage.From.Id != telegramBot.ThisBot.Id)
                {
                    PipelineController.ChangePipeline(nameof(ReplyToMessage));
                    await PipelineController.Next(Session);
                    return;
                }
            }    

            await PipelineController.Next(Session);
        }
    }
}
