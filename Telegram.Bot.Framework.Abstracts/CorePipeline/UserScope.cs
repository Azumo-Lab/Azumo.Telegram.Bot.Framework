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

using Azumo.Pipeline;
using Azumo.Pipeline.Abstracts;
using Microsoft.Extensions.DependencyInjection;
using Telegram.Bot.Framework.Abstracts.Attributes;
using Telegram.Bot.Framework.Abstracts.Bots;
using Telegram.Bot.Framework.Abstracts.Users;
using Telegram.Bot.Types.Enums;

namespace Telegram.Bot.Framework.Abstracts.CorePipeline
{
    [DependencyInjection(ServiceLifetime.Singleton, typeof(ITelegramService))]
    internal class UserScope : ITelegramService
    {
        public static async Task Invoke(TGChat tGChat)
        {
            var pipelineController = tGChat.UserService.GetRequiredService<IPipelineController<TGChat>>();
            _ = await pipelineController.SwitchTo(tGChat.Type, tGChat);
        }

        public void AddServices(IServiceCollection services) =>
            _ = services.AddScoped(x => PipelineFactory
                .CreateIPipelineBuilder<TGChat>()
                .AddProcedure(new PipelineNull())
                .CreatePipeline(UpdateType.Unknown)
                .AddProcedure(new PipelineControllerInvoke())
                .CreatePipeline(UpdateType.Message)
                .CreatePipeline(UpdateType.ChosenInlineResult)
                .CreatePipeline(UpdateType.CallbackQuery)
                .CreatePipeline(UpdateType.EditedMessage)
                .CreatePipeline(UpdateType.ChannelPost)
                .CreatePipeline(UpdateType.EditedChannelPost)
                .CreatePipeline(UpdateType.ShippingQuery)
                .CreatePipeline(UpdateType.PreCheckoutQuery)
                .CreatePipeline(UpdateType.Poll)
                .CreatePipeline(UpdateType.PollAnswer)
                .CreatePipeline(UpdateType.MyChatMember)
                .CreatePipeline(UpdateType.ChatMember)
                .CreatePipeline(UpdateType.ChatJoinRequest)
                .BuilderPipelineController());
    }
}
