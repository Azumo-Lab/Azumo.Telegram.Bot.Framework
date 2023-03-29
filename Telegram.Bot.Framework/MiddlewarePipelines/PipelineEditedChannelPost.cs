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
using System.Threading.Tasks;
using Telegram.Bot.Framework.Abstract.Sessions;
using Telegram.Bot.Framework.InternalImplementation.Sessions;
using Telegram.Bot.Framework.MiddlewarePipelines.Middlewares;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.MiddlewarePipelines
{
    /// <summary>
    /// 
    /// </summary>
    internal class PipelineEditedChannelPost : AbstractMiddlewarePipeline
    {
        public PipelineEditedChannelPost(IServiceProvider serviceProvider) : base(serviceProvider)
        {

        }
        public override UpdateType InvokeType => UpdateType.EditedChannelPost;

        protected override void AddMiddlewareHandles(IServiceProvider serviceProvider)
        {
            AddMiddleware<ActionUpdateTypeInvoke>();
        }

        protected override Task InvokeAction(ITelegramSession session)
        {
            return Task.CompletedTask;
        }
    }
}
